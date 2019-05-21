using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationPackage
    {
        public string PackagePath { get; set; }
        public string DisplayName { get; set; }
        public long TotalBytes { get; set; }

        public bool Downloading { get; set; }
        public double DownloadProgress { get; set; }
        public long TransferredBytes { get; set; }

        public bool Extracted { get; set; }
        public bool IsDirty { get; set; }

        public List<AnnotationImage> Images { get; set; }
        public AnnotationPackageInfo Info { get; set; }

        public double AnnotationPercentage {
            get {
                return this.Info.AnnotationPercentage;
            }
        }
        public string DirtyDisplayName
        {
            get
            {
                // An asterisk is commonly attached to filenames when they are dirty
                return $"{this.DisplayName}{(this.IsDirty ? "*" : "")}";
            }
        }

        public AnnotationPackage() { }

        public AnnotationPackage(AnnotationPackage package)
        {
            this.Downloading = package.Downloading;
            this.DownloadProgress = package.DownloadProgress;
            this.Extracted = package.Extracted;
            this.PackagePath = package.PackagePath;
            this.DisplayName = package.DisplayName;
            this.Images = package.Images;
            this.Info = package.Info;
        }
    }
}
