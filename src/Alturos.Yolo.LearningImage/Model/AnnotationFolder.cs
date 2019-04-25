using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationFolder
    {
        public bool Selected { get; set; }
        public string DirectoryPath { get; set; }
        public string DirectoryName { get; set; }
        public List<AnnotationImage> Images { get; set; }

        public AnnotationFolder() {
            this.Selected = true;
        }
    }
}
