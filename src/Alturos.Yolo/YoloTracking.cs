using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Alturos.Yolo
{
    public class YoloTracking
    {
        private YoloWrapper _yoloWrapper;
        private Point _trackingObject;
        private int _maxDistance;
        private int _index;

        public YoloTracking(YoloWrapper yoloWrapper, int maxDistance = 1000)
        {
            this._yoloWrapper = yoloWrapper;
            this._maxDistance = maxDistance;
        }

        public void SetTrackingObject(YoloItem trackingObject)
        {
            this._trackingObject = trackingObject.Center();
        }

        public void SetTrackingObject(Point trackingObject)
        {
            this._trackingObject = trackingObject;
        }

        public YoloTrackingItem Analyse(byte[] imageData)
        {
            var items = this._yoloWrapper.Detect(imageData);

            var probableObject = this.FindBestMatch(items, this._maxDistance);
            if (probableObject == null)
            {
                return null;
            }

            this._trackingObject = probableObject.Center();
            var taggedImageData = this.DrawImage(imageData, probableObject);

            return new YoloTrackingItem(probableObject, this._index++, taggedImageData);
        }

        private YoloItem FindBestMatch(IEnumerable<YoloItem> items, int maxDistance)
        {
            var distanceItems = items.Select(o => new { Distance = this.Distance(o.Center(), this._trackingObject), Item = o }).Where(o => o.Distance <= maxDistance).OrderBy(o => o.Distance);

            var bestMatch = distanceItems.FirstOrDefault();
            return bestMatch?.Item;
        }

        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(this.Pow2(p2.X - p1.X) + Pow2(p2.Y - p1.Y));
        }

        private double Pow2(double x)
        {
            return x * x;
        }

        private byte[] DrawImage(byte[] imageData, YoloItem item)
        {
            using (var memoryStream = new MemoryStream(imageData))
            using (var image = Image.FromStream(memoryStream))
            using (var canvas = Graphics.FromImage(image))
            using (var pen = new Pen(Brushes.Pink, 3))
            {
                canvas.DrawRectangle(pen, item.X, item.Y, item.Width, item.Height);
                canvas.Flush();

                using (var memoryStream2 = new MemoryStream())
                {
                    image.Save(memoryStream2, ImageFormat.Bmp);
                    return memoryStream2.ToArray();
                }
            }
        }
    }
}
