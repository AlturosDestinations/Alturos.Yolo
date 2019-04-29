using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class Main : Form
    {
        private readonly IBoundingBoxReader _boundingBoxReader;

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

        private void fromAmazonS3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LoadPackages(new AmazonS3PackageProvider());
        }

        private void fromPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LoadPackages(new WindowsFileSystemPackageProvider());
        }

        private void LoadPackages(IAnnotationPackageProvider annotationPackageProvider)
        {
            this.annotationPackageListControl.Setup(this._boundingBoxReader, annotationPackageProvider);
            this.annotationPackageListControl.LoadPackages();
        }

        private void toAmazonS3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SyncPackages(new AmazonS3PackageProvider());
        }

        private void SyncPackages(IAnnotationPackageProvider annotationPackageProvider)
        {
            annotationPackageProvider.SyncPackages(this.annotationPackageListControl.GetAllPackages());
        }

        #endregion

        #region Export and Tags

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

        private void applyFromExcelSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var packages = this.annotationPackageListControl.GetAllPackages();
            this.tagListControl.LoadFromExcel(packages);
        }

        #endregion

        #region Delegate Callbacks

        private void FolderSelected(AnnotationPackage package)
        {
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
            this.annotationPackageListControl.UnzipPackage(package);

            // Select folder to apply the images after extraction
            this.FolderSelected(package);
        }

        #endregion
    }
}
