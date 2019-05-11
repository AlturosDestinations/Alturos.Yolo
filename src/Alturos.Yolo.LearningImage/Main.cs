using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class Main : Form
    {
        private readonly IBoundingBoxReader _boundingBoxReader;
        private readonly IAnnotationPackageProvider _annotationPackageProvider;
        private readonly List<ObjectClass> _objectClasses;

        public Main(IBoundingBoxReader boundingBoxReader)
        {
            this._boundingBoxReader = boundingBoxReader;

            var startupForm = new StartupForm();
            var dialogResult = startupForm.ShowDialog();

            this._annotationPackageProvider = startupForm.AnnotationPackageProvider;
            this._objectClasses = startupForm.ObjectClasses;

            this.InitializeComponent();
            this.downloadControl.Dock = DockStyle.Fill;

            if (dialogResult == DialogResult.OK)
            {
                this.CreateYoloObjectNames();
                Task.Run(async () => await this.LoadPackagesAsync());
            }
        }

        #region Initialization and Cleanup

        private void annotationPackageList_Load(object sender, EventArgs e)
        {
            this.annotationPackageListControl.PackageSelected += this.PackageSelected;
        }

        private void annotationImageList_Load(object sender, EventArgs e)
        {
            this.annotationImageListControl.ImageSelected += this.ImageSelected;
            this.downloadControl.ExtractionRequested += this.ExtractionRequestedAsync;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.annotationPackageListControl.PackageSelected -= this.PackageSelected;

            this.annotationImageListControl.ImageSelected -= this.ImageSelected;
            this.downloadControl.ExtractionRequested -= this.ExtractionRequestedAsync;
        }

        private void CreateYoloObjectNames()
        {
            var sb = new StringBuilder();
            foreach (var objectClass in this._objectClasses)
            {
                sb.AppendLine(objectClass.Name);
            }

            var yoloMarkPath = @"yolomark\data";
            if (!Directory.Exists(yoloMarkPath))
            {
                Directory.CreateDirectory(yoloMarkPath);
            }

            File.WriteAllText(Path.Combine(yoloMarkPath, "obj.names"), sb.ToString());
        }

        #endregion

        #region Load and Sync

        private async void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await this.LoadPackagesAsync();
        }

        private async Task LoadPackagesAsync()
        {
            this.annotationPackageListControl.Setup(this._boundingBoxReader, this._annotationPackageProvider, this._objectClasses);
            await this.annotationPackageListControl.LoadPackagesAsync();

            this.syncToolStripMenuItem.Enabled = true;
            this.exportToolStripMenuItem.Enabled = true;
        }

        private async void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var packages = this.annotationPackageListControl.GetAllPackages().Where(o => o.Extracted && o.Info.IsAnnotated).ToArray();
            if (packages.Length > 0)
            {
                // Proceed with syncing
                var sb = new StringBuilder();
                foreach (var package in packages)
                {
                    sb.AppendLine(package.DisplayName);
                }

                var dialogResult = MessageBox.Show($"Do you want to sync the following packages?\n\n{sb.ToString()}", "Confirm syncing", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.OK)
                {
                    var syncForm = new SyncForm(this._annotationPackageProvider);
                    syncForm.Show();

                    await syncForm.Sync(packages);
                }
            }
            else
            {
                MessageBox.Show("There are no annoted packages to sync.", "Nothing to sync!");
            }
        }

        #endregion

        #region Export

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var images = this.annotationPackageListControl.GetAllImages();

            var exportDialog = new ExportDialog();
            exportDialog.CreateImages(images.ToList());
            exportDialog.SetObjectClasses(this._objectClasses);
            exportDialog.Show();
        }

        #endregion

        #region Upload

        private void addPackageStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "ZIP files (*.zip)|*.zip"
            };

            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var file = openFileDialog.FileName;
                this._annotationPackageProvider.UploadPackageAsync(file);
            }
        }

        #endregion

        #region Delegate Callbacks

        private void PackageSelected(AnnotationPackage package)
        {
            this.annotationImageListControl.Hide();
            this.downloadControl.Hide();

            this.annotationImageListControl.SetImages(null);
            this.annotationImageControl.SetImage(null, null);

            if (package == null)
            {
                return;
            }

            this.tagListControl.SetTags(package.Info);

            if (package.Extracted)
            {
                if (package.Images == null)
                {
                    this.annotationPackageListControl.AnalyzeAnnotationStatus(package);
                }

                this.annotationImageListControl.SetImages(package.Images);
                this.annotationImageListControl.DataGridView.Refresh();
                this.annotationImageListControl.Show();
            }
            else
            {
                //TODO: On selected item change in the datagrid always new tasks started...
                this.downloadControl.ShowDownloadDialog(package);
            }

            //TODO: Why refresh?
            this.annotationPackageListControl.DataGridView.Refresh();
        }

        private void ImageSelected(AnnotationImage image)
        {
            this.annotationImageControl.SetImage(image, this._objectClasses);
        }

        private async Task ExtractionRequestedAsync(AnnotationPackage package)
        {
            var downloadedPackage = await this._annotationPackageProvider.DownloadPackageAsync(package);
            this.annotationPackageListControl.UnzipPackage(downloadedPackage);

            // Select folder to apply the images after extraction
            this.PackageSelected(downloadedPackage);
        }

        #endregion
    }
}
