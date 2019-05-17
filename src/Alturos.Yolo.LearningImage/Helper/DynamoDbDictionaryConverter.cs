using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Helper
{
    public class DynamoDBDictionaryConverter : IPropertyConverter
    {
        private readonly Func<object, DynamoDBEntry> Convert;

        public DynamoDBDictionaryConverter()
        {
            var method = typeof(DynamoDBEntryConversion).GetMethod(nameof(DynamoDBEntryConversion.ConvertToEntry));

            this.Convert = o =>
            {
                var generic = method.MakeGenericMethod(o.GetType());
                return (DynamoDBEntry)generic.Invoke(DynamoDBEntryConversion.V2, new[] { o });
            };
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var dictionary = value as IDictionary<string, object>;
            if (dictionary == null)
                throw new InvalidOperationException();

            var d = new Document();
            foreach (var keyValuePair in dictionary)
            {
                var key = keyValuePair.Key;
                var obj = keyValuePair.Value;
                var dynamoDbEntry = (DynamoDBEntry)null;
                if (obj != null)
                    dynamoDbEntry = Convert(obj);
                d[key] = dynamoDbEntry;
            }
            return d;
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            var d = entry as Document;
            if (d == null)
                throw new InvalidOperationException();

            var result = new Dictionary<string, object>();
            foreach (var keyValuePair in d)
            {
                result.Add(keyValuePair.Key, ConvertValue(keyValuePair.Value));
            }
            return result;
        }

        private object ConvertValue(DynamoDBEntry value)
        {
            var primitive = value as Primitive;
            if (primitive != null)
            {
                switch (primitive.Type)
                {
                    case DynamoDBEntryType.String:
                        return value.AsString();
                    case DynamoDBEntryType.Numeric:
                        {
                            return DynamoDBEntryConversion.V1.TryConvertFromEntry(value, out int integer) ? integer : value.AsDouble();
                        }
                    case DynamoDBEntryType.Binary:
                        return value.AsByteArray();
                }
            }
            else if (value is DynamoDBList)
            {
                var result = new List<object>();
                var list = (DynamoDBList)value;
                foreach (var item in list.Entries)
                {
                    result.Add(this.ConvertValue(item));
                }
                return result;
            }
            else if (value is DynamoDBBool)
            {
                return value.AsBoolean();
            }
            else if (value is DynamoDBNull)
            {
                return null;
            }
            else if (value is Document)
            {
                return this.FromEntry(value);
            }

            throw new InvalidOperationException();
        }
    }
}
