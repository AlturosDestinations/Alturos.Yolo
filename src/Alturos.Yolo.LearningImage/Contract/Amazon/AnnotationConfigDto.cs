using Alturos.Yolo.LearningImage.Model;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Contract.Amazon
{
    [DynamoDBTable("ObjectDetectionImageAnnotation")]
    internal class AnnotationConfigDto
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public List<ObjectClass> ObjectClasses { get; set; }
        public List<string> Tags { get; set; }

        public AnnotationConfigDto()
        {
            this.ObjectClasses = new List<ObjectClass>();
            this.Tags = new List<string>();
        }
    }
}
