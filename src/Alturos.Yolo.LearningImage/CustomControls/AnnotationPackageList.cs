using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationPackageList : UserControl
    {
        public Action<AnnotationPackage> FolderSelected { get; set; }

        private IBoundingBoxReader _boundingBoxReader;
        private IAnnotationPackageProvider _annotationPackageProvider;

        public AnnotationPackageList()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }

        public void Initialize(IBoundingBoxReader boundingBoxReader, IAnnotationPackageProvider annotationPackageProvider)
        {
            this._boundingBoxReader = boundingBoxReader;
            this._annotationPackageProvider = annotationPackageProvider;
        }

        public AnnotationImage[] GetAll()
        {
            var items = new List<AnnotationImage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var package = row.DataBoundItem as AnnotationPackage;
                if (package.Extracted)
                {
                    items.AddRange(package.Images);
                }
            }

            return items.ToArray();
        }

        public AnnotationImage[] GetSelected()
        {
            var items = new List<AnnotationImage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var package = row.DataBoundItem as AnnotationPackage;
                if (package.Extracted && package.Selected) {
                    items.AddRange(package.Images.Where(o => o.Selected));
                }
            }

            return items.ToArray();
        }

        public void LoadPackages()
        {
            var packages = this._annotationPackageProvider.GetPackages();
            this.dataGridView1.DataSource = packages;
        }

        public void UnzipPackage(AnnotationPackage package)
        {
            var downloadedPackage = this._annotationPackageProvider.DownloadPackage(package);
            var zipFilePath = downloadedPackage.PackagePath;

            var extractedPackagePath = Path.Combine(Path.GetDirectoryName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (Directory.Exists(extractedPackagePath))
            {
                Directory.Delete(extractedPackagePath, true);
            }

            ZipFile.ExtractToDirectory(downloadedPackage.PackagePath, extractedPackagePath);
            File.Delete(zipFilePath);

            downloadedPackage.Extracted = true;
            downloadedPackage.PackagePath = extractedPackagePath;
        }

        public void OpenPackage(AnnotationPackage package)
        {
            if (!package.Extracted)
            {
                return;
            }

            var files = Directory.GetFiles(package.PackagePath, "*.*", SearchOption.TopDirectoryOnly);
            var items = files.Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).Select(o => new AnnotationImage
            {
                FilePath = o,
                DisplayName = new FileInfo(o).Name,
                BoundingBoxes = this._boundingBoxReader.GetBoxes(this._boundingBoxReader.GetDataPath(o)).ToList()
            }).ToList();

            if (items.Count == 0)
            {
                return;
            }

            package.Images = items;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var package = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationPackage;
            this.FolderSelected?.Invoke(package);
        }
    }
}
