using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class Main : Form
    {
        private readonly IBoundingBoxReader _boundingBoxReader;

        private IAnnotationPackageProvider _annotationPackageProvider;

        public Main(IBoundingBoxReader boundingBoxReader)
        {
            this._boundingBoxReader = boundingBoxReader;

            this.InitializeComponent();
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

        private void fromAmazonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._annotationPackageProvider = new AmazonPackageProvider();
            this.LoadPackages();
        }

        private void fromPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._annotationPackageProvider = new WindowsFileSystemPackageProvider();
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
                // Check file size
                long fileSize = 0;

                foreach (var package in packages)
                {
                    var attr = File.GetAttributes(package.PackagePath);
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        var directoryInfo = new DirectoryInfo(package.PackagePath);
                        fileSize += directoryInfo.EnumerateFiles().Sum(file => file.Length);
                    }
                    else
                    {
                        fileSize = new FileInfo(package.PackagePath).Length;
                    }
                }

                // Warn user if he's about to upload a gazillion of files
                var megaBytes = Math.Round(((double)fileSize) / (1024 * 1024), 2);
                if (megaBytes > 100)
                {
                    var dialogResult = MessageBox.Show($"You're about to upload {megaBytes} MB! Are you sure you want to proceed?", $"Uploading {packages.Length} packages", MessageBoxButtons.YesNo);
                    if (dialogResult != DialogResult.Yes)
                    {
                        return;
                    }
                }

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

            int boxCount = 0;
            foreach (var image in images)
            {
                var boxes = image.BoundingBoxes;
                foreach (var box in boxes)
                {
                    boxCount = Math.Max(boxCount, box.ObjectIndex + 1);
                }
            }
            exportDialog.CreateObjectClasses(boxCount);

            exportDialog.Show();
        }

        #endregion

        #region Delegate Callbacks

        private void FolderSelected(AnnotationPackage package)
        {
            if (package == null)
            {
                this.annotationImageListControl.SetImages(null);
                this.annotationImageControl.SetImage(null);

                return;
            }

            if (package.Extracted)
            {
                if (package.Images == null)
                {
                    this.annotationPackageListControl.OpenPackage(package);
                }

                this.annotationImageListControl.SetImages(package.Images);
            }
            else
            {
                this.annotationImageListControl.SetImages(null);
                this.annotationImageControl.SetImage(null);

                this.annotationImageListControl.ShowExtractionWarning(package);
            }

            this.tagListControl.SetTags(package.Info);
        }

        private void ImageSelected(AnnotationImage image)
        {
            this.annotationImageControl.SetImage(image);
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
