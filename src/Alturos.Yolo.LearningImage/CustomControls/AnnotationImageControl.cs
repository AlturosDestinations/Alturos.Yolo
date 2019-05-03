using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationImageControl : UserControl
    {
        private List<ObjectClass> _objectClasses;

        public AnnotationImageControl()
        {
            this.InitializeComponent();
        }

        public void SetImage(AnnotationImage image, List<ObjectClass> objectClasses)
        {
            var oldImage = this.pictureBox1.Image;

            if (image == null)
            {
                this.pictureBox1.Image = null;
            }
            else
            {
                this.pictureBox1.Image = this.DrawBoxes(image);
            }

            if (oldImage != null)
            {
                oldImage.Dispose();
            }

            this._objectClasses = objectClasses;
        }

        private Image DrawBoxes(AnnotationImage image)
        {
            var colorCodes = this.GetColorCodes();

            var items = image.BoundingBoxes;

            var bitmap = new Bitmap(image.FilePath);
            using (var canvas = Graphics.FromImage(bitmap))
            {
                foreach (var item in items)
                {
                    var width = item.Width * bitmap.Width;
                    var heigth = item.Height * bitmap.Height;
                    var x = (item.CenterX * bitmap.Width) - (width / 2);
                    var y = (item.CenterY * bitmap.Height) - (heigth / 2);

                    var color = ColorTranslator.FromHtml(colorCodes[item.ObjectIndex]);
                    var pen = new Pen(color, 3);

                    var objectClass = this._objectClasses?.FirstOrDefault(o => o.Id == item.ObjectIndex);
                    if (objectClass != null)
                    {
                        using (var brush = new SolidBrush(color))
                        using (var font = new Font("Arial", 24))
                        {
                            canvas.DrawString($"{objectClass.Id} {objectClass.Name}", font, brush, new PointF(x + 4, y + 4));
                        }
                    }

                    canvas.DrawRectangle(pen, x, y, width, heigth);
                }

                canvas.Flush();
            }

            return bitmap;
        }

        private string[] GetColorCodes()
        {
            return new string[] { "#E3330E", "#48E10F", "#D40FE1", "#24ECE3", "#EC2470" };
        }
    }
}
