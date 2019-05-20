using Alturos.Yolo.LearningImage.Helper;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationImageControl : UserControl
    {
        public Action<AnnotationPackage> PackageEdited { get; set; }

        private Point _mousePosition = new Point(0, 0);
        private List<Rectangle> _rectangles = new List<Rectangle>();
        private int _mouseDragElementSize = 10;
        private AnnotationBoundingBox _cachedBoundingBox;
        private AnnotationBoundingBox _selectedBoundingBox;
        private DragPoint _dragPoint;
        private AnnotationImage _annotationImage;
        private List<ObjectClass> _objectClasses;
        private AnnotationPackage _package;
        private bool _autoPlace;

        public AnnotationImageControl()
        {
            this.InitializeComponent();
            this._rectangles.Add(new Rectangle(100, 100, 100, 100));

            this.pictureBox1.ContextMenuStrip = this.contextMenuStripPicture;
        }

        public void SetPackage(AnnotationPackage package)
        {
            this._package = package;
        }

        public void SetImage(AnnotationImage image, List<ObjectClass> objectClasses)
        {
            this._annotationImage = image;
            this._objectClasses = objectClasses;

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

        public void ApplyCachedBoundingBox()
        {
            if (!this._autoPlace)
            {
                return;
            }

            if (this._cachedBoundingBox != null && !this._annotationImage.BoundingBoxes.Any())
            {
                this._annotationImage.BoundingBoxes.Add(new AnnotationBoundingBox(this._cachedBoundingBox));
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

        private float PointDistance(PointF pt1, Point pt2)
        {
            float dx = pt1.X - pt2.X;
            float dy = pt1.Y - pt2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private DragPoint[] GetDragPoints(Rectangle rectangle, int drawOffset)
        {
            var p1 = new Point(rectangle.X - drawOffset, rectangle.Y - drawOffset);
            var p2 = new Point(p1.X + rectangle.Width, p1.Y);
            var p3 = new Point(p1.X, p1.Y + rectangle.Height);
            var p4 = new Point(p1.X + rectangle.Width, p1.Y + rectangle.Height);

            return new DragPoint[]
            {
                new DragPoint(p1, DragPointPosition.TopLeft, DragPointType.Default),
                new DragPoint(p2, DragPointPosition.TopRight, DragPointType.Delete),
                new DragPoint(p3, DragPointPosition.BottomLeft, DragPointType.Default),
                new DragPoint(p4, DragPointPosition.BottomRight, DragPointType.Resize)
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
            if (boundingBoxes == null)
            {
                return;
            }

            foreach (var boundingBox in boundingBoxes)
            {
                var width = boundingBox.Width * canvasInfo.ScaledWidth;
                var height = (boundingBox.Height * canvasInfo.ScaledHeight);
                var x = (boundingBox.CenterX * canvasInfo.ScaledWidth) - (width / 2) + canvasInfo.OffsetX;
                var y = (boundingBox.CenterY * canvasInfo.ScaledHeight) - (height / 2) + canvasInfo.OffsetY;

                var rectangle = new Rectangle((int)x, (int)y, (int)width, (int)height);

                var brush = DrawHelper.GetColorCode(boundingBox.ObjectIndex);

                e.Graphics.DrawRectangle(new Pen(brush, 2), rectangle);
                this.DrawLabel(e.Graphics, (float)x, (float)y, this._objectClasses.FirstOrDefault(o => o.Id == boundingBox.ObjectIndex));

                var biggerRectangle = Rectangle.Inflate(rectangle, 20, 20);
                if (biggerRectangle.Contains(this._mousePosition))
                {
                    var dragPoints = this.GetDragPoints(rectangle, drawOffset);
                    foreach (var dragPoint in dragPoints)
                    {
                        var dragElementBrush = Brushes.LightPink;
                        if (this.PointDistance(this._mousePosition, new Point(dragPoint.Point.X, dragPoint.Point.Y)) < 15)
                        {
                            Cursor.Current = Cursors.Hand;
                            dragElementBrush = Brushes.Yellow;
                        }

                        if (dragPoint.Type == DragPointType.Resize)
                        {
                            var points = new Point[]
                            {
                                new Point(dragPoint.Point.X, dragPoint.Point.Y),
                                new Point(dragPoint.Point.X + 15 , dragPoint.Point.Y),
                                new Point(dragPoint.Point.X, dragPoint.Point.Y + 15),
                            };

                            e.Graphics.FillPolygon(dragElementBrush, points);
                        }
                        else if (dragPoint.Type == DragPointType.Delete)
                        {
                            e.Graphics.FillEllipse(Brushes.Red, dragPoint.Point.X - this._mouseDragElementSize / 3, dragPoint.Point.Y - this._mouseDragElementSize / 3, this._mouseDragElementSize * 1.5f, this._mouseDragElementSize * 1.5f);

                            using (var pen = new Pen(Brushes.White, 4)) {
                                var centerX = dragPoint.Point.X + this._mouseDragElementSize / 2;
                                var centerY = dragPoint.Point.Y + this._mouseDragElementSize / 2;
                                e.Graphics.DrawLine(pen, new Point(centerX - 4, centerY - 4), new Point(centerX + 4, centerY + 4));
                                e.Graphics.DrawLine(pen, new Point(centerX + 4, centerY - 4), new Point(centerX - 4, centerY + 4));
                            }
                        }
                        else
                        {
                            e.Graphics.FillEllipse(dragElementBrush, dragPoint.Point.X, dragPoint.Point.Y, this._mouseDragElementSize, this._mouseDragElementSize);
                        }
                    }
                }
            }
        }

        private void DrawLabel(Graphics graphics, float x, float y, ObjectClass objectClass)
        {
            if (objectClass == null)
            {
                return;
            }

            using (var brush = new SolidBrush(DrawHelper.GetColorCode(objectClass.Id)))
            using (var bgBrush = new SolidBrush(Color.FromArgb(128, 255, 255, 255)))
            using (var font = new Font("Arial", 20))
            {
                var text = $"{objectClass.Id} {objectClass.Name}";
                var point = new PointF(x + 4, y + 4);
                var size = graphics.MeasureString(text, font);

                graphics.FillRectangle(bgBrush, point.X, point.Y, size.Width, size.Height);
                graphics.DrawString(text, font, brush, point);
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
            if (boundingBoxes == null) { return; }
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
                    if (this.PointDistance(this._mousePosition, new Point(dragPoint.Point.X, dragPoint.Point.Y)) < 15)
                    {
                        this._dragPoint = dragPoint;
                        startDrag = true;
                        break;
                    }
                }

                if (startDrag)
                {
                    this._selectedBoundingBox = boundingBox;
                    this._cachedBoundingBox = new AnnotationBoundingBox(this._selectedBoundingBox);
                    break;
                }
                else
                {
                    this._dragPoint = null;
                    this._selectedBoundingBox = null;
                }
            }

            this._mousePosition = e.Location;
            this.pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this._dragPoint?.Type == DragPointType.Delete) {
                this._annotationImage?.BoundingBoxes.Remove(this._selectedBoundingBox);

                this.PackageEdited?.Invoke(this._package);
            }

            this._selectedBoundingBox = null;

            this._mousePosition = new Point(0,0);
            this.pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._selectedBoundingBox != null)
            {
                var canvasInfo = this.GetCanvasInformation();
                var centerX = 0.0;
                var centerY = 0.0;

                var x = (e.X - canvasInfo.OffsetX) / canvasInfo.ScaledWidth;
                var y = (e.Y - canvasInfo.OffsetY) / canvasInfo.ScaledHeight;

                if (this._dragPoint.Position == DragPointPosition.TopLeft)
                {
                    centerX = x + (this._selectedBoundingBox.Width / 2);
                    centerY = y + (this._selectedBoundingBox.Height / 2);
                }

                if (this._dragPoint.Position == DragPointPosition.TopRight)
                {
                    centerX = x - (this._selectedBoundingBox.Width / 2);
                    centerY = y + (this._selectedBoundingBox.Height / 2);
                }

                if (this._dragPoint.Position == DragPointPosition.BottomLeft)
                {
                    centerX = x + (this._selectedBoundingBox.Width / 2);
                    centerY = y - (this._selectedBoundingBox.Height / 2);
                }

                centerX = centerX.Clamp(this._selectedBoundingBox.Width / 2, 1 - this._selectedBoundingBox.Width / 2);
                centerY = centerY.Clamp(this._selectedBoundingBox.Height / 2, 1 - this._selectedBoundingBox.Height / 2);

                if (this._dragPoint.Position == DragPointPosition.BottomRight)
                {
                    var topLeftX = this._selectedBoundingBox.CenterX - (this._selectedBoundingBox.Width / 2);
                    var topLeftY = this._selectedBoundingBox.CenterY - (this._selectedBoundingBox.Height / 2);

                    var width = Math.Max(0.02, x - topLeftX);
                    var height = Math.Max(0.02, y - topLeftY);

                    centerX = topLeftX + (width / 2);
                    centerY = topLeftY + (height / 2);

                    this._selectedBoundingBox.Width = (float)width;
                    this._selectedBoundingBox.Height = (float)height;
                }

                this._selectedBoundingBox.CenterX = (float)centerX;
                this._selectedBoundingBox.CenterY = (float)centerY;

                this._cachedBoundingBox.CenterX = this._selectedBoundingBox.CenterX;
                this._cachedBoundingBox.CenterY = this._selectedBoundingBox.CenterY;
                this._cachedBoundingBox.Width = this._selectedBoundingBox.Width;
                this._cachedBoundingBox.Height = this._selectedBoundingBox.Height;


                this.PackageEdited?.Invoke(this._package);
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

            if (this._annotationImage.BoundingBoxes == null)
            {
                this._annotationImage.BoundingBoxes = new List<AnnotationBoundingBox>();
            }
            this._annotationImage.BoundingBoxes.Add(new AnnotationBoundingBox
            {
                CenterX = (float)x,
                CenterY = (float)y,
                Height = (float)height,
                Width = (float)width
            });

            this.PackageEdited?.Invoke(this._package);
        }

        private void clearAnnotationsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this._annotationImage.BoundingBoxes = null;
            this.PackageEdited?.Invoke(this._package);
        }

        private void checkBoxAutoPlace_CheckedChanged(object sender, EventArgs e)
        {
            this._autoPlace = this.checkBoxAutoPlace.Checked;
        }
    }
}
