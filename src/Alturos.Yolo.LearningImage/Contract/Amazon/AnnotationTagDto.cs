using Amazon.DynamoDBv2.DataModel;

namespace Alturos.Yolo.LearningImage.Contract.Amazon
{
    [DynamoDBTable("ImageAnnotationPackageTag")]
    public class AnnotationTagDto
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Tag { get; set; }
    }
}
