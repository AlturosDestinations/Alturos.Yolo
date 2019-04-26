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
            this.annotationPackageList.FolderSelected += this.FolderSelected;
        }

        private void annotationImageList_Load(object sender, EventArgs e)
        {
            this.annotationImageList.ImageSelected += this.ImageSelected;
            this.annotationImageList.ExtractionRequested += this.ExtractionRequested;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.annotationPackageList.FolderSelected -= this.FolderSelected;
            this.annotationImageList.ImageSelected -= this.ImageSelected;
            this.annotationImageList.ExtractionRequested -= this.ExtractionRequested;
        }

        #endregion

        #region Load and Export

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
            this.annotationPackageList.Initialize(this._boundingBoxReader, annotationPackageProvider);
            this.annotationPackageList.LoadPackages();
        }

        private void allImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = this.annotationPackageList.GetAll();
            this.Export(items);
        }

        private void selectedImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = this.annotationPackageList.GetSelected();
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
            if (package.Extracted)
            {
                if (package.Images == null)
                {
                    this.annotationPackageList.OpenPackage(package);
                }

                this.annotationImageList.SetImages(package.Images);
            }
            else
            {
                this.annotationImageList.SetImages(null);
                this.annotationImageList.ShowExtractionWarning(package);
            }
        }

        private void ImageSelected(AnnotationImage image)
        {
            this.annotationImageControl.SetImage(image);
        }

        private void ExtractionRequested(AnnotationPackage package)
        {
            this.annotationPackageList.UnzipPackage(package);

            // Select folder to apply the images after extraction
            this.FolderSelected(package);
        }

        #endregion
    }
}
