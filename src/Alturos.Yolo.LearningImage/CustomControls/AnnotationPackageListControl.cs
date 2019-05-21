using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationPackageListControl : UserControl
    {
        private static ILog Log = LogManager.GetLogger(typeof(AnnotationPackageListControl));

        public event Action<AnnotationPackage> PackageSelected;

        private IAnnotationPackageProvider _annotationPackageProvider;

        public AnnotationPackageListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.labelLoading.Location = new Point(5, 20);
        }

        public void Setup(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;
        }

        public void RefreshData()
        {
            this.dataGridView1.Refresh();
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

        public async Task LoadPackagesAsync()
        {
            this.labelLoading.Invoke((MethodInvoker)delegate { this.labelLoading.Visible = true; });
            this.dataGridView1.Invoke((MethodInvoker)delegate { this.dataGridView1.Visible = false; });

            var packages = await this._annotationPackageProvider.GetPackagesAsync();

            this.labelLoading.Invoke((MethodInvoker)delegate { this.labelLoading.Visible = false; });

            //foreach (var package in packages)
            //{
            //    //if (package.Extracted && package.Images == null)
            //    //{
            //    //    //this.LoadAnnotationImages(package);
            //    //}
            //}

            if (packages?.Length > 0)
            {
                this.dataGridView1.Invoke((MethodInvoker)delegate
                {
                    this.dataGridView1.Visible = true;
                    this.dataGridView1.DataSource = packages;
                });
            }
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var package = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationPackage;
            this.PackageSelected?.Invoke(package);
        }

        private async void redownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var package = this.dataGridView1.Rows[this.dataGridView1.CurrentCell.RowIndex].DataBoundItem as AnnotationPackage;

            if (package.Downloading)
            {
                return;
            }

            package.Downloading = true;
            this.PackageSelected?.Invoke(package);

            var downloadedPackage = await this._annotationPackageProvider.RefreshPackageAsync(package);
            this.UnzipPackage(downloadedPackage);

            this.PackageSelected?.Invoke(downloadedPackage);
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var item = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as AnnotationPackage;

            if (item.Info.IsAnnotated)
            {
                this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.GreenYellow;
                return;
            }

            if (item.Extracted)
            {
                this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                return;
            }

            this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
        }
    }
}
