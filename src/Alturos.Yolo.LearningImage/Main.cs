using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            startupForm.ShowDialog();

            this._annotationPackageProvider = startupForm.AnnotationPackageProvider;
            this._objectClasses = startupForm.ObjectClasses;

            this.InitializeComponent();

            this.LoadPackages();
        }

        #region Initialization and Cleanup

        private void annotationPackageList_Load(object sender, EventArgs e)
        {
            this.annotationPackageListControl.FolderSelected += this.FolderSelected;
        }

        private void annotationImageList_Load(object sender, EventArgs e)
        {
            this.annotationImageListControl.ImageSelected += this.ImageSelected;
            this.annotationImageListControl.ExtractionRequested += this.ExtractionRequested;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.annotationPackageListControl.FolderSelected -= this.FolderSelected;

            this.annotationImageListControl.ImageSelected -= this.ImageSelected;
            this.annotationImageListControl.ExtractionRequested -= this.ExtractionRequested;
        }

        #endregion

        #region Load and Sync

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LoadPackages();
        }

        private void LoadPackages()
        {
            this.annotationPackageListControl.Setup(this._boundingBoxReader, this._annotationPackageProvider);
            this.annotationPackageListControl.LoadPackages();

            this.syncToolStripMenuItem.Enabled = true;
            this.exportToolStripMenuItem.Enabled = true;
        }

        private async void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var packages = this.annotationPackageListControl.GetSelectedPackages().Where(o => o.Extracted).ToArray();
            if (packages.Length > 0)
            {
                // Proceed with syncing
                var syncForm = new SyncForm(this._annotationPackageProvider);
                syncForm.Show();

                await syncForm.Sync(packages);
            }
        }

        #endregion

        #region Export

        private void allImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = this.annotationPackageListControl.GetAllImages();
            this.Export(items);
        }

        private void selectedImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = this.annotationPackageListControl.GetSelectedImages();
            this.Export(items);
        }

        private void Export(AnnotationImage[] images)
        {
            var exportDialog = new ExportDialog();
            exportDialog.CreateImages(images.ToList());
            exportDialog.SetObjectClasses(this._objectClasses);
            exportDialog.Show();
        }

        #endregion

        #region Delegate Callbacks

        private void FolderSelected(AnnotationPackage package)
        {
            if (package == null)
            {
                this.annotationImageListControl.SetImages(null);
                this.annotationImageControl.SetImage(null, null);

                return;
            }

            if (package.Extracted)
            {
                if (package.Images == null)
                {
                    this.annotationPackageListControl.OpenPackage(package);
                }

                this.annotationPackageListControl.UpdateAnnotationStatus(package);
                this.annotationImageListControl.SetImages(package.Images);
            }
            else
            {
                this.annotationImageListControl.SetImages(null);
                this.annotationImageControl.SetImage(null, null);

                this.annotationImageListControl.ShowExtractionWarning(package);
            }

            this.annotationPackageListControl.DataGridView.Refresh();
            this.annotationImageListControl.DataGridView.Refresh();

            this.tagListControl.SetTags(package.Info);
        }

        private void ImageSelected(AnnotationImage image)
        {
            this.annotationImageControl.SetImage(image, this._objectClasses);
        }

        private void ExtractionRequested(AnnotationPackage package)
        {
            var downloadedPackage = this._annotationPackageProvider.DownloadPackage(package);
            this.annotationPackageListControl.UnzipPackage(downloadedPackage);

            // Select folder to apply the images after extraction
            this.FolderSelected(downloadedPackage);
        }

        #endregion
    }
}
