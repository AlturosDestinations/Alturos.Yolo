using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
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
        private AnnotationPackage _selectedPackage;

        public Main(IBoundingBoxReader boundingBoxReader)
        {
            this._boundingBoxReader = boundingBoxReader;

            var startupForm = new StartupForm();
            startupForm.StartPosition = FormStartPosition.CenterScreen;
            var dialogResult = startupForm.ShowDialog(this);

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

        private void annotationImageControl_Load(object sender, EventArgs e)
        {
            this.annotationImageControl.PackageEdited += this.PackageEdited;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var unsyncedPackages = this.annotationPackageListControl.GetAllPackages().Where(o => o.IsDirty);
            if (unsyncedPackages.Any())
            {
                var sb = new StringBuilder();
                foreach (var package in unsyncedPackages)
                {
                    sb.AppendLine(package.DisplayName);
                }

                var dialogResult = MessageBox.Show(
                    "The following packages have been modified and not synced yet:\n\n" +
                        $"{sb.ToString()}\n\n" +
                        "If you close now any unsynced package will be lost.\n\nClose anyway and discard your changes?",
                    "Discard unsynced changes?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.annotationPackageListControl.PackageSelected -= this.PackageSelected;

            this.annotationImageListControl.ImageSelected -= this.ImageSelected;
            this.downloadControl.ExtractionRequested -= this.ExtractionRequestedAsync;

            this.annotationImageControl.PackageEdited -= this.PackageEdited;
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

            this.Invoke((MethodInvoker)delegate {
                this.syncToolStripMenuItem.Enabled = true;
                this.exportToolStripMenuItem.Enabled = true;
                this.downloadControl.Enabled = true;
            });
        }

        private void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var packages = this.annotationPackageListControl.GetAllPackages().Where(o => o.Extracted && o.IsDirty).ToArray();
            if (packages.Length > 0)
            {
                // Proceed with syncing
                var sb = new StringBuilder();
                foreach (var package in packages)
                {
                    sb.AppendLine(package.DisplayName);
                }

                var dialogResult = MessageBox.Show($"Do you want to sync the following packages?\n\n{sb.ToString()}", "Confirm syncing", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var syncForm = new SyncForm(this._annotationPackageProvider);
                    syncForm.Show();

                    _ = Task.Run(() => syncForm.Sync(packages));
                }
            }
            else
            {
                MessageBox.Show("There are no unchanged packages to sync.", "Nothing to sync!");
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
            // Sync
            if (this._selectedPackage != null && this._selectedPackage.IsDirty)
            {
                var dialogResult = MessageBox.Show("Do you want to sync now?", "Sync Request", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var syncForm = new SyncForm(this._annotationPackageProvider);
                    syncForm.Show();

                    var packageEdited = this._selectedPackage;
                    Task.Run(() => syncForm.Sync(new AnnotationPackage[] { packageEdited }));
                }
            }

            this._selectedPackage = package;

            this.annotationImageListControl.Hide();
            this.downloadControl.Hide();

            this.annotationImageListControl.SetImages(null);

            this.annotationImageControl.SetPackage(null);
            this.annotationImageControl.SetImage(null, null);

            if (package == null)
            {
                return;
            }

            this.tagListControl.SetTags(package.Info);

            if (package.Extracted)
            {
                this.annotationPackageListControl.SetAnnotationMetadata(package);

                if (package.Images == null)
                {
                    this.annotationPackageListControl.LoadAnnotationImages(package);
                }

                this.annotationImageListControl.SetImages(package.Images);
                this.annotationImageListControl.DataGridView.Refresh();
                this.annotationImageListControl.Show();

                this.annotationImageControl.SetPackage(package);
            }
            if (package.Downloading)
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
            this.annotationImageControl.ApplyCachedBoundingBox();

            if (image.BoundingBoxes == null)
            {
                image.BoundingBoxes = new List<AnnotationBoundingBox>();
                this.PackageEdited(this._selectedPackage);
            }
        }

        private void PackageEdited(AnnotationPackage package)
        {
            package.IsDirty = true;
            this.annotationPackageListControl.UpdateAnnotationPercentage(package);
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
