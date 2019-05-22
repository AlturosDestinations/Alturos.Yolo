using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    [DynamoDBTable("AnnotationConfiguration")]
    public class AnnotationConfig
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public List<ObjectClass> ObjectClasses { get; set; }
        public List<Tag> Tags { get; set; }

        public AnnotationConfig()
        {
            this.ObjectClasses = new List<ObjectClass>();
            this.Tags = new List<Tag>();
        }
    }
}
