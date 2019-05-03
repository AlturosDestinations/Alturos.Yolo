using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationImageDto
    {
        public string FilePath { get; set; }
        public List<AnnotationBoundingBox> BoundingBoxes { get; set; }
    }
}
