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

        private void annotationFolderList_Load(object sender, EventArgs e)
        {
            this.annotationFolderList.Initialize(this._boundingBoxReader);
            this.annotationFolderList.FolderSelected += this.FolderSelected;
        }

        private void annotationImageList_Load(object sender, EventArgs e)
        {
            this.annotationImageList.ImageSelected += this.ImageSelected;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.annotationFolderList.FolderSelected -= FolderSelected;
            this.annotationImageList.ImageSelected -= ImageSelected;
        }

        #endregion

        #region Load and Export

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.annotationFolderList.LoadFolders();
        }

        private void allImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = this.annotationFolderList.GetAll();
            this.Export(items);
        }

        private void selectedImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = this.annotationFolderList.GetSelected();
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

        private void FolderSelected(AnnotationFolder folder)
        {
            this.annotationImageList.SetImages(folder.Images);
        }

        private void ImageSelected(AnnotationImage image)
        {
            this.annotationImageControl.SetImage(image);
        }

        #endregion
    }
}
