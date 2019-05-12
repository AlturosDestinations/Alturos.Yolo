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
        private DragPoint _dragPoint;
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

        private CanvasInfo GetCanvasInformation()
        {
            var imageWidth = this.pictureBox1.Image.Width;
            var imageHeight = this.pictureBox1.Image.Height;
            var canvasWidth = this.pictureBox1.Width;
            var canvasHeight = this.pictureBox1.Height;

            var imageRatio = imageWidth / (float)imageHeight; // image W:H ratio
            var containerRatio = canvasWidth / (float)canvasHeight; // container W:H ratio

            var canvasInfo = new CanvasInfo();
            if (imageRatio >= containerRatio)
            {
                //Horizontal image
                var scaleFactor = canvasWidth / (float)imageWidth;
                canvasInfo.ScaledHeight = imageHeight * scaleFactor;
                canvasInfo.ScaledWidth = imageWidth * scaleFactor;
                canvasInfo.OffsetY = (canvasHeight - canvasInfo.ScaledHeight) / 2;
            }
            else
            {
                //Vertical image
                var scaleFactor = canvasHeight / (float)imageHeight;
                canvasInfo.ScaledHeight = imageHeight * scaleFactor;
                canvasInfo.ScaledWidth = imageWidth * scaleFactor;
                canvasInfo.OffsetX = (canvasWidth - canvasInfo.ScaledWidth) / 2;
            }

            return canvasInfo;
        }

        private float CalculateDistanceBetweenPoints(PointF pt1, Point pt2)
        {
            float dx = pt1.X - pt2.X;
            float dy = pt1.Y - pt2.Y;
            return dx * dx + dy * dy;
        }

        private DragPoint[] GetDragPoints(Rectangle rectangle, int drawOffset)
        {
            var p1 = new Point(rectangle.X - drawOffset, rectangle.Y - drawOffset);
            var p2 = new Point(p1.X + rectangle.Width, p1.Y);
            var p3 = new Point(p1.X, p1.Y + rectangle.Height);
            var p4 = new Point(p1.X + rectangle.Width, p1.Y + rectangle.Height);

            return new DragPoint[]
            {
                new DragPoint(p1, DragPointPosition.TopLeft),
                new DragPoint(p2, DragPointPosition.TopRight),
                new DragPoint(p3, DragPointPosition.BottomLeft),
                new DragPoint(p4, DragPointPosition.BottomRight)
            };
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (this._annotationImage?.BoundingBoxes == null)
            {
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.High;

            var drawOffset = this._mouseDragElementSize / 2;
            var canvasInfo = this.GetCanvasInformation();

            Cursor.Current = Cursors.Default;

            var boundingBoxes = this._annotationImage?.BoundingBoxes;
            foreach (var boundingBox in boundingBoxes)
            {
                var width = boundingBox.Width * canvasInfo.ScaledWidth;
                var height = (boundingBox.Height * canvasInfo.ScaledHeight);
                var x = (boundingBox.CenterX * canvasInfo.ScaledWidth) - (width / 2) + canvasInfo.OffsetX;
                var y = (boundingBox.CenterY * canvasInfo.ScaledHeight) - (height / 2) + canvasInfo.OffsetY;

                var rectangle = new Rectangle((int)x, (int)y, (int)width, (int)height);

                var brush = Brushes.Yellow;

                e.Graphics.DrawRectangle(new Pen(brush, 2), rectangle);

                var biggerRectangle = Rectangle.Inflate(rectangle, 20, 20);
                if (biggerRectangle.Contains(this._mousePosition))
                {
                    brush = Brushes.OrangeRed;

                    var dragPoints = this.GetDragPoints(rectangle, drawOffset);
                    foreach (var dragPoint in dragPoints)
                    {
                        var dragElementBrush = Brushes.LightPink;
                        if (this.CalculateDistanceBetweenPoints(this._mousePosition, new Point(dragPoint.Point.X, dragPoint.Point.Y)) < 200)
                        {
                            Cursor.Current = Cursors.Hand;
                            dragElementBrush = Brushes.Yellow;
                        }

                        if (dragPoint.Position == DragPointPosition.BottomRight)
                        {
                            var points = new Point[]
                            {
                                new Point(dragPoint.Point.X, dragPoint.Point.Y),
                                new Point(dragPoint.Point.X + 15 , dragPoint.Point.Y),
                                new Point(dragPoint.Point.X, dragPoint.Point.Y + 15),
                            };

                            e.Graphics.FillPolygon(dragElementBrush, points);

                            continue;
                        }

                        e.Graphics.FillEllipse(dragElementBrush, dragPoint.Point.X, dragPoint.Point.Y, this._mouseDragElementSize, this._mouseDragElementSize);
                    }
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._annotationImage?.BoundingBoxes == null)
            {
                return;
            }

            var drawOffset = this._mouseDragElementSize / 2;
            var canvasInfo = this.GetCanvasInformation();

            var boundingBoxes = this._annotationImage?.BoundingBoxes;
            foreach (var boundingBox in boundingBoxes)
            {
                var width = boundingBox.Width * canvasInfo.ScaledWidth;
                var height = (boundingBox.Height * canvasInfo.ScaledHeight);
                var x = (boundingBox.CenterX * canvasInfo.ScaledWidth) - (width / 2) + canvasInfo.OffsetX;
                var y = (boundingBox.CenterY * canvasInfo.ScaledHeight) - (height / 2) + canvasInfo.OffsetY;

                var rectangle = new Rectangle((int)x, (int)y, (int)width, (int)height);

                var startDrag = false;

                var dragPoints = this.GetDragPoints(rectangle, drawOffset);
                foreach (var dragPoint in dragPoints)
                {
                    if (this.CalculateDistanceBetweenPoints(this._mousePosition, new Point(dragPoint.Point.X, dragPoint.Point.Y)) < 200)
                    {
                        this._dragPoint = dragPoint;
                        startDrag = true;
                        break;
                    }
                }

                if (startDrag)
                {
                    this._selectedItem = boundingBox;
                    break;
                }
                else
                {
                    this._dragPoint = null;
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

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._selectedItem != null)
            {
                var canvasInfo = this.GetCanvasInformation();
                var centerX = 0.0;
                var centerY = 0.0;

                var x = (e.X - canvasInfo.OffsetX) / canvasInfo.ScaledWidth;
                var y = (e.Y - canvasInfo.OffsetY) / canvasInfo.ScaledHeight;

                if (this._dragPoint.Position == DragPointPosition.TopLeft)
                {
                    centerX = x + (this._selectedItem.Width / 2);
                    centerY = y + (this._selectedItem.Height / 2);
                }

                if (this._dragPoint.Position == DragPointPosition.TopRight)
                {
                    centerX = x - (this._selectedItem.Width / 2);
                    centerY = y + (this._selectedItem.Height / 2);
                }

                if (this._dragPoint.Position == DragPointPosition.BottomRight)
                {
                    var topLeftX = this._selectedItem.CenterX - (this._selectedItem.Width / 2);
                    var topLeftY = this._selectedItem.CenterY - (this._selectedItem.Height / 2);

                    var width = x - topLeftX;
                    var height = y - topLeftY;

                    centerX = topLeftX + (width / 2);
                    centerY = topLeftY + (height / 2);

                    this._selectedItem.Width = (float)width;
                    this._selectedItem.Height = (float)height;
                }

                if (this._dragPoint.Position == DragPointPosition.BottomLeft)
                {
                    return;
                }

                this._selectedItem.CenterX = (float)centerX;
                this._selectedItem.CenterY = (float)centerY;
            }

            this._mousePosition = e.Location;
            this.pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var canvasInfo = this.GetCanvasInformation();
            var width = 0.04;
            var height = 0.06;

            var x = (e.X - canvasInfo.OffsetX + (width * canvasInfo.ScaledWidth / 2)) / canvasInfo.ScaledWidth;
            var y = (e.Y - canvasInfo.OffsetY + (height * canvasInfo.ScaledHeight / 2)) / canvasInfo.ScaledHeight;

            this._annotationImage.BoundingBoxes.Add(new AnnotationBoundingBox
            {
                CenterX = (float)x,
                CenterY = (float)y,
                Height = (float)height,
                Width = (float)width
            });
        }
    }
}
