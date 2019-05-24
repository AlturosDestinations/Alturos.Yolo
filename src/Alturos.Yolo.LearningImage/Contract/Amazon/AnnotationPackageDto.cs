using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Contract.Amazon
{
    [DynamoDBTable("ObjectDetectionImageAnnotation")]
    public class AnnotationPackageDto
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public bool IsAnnotated { get; set; }
        public double AnnotationPercentage { get; set; }
        public List<AnnotationImageDto> Images { get; set; }
        public List<string> Tags { get; set; }
    }
}
