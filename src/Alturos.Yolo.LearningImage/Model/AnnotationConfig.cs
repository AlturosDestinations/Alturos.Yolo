using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    [DynamoDBTable("AnnotationConfiguration")]
    public class AnnotationConfig
    {
        public List<ObjectClass> ObjectClasses { get; set; }
        public List<Tag> Tags { get; set; }

        public AnnotationConfig()
        {
            this.ObjectClasses = new List<ObjectClass>();
            this.Tags = new List<Tag>();
        }
    }
}
