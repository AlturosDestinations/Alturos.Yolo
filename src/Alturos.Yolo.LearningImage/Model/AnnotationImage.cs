using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationImage
    {
        public bool Selected { get; set; }
        public string FilePath { get; set; }
        public string DisplayName { get; set; }
        public List<AnnotationBoundingBox> BoundingBoxes { get; set; }

        public AnnotationImage()
        {
            Selected = true;
        }

        public AnnotationImage(AnnotationImage image)
        {
            Selected = true;
            FilePath = image.FilePath;
            DisplayName = image.DisplayName;
            BoundingBoxes = image.BoundingBoxes;
        }
    }
}
