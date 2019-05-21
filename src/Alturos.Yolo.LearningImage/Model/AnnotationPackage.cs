using Alturos.Yolo.LearningImage.Helper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationPackage
    {
        private FileInfo[] _files;

        public string PackagePath { get; set; }
        public string DisplayName { get; set; }
        public long TotalBytes { get; set; }

        public bool Downloading { get; set; }
        public double DownloadProgress { get; set; }
        public long TransferredBytes { get; set; }

        public bool Extracted { get; set; }
        public bool IsDirty { get; set; }

        public AnnotationPackageInfo Info { get; set; }

        public double AnnotationPercentage
        {
            get
            {
                return this.Info.AnnotationPercentage;
            }
        }

        public AnnotationImage[] GetImages()
        {
            if (this.Info.Images == null)
            {
                this.Info.Images = new List<AnnotationImageDto>();
            }

            if (this._files == null)
            {
                this._files = Directory.GetFiles(this.PackagePath).Select(o => new FileInfo(o)).OrderBy(o => o.Name.GetFirstNumber()).ToArray();
            }

            var query = from file in this._files
                        join image in this.Info.Images on file.Name equals image.ImageName into j1
                        from annotationImage in j1.DefaultIfEmpty(new AnnotationImageDto())
                        select new AnnotationImage
                        {
                            BoundingBoxes = annotationImage?.BoundingBoxes,
                            DisplayName = file.Name,
                            FilePath = file.FullName,
                            Package = this
                        };

            return query.ToArray();
        }

        public void UpdateAnnotationStatus(AnnotationImage annotationImage)
        {
            var image1 = this.Info.Images.Where(o => o.ImageName.Equals(annotationImage.DisplayName)).FirstOrDefault();
            if (image1 == null)
            {
                image1 = new AnnotationImageDto { ImageName = annotationImage.DisplayName, BoundingBoxes = annotationImage.BoundingBoxes };
                this.Info.Images.Add(image1);
            }
            else
            {
                image1.BoundingBoxes = annotationImage.BoundingBoxes;
            }
            
            var images = this.GetImages();

            var annotationPercentage = this.Info.Images.Count / (double)images.Length * 100.0;

            this.Info.AnnotationPercentage = annotationPercentage;
            this.Info.IsAnnotated = annotationPercentage >= 100;
        }

        public string DirtyDisplayName
        {
            get
            {
                // An asterisk is commonly attached to filenames when they are dirty
                return $"{this.DisplayName}{(this.IsDirty ? "*" : "")}";
            }
        }
    }
}
