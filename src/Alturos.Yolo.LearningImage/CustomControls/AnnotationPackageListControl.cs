using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationPackageListControl : UserControl
    {
        public Action<AnnotationPackage> FolderSelected { get; set; }

        private IBoundingBoxReader _boundingBoxReader;
        private IAnnotationPackageProvider _annotationPackageProvider;

        public AnnotationPackageListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }

        public void Setup(IBoundingBoxReader boundingBoxReader, IAnnotationPackageProvider annotationPackageProvider)
        {
            this._boundingBoxReader = boundingBoxReader;
            this._annotationPackageProvider = annotationPackageProvider;
        }

        public AnnotationPackage[] GetAllPackages()
        {
            var items = new List<AnnotationPackage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var package = row.DataBoundItem as AnnotationPackage;
                items.Add(package);
            }

            return items.ToArray();
        }

        public AnnotationPackage[] GetSelectedPackages()
        {
            var items = new List<AnnotationPackage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var package = row.DataBoundItem as AnnotationPackage;
                if (package.Selected)
                {
                    items.Add(package);
                }
            }

            return items.ToArray();
        }

        public AnnotationImage[] GetAllImages()
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

        public AnnotationImage[] GetSelectedImages()
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
            var zipFilePath = package.PackagePath;

            var extractedPackagePath = Path.Combine(Path.GetDirectoryName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (Directory.Exists(extractedPackagePath))
            {
                Directory.Delete(extractedPackagePath, true);
            }

            ZipFile.ExtractToDirectory(package.PackagePath, extractedPackagePath);
            File.Delete(zipFilePath);

            package.Extracted = true;
            package.PackagePath = extractedPackagePath;
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

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var package = row.DataBoundItem as AnnotationPackage;
                package.Selected = true;
            }

            this.dataGridView1.Refresh();
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var package = row.DataBoundItem as AnnotationPackage;
                package.Selected = false;
            }

            this.dataGridView1.Refresh();
        }

        private void redownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FolderSelected?.Invoke(null);

            var package = this.dataGridView1.Rows[this.dataGridView1.CurrentCell.RowIndex].DataBoundItem as AnnotationPackage;

            var downloadedPackage = this._annotationPackageProvider.RefreshPackage(package);
            this.UnzipPackage(downloadedPackage);

            downloadedPackage.Images = null;
            this.FolderSelected?.Invoke(downloadedPackage);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            var package = this.dataGridView1.CurrentRow?.DataBoundItem as AnnotationPackage;
            if (package == null)
            {
                return;
            }

            if (!package.Extracted)
            {
                return;
            }

            var arguments = $@"""{package.PackagePath}"" C:\Users\kneluk\Documents\GitHub\Yolo_mark\x64\Release\data\train.txt C:\Users\kneluk\Documents\GitHub\Yolo_mark\x64\Release\data\obj.names";
            var process = Process.Start(@"C:\Users\kneluk\Documents\GitHub\Yolo_mark\x64\Release\yolo_mark.exe", arguments);
            process.WaitForExit();

            package.Images = null;
            this.FolderSelected?.Invoke(package);
        }
    }
}
