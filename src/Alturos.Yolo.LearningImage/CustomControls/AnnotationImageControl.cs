using Alturos.Yolo.LearningImage.Helper;
using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationImageControl : UserControl
    {
        private Point _mousePosition = new Point(0, 0);
        private List<Rectangle> _rectangles = new List<Rectangle>();
        private int _mouseDragElementSize = 10;
        private AnnotationBoundingBox _selectedItem;
        private AnnotationImage _annotationImage;

        public AnnotationImageControl()
        {
            this.InitializeComponent();
            this._rectangles.Add(new Rectangle(100, 100, 100, 100));
        }

        public void SetImage(AnnotationImage image, List<ObjectClass> objectClasses)
        {
            this._annotationImage = image;

            var oldImage = this.pictureBox1.Image;

            if (image == null)
            {
                this.pictureBox1.Image = null;
            }
            else
            {
                this.pictureBox1.Image = DrawHelper.DrawBoxes(image, objectClasses);
            }

            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (this._annotationImage == null)
            {
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.High;

            var drawOffset = this._mouseDragElementSize / 2;

            var boundingBoxes = this._annotationImage.BoundingBoxes;
            if (boundingBoxes == null)
            {
                return;
            }

            //var scalingHorizontal = (double)this.pictureBox1.ClientSize.Width / this.pictureBox1.Image.Width;

            foreach (var boundingBox in boundingBoxes)
            {
                var width = boundingBox.Width * this.pictureBox1.ClientSize.Width;// * scalingHorizontal;
                var height = boundingBox.Height * this.pictureBox1.ClientSize.Height;
                var x = (boundingBox.CenterX * this.pictureBox1.ClientSize.Width) - (width / 2);
                var y = (boundingBox.CenterY * this.pictureBox1.ClientSize.Height) - (height / 2);

                var rectangle = new Rectangle((int)x, (int)y, (int)width, (int)height);

                var brush = Brushes.Yellow;

                var biggerRectangle = Rectangle.Inflate(rectangle, 20, 20);
                if (biggerRectangle.Contains(this._mousePosition))
                {
                    brush = Brushes.OrangeRed;

                    var points = this.GetRectangleCorners(rectangle, drawOffset);
                    foreach (var point in points)
                    {
                        var dragElementBrush = Brushes.LightPink;
                        if (this.CalculateDistanceBetweenPoints(this._mousePosition, new Point(point.X, point.Y)) < 100)
                        {
                            dragElementBrush = Brushes.Blue;
                        }

                        e.Graphics.FillEllipse(dragElementBrush, point.X, point.Y, this._mouseDragElementSize, this._mouseDragElementSize);
                    }
                }

                e.Graphics.DrawRectangle(new Pen(brush, 2), rectangle);
            }
        }

        private Point[] GetRectangleCorners(Rectangle rectangle, int drawOffset)
        {
            var p1 = new Point(rectangle.X - drawOffset, rectangle.Y - drawOffset);
            var p2 = new Point(p1.X + rectangle.Width, p1.Y);
            var p3 = new Point(p1.X + rectangle.Width, p1.Y + rectangle.Height);
            var p4 = new Point(p1.X, p1.Y + rectangle.Height);

            return new Point[] { p1, p2, p3, p4 };
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._annotationImage == null)
            {
                return;
            }

            var boundingBoxes = this._annotationImage.BoundingBoxes;
            if (boundingBoxes == null)
            {
                return;
            }

            var drawOffset = this._mouseDragElementSize / 2;
            foreach (var boundingBox in boundingBoxes)
            {
                var width = boundingBox.Width * this.pictureBox1.ClientSize.Width;// * scalingHorizontal;
                var height = boundingBox.Height * this.pictureBox1.ClientSize.Height;
                var x = (boundingBox.CenterX * this.pictureBox1.ClientSize.Width) - (width / 2);
                var y = (boundingBox.CenterY * this.pictureBox1.ClientSize.Height) - (height / 2);

                var rectangle = new Rectangle((int)x, (int)y, (int)width, (int)height);

                var startDrag = false;

                var points = this.GetRectangleCorners(rectangle, drawOffset);
                foreach (var point in points)
                {
                    if (this.CalculateDistanceBetweenPoints(this._mousePosition, new Point(point.X, point.Y)) < 100)
                    {
                        startDrag = true;
                    }
                }

                if (startDrag)
                {
                    this._selectedItem = boundingBox;
                }
                else
                {
                    this._selectedItem = null;
                }
            }

            this._mousePosition = e.Location;
            this.pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this._selectedItem = null;

            this._mousePosition = new Point(0,0);
            this.pictureBox1.Invalidate();
        }

        private float CalculateDistanceBetweenPoints(PointF pt1, Point pt2)
        {
            float dx = pt1.X - pt2.X;
            float dy = pt1.Y - pt2.Y;
            return dx * dx + dy * dy;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._selectedItem != null)
            {
                var x = (float)e.X / this.pictureBox1.ClientSize.Width;
                var centerX = x + (this._selectedItem.Width / 2);

                var y = (float)e.Y / this.pictureBox1.ClientSize.Height;
                var centerY = y + (this._selectedItem.Height / 2);

                this._selectedItem.CenterX = centerX;
                this._selectedItem.CenterY = centerY;
            }

            this._mousePosition = e.Location;
            this.pictureBox1.Invalidate();
        }
    }
}
