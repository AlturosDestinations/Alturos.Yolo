using Alturos.Yolo.LearningImage.Helper;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    [DynamoDBTable("ObjectDetectionImageAnnotation")]
    public class AnnotationPackageInfo
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public bool IsAnnotated { get; set; }
        public double AnnotationPercentage { get; set; }
        public List<AnnotationImageDto> ImageDtos { get; set; }
        [DynamoDBProperty(typeof(DynamoDBDictionaryConverter))]
        public Dictionary<string, object> Tags { get; set; }
    }
}
