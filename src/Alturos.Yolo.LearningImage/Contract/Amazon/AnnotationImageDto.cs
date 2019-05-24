using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Contract.Amazon
{
    public class AnnotationImageDto
    {
        public string ImageName { get; set; }
        public List<AnnotationBoundingBox> BoundingBoxes { get; set; }
    }
}
