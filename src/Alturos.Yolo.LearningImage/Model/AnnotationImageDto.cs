using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationImageDto
    {
        public string ImageName { get; set; }
        public List<AnnotationBoundingBox> BoundingBoxes { get; set; }
    }
}
