using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Contract.Amazon;
using Alturos.Yolo.LearningImage.Forms;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class Main : Form
    {
        private readonly IAnnotationPackageProvider _annotationPackageProvider;
        private readonly AnnotationConfig _annotationConfig;

        public Main()
        {
            this._annotationPackageProvider = new AmazonAnnotationPackageProvider();

            this._annotationConfig = this._annotationPackageProvider.GetAnnotationConfigAsync().GetAwaiter().GetResult();
            if (this._annotationConfig == null)
            {
                this._annotationConfig = new AnnotationConfig();

                using (var configurationForm = new ConfigurationForm())
                {
                    configurationForm.Setup(this._annotationConfig);
                    configurationForm.ShowDialog();
                }
            }

            this.InitializeComponent();
            this.downloadControl.Dock = DockStyle.Fill;

            this.annotationPackageListControl.Setup(this._annotationPackageProvider);

            this.autoplaceAnnotationsToolStripMenuItem.Checked = true;

            this.annotationDrawControl.AutoplaceAnnotations = true;
            this.annotationDrawControl.SetObjectClasses(this._annotationConfig.ObjectClasses);
            this.annotationDrawControl.ShowLabels = true;

            this.showLabelsToolStripMenuItem.Checked = true;
        }

        #region Initialization and Cleanup

        private void Main_Load(object sender, EventArgs e)
        {
            Task.Run(async () => await this.LoadPackagesAsync());
            this.RegisterEvents();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var confirmClosing = ConfirmDiscardingUnsavedChanges();
            if (!confirmClosing)
            {
                e.Cancel = true;
            }
        }

        private bool ConfirmDiscardingUnsavedChanges()
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
                    return false;
                }
            }

            return true;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.UnregisterEvents();
        }

        private void RegisterEvents()
        {
            this.annotationPackageListControl.PackageSelected += this.PackageSelected;

            this.annotationImageListControl.ImageSelected += this.ImageSelected;
            this.downloadControl.ExtractionRequested += this.ExtractionRequestedAsync;

            this.annotationDrawControl.ImageEdited += this.ImageEdited;

            this.tagListControl.TagsRequested += this.TagsRequested;
        }

        private void UnregisterEvents()
        {
            this.annotationPackageListControl.PackageSelected -= this.PackageSelected;

            this.annotationImageListControl.ImageSelected -= this.ImageSelected;
            this.downloadControl.ExtractionRequested -= this.ExtractionRequestedAsync;

            this.annotationDrawControl.ImageEdited -= this.ImageEdited;

            this.tagListControl.TagsRequested -= this.TagsRequested;
        }

        #endregion

        #region Load and Sync

        private async void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await this.LoadPackagesAsync();
        }

        private async Task LoadPackagesAsync()
        {
            this.EnableMainMenu(false);
            await this.annotationPackageListControl.LoadPackagesAsync();
            this.EnableMainMenu(true);
        }

        private void EnableMainMenu(bool enable)
        {
            this.Invoke((MethodInvoker)delegate {
                this.menuStripMain.Enabled = enable;
                this.exportToolStripMenuItem.Enabled = enable;
                this.annotationPackageListControl.Enabled = enable;
                this.annotationImageListControl.Enabled = enable;
                this.annotationDrawControl.Enabled = enable;
                this.downloadControl.Enabled = enable;
                this.tagListControl.Enabled = enable;
            });
        }

        private void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var packages = this.annotationPackageListControl.GetAllPackages().Where(o => o.IsDirty).ToArray();
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
            //var images = this.annotationPackageListControl.GetAllImages();

            using (var exportDialog = new ExportDialog(this._annotationPackageProvider))
            {
                //exportDialog.CreateImages(images.ToList());
                //exportDialog.SetObjectClasses(this._objectClasses);
                exportDialog.ShowDialog();
            }
        }

        #endregion

        #region Upload

        private void addPackageStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "ZIP files (*.zip)|*.zip"
            })
            {

                var dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    var file = openFileDialog.FileName;
                    this._annotationPackageProvider.UploadPackageAsync(file);
                }
            }
        }

        #endregion

        #region Configuration

        private void AutoplaceAnnotationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.autoplaceAnnotationsToolStripMenuItem.Checked = !this.autoplaceAnnotationsToolStripMenuItem.Checked;
            this.annotationDrawControl.AutoplaceAnnotations = this.autoplaceAnnotationsToolStripMenuItem.Checked;
        }

        private void ShowLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.showLabelsToolStripMenuItem.Checked = !this.showLabelsToolStripMenuItem.Checked;
            this.annotationDrawControl.ShowLabels = this.showLabelsToolStripMenuItem.Checked;
        }

        private async void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var configurationForm = new ConfigurationForm())
            {
                configurationForm.Setup(this._annotationConfig);
                var dialogResult = configurationForm.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    await this._annotationPackageProvider.SetAnnotationConfigAsync(this._annotationConfig);
                }
            }
        }

        #endregion

        #region Delegate Callbacks

        private void PackageSelected(AnnotationPackage package)
        {
            //// Sync
            //if (this._selectedPackage != null && this._selectedPackage.IsDirty)
            //{
            //    var dialogResult = MessageBox.Show("Do you want to sync now?", "Sync Request", MessageBoxButtons.YesNo);
            //    if (dialogResult == DialogResult.Yes)
            //    {
            //        var syncForm = new SyncForm(this._annotationPackageProvider);
            //        syncForm.Show();

            //        var packageEdited = this._selectedPackage;
            //        Task.Run(() => syncForm.Sync(new AnnotationPackage[] { packageEdited }));
            //    }
            //}

            //this._selectedPackage = package;

            this.annotationImageListControl.Hide();
            this.downloadControl.Hide();

            this.annotationImageListControl.Reset();
            this.annotationDrawControl.Reset();

            if (package == null)
            {
                return;
            }

            this.tagListControl.SetTags(package);

            if (package.Extracted)
            {
                this.annotationImageListControl.SetPackage(package);
                this.annotationImageListControl.Show();

                this.annotationPackageListControl.RefreshData();
            }
            else
            {
                this.downloadControl.ShowDownloadDialog(package);
            }
        }

        private void ImageSelected(AnnotationImage image)
        {
            if (image == null)
            {
                return;
            }

            this.annotationDrawControl.SetImage(image);
            this.annotationDrawControl.ApplyCachedBoundingBox();

            if (image.BoundingBoxes == null)
            {
                image.BoundingBoxes = new List<AnnotationBoundingBox>();
                this.ImageEdited(image);
            }
        }

        private void ImageEdited(AnnotationImage annotationImage)
        {
            if (annotationImage == null)
            {
                return;
            }

            annotationImage.Package.IsDirty = true;

            annotationImage.Package.UpdateAnnotationStatus(annotationImage);
            this.annotationPackageListControl.RefreshData();
        }

        private async Task ExtractionRequestedAsync(AnnotationPackage package)
        {
            var downloadedPackage = await this._annotationPackageProvider.DownloadPackageAsync(package);
            this.annotationPackageListControl.UnzipPackage(downloadedPackage);

            // Select folder to apply the images after extraction
            this.PackageSelected(downloadedPackage);
        }

        private List<string> TagsRequested()
        {
            var form = new TagSelectionForm();
            form.Setup(this._annotationConfig);
            form.ShowDialog();

            return form.SelectedTags;
        }

        #endregion
    }
}
