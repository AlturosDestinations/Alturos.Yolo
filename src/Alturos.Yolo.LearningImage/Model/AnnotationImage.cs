using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationImage
    {
        public string FilePath { get; set; }
        public string DisplayName { get; set; }
        public List<AnnotationBoundingBox> BoundingBoxes { get; set; }

        public AnnotationImage() { }

        public AnnotationImage(AnnotationImage image)
        {
            this.FilePath = image.FilePath;
            this.DisplayName = image.DisplayName;
            this.BoundingBoxes = image.BoundingBoxes;
        }
    }
}
