using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    [DynamoDBTable("ObjectDetectionImageAnnotation")]
    public class AnnotationPackageInfo
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Weather { get; set; }
        public List<string> Color { get; set; }
        public string Driver { get; set; }
        public string Device { get; set; }
        public string Flag { get; set; }
        public bool IsAnnotated { get; set; }
    }
}
