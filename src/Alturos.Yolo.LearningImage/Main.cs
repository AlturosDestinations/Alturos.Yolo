using Alturos.Yolo.LearningImage.Contract;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class Main : Form
    {
        private readonly IBoundingBoxReader _boundingBoxReader;

        public Main()
        {
            this.InitializeComponent();

            this.annotationFolderList.FolderSelected += this.FolderSelected;
            this.annotationImageList.ImageSelected += this.ImageSelected;

            this._boundingBoxReader = new YoloReader();
        }

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

        public Image DrawBoxes(string imagePath)
        {
            var dataPath = this._boundingBoxReader.GetDataPath(imagePath);
            if (!File.Exists(dataPath))
            {
                return null;
            }

            var colorCodes = this.GetColorCodes();

            var items = this._boundingBoxReader.GetBoxes(dataPath);

            var image = new Bitmap(imagePath);
            using (var canvas = Graphics.FromImage(image))
            {
                foreach (var item in items)
                {
                    var width = item.Width * image.Width;
                    var heigth = item.Height * image.Height;
                    var x = (item.CenterX * image.Width) - (width / 2);
                    var y = (item.CenterY * image.Height) - (heigth / 2);

                    var color = ColorTranslator.FromHtml(colorCodes[item.ObjectIndex]);
                    var pen = new Pen(color, 3);

                    canvas.DrawRectangle(pen, x, y, width, heigth);
                }

                canvas.Flush();
            }

            return image;
        }

        private string[] GetColorCodes()
        {
            return new string[] { "#E3330E", "#48E10F", "#D40FE1", "#24ECE3", "#EC2470" };
        }

        private void Export(AnnotationImage[] images)
        {
            var exportDialog = new ExportDialog(this._boundingBoxReader);
            exportDialog.CreateImages(images.ToList());

            int boxCount = 0;
            foreach (var image in images)
            {
                var boxes = this._boundingBoxReader.GetBoxes(this._boundingBoxReader.GetDataPath(image.FilePath));
                foreach (var box in boxes)
                {
                    boxCount = Math.Max(boxCount, box.ObjectIndex + 1);
                }
            }
            exportDialog.CreateObjectClasses(boxCount);

            exportDialog.Show();
        }

        #region Delegate Callbacks

        private void FolderSelected(AnnotationFolder folder)
        {
            this.annotationImageList.SetImages(folder.Images);
        }

        private void ImageSelected(AnnotationImage image)
        {
            var oldImage = this.pictureBox1.Image;

            this.pictureBox1.Image = this.DrawBoxes(image.FilePath);

            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        #endregion
    }
}
