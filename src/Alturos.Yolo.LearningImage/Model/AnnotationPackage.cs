using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationPackage
    {
        public bool Selected { get; set; }
        public bool Extracted { get; set; }
        public string PackagePath { get; set; }
        public string DisplayName { get; set; }
        public List<AnnotationImage> Images { get; set; }
        public AnnotationPackageInfo Info { get; set; }
        public double AnnotationPercentage { get { return Info.AnnotationPercentage; } }

        public AnnotationPackage() {
            this.Selected = true;
        }

        public AnnotationPackage(AnnotationPackage package)
        {
            this.Selected = true;
            this.Extracted = package.Extracted;
            this.PackagePath = package.PackagePath;
            this.DisplayName = package.DisplayName;
            this.Images = package.Images;
            this.Info = package.Info;
        }
    }
}
