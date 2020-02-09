using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Alturos.Yolo
{
    public class YoloTracking
    {
        //private Point _trackingObject;
        private int _maxDistance;
        private int _index;
        private readonly Dictionary<string, YoloTrackingItem> _lastItems;

        public YoloTracking(int maxDistance = 1000)
        {
            this._maxDistance = maxDistance;

            this._lastItems = new Dictionary<string, YoloTrackingItem>();
        }

        public void Reset()
        {
            this._index = 0;
            this._lastItems.Clear();
        }

        public IEnumerable<YoloTrackingItem> Analyse(IEnumerable<YoloItem> items)
        {
            this._index++;

            if (this._lastItems.Count == 0)
            {
                foreach (var item in items)
                {
                    var trackingItem = new YoloTrackingItem(item, this.GetObjectId());
                    this._lastItems.Add(trackingItem.ObjectId, trackingItem);
                }

                return this._lastItems.Values;
            }

            var trackingItems = new List<YoloTrackingItem>();

            foreach (var item in items)
            {
                var bestMatch = this._lastItems.Values.Select(o => new
                {
                    Item = o,
                    Distance = this.Distance(o.Center(), item.Center()),
                    SizeDifference = this.GetSizeDifferencePercentage(o, item)
                })
                    .Where(o => !trackingItems.Select(x => x.ObjectId).Contains(o.Item.ObjectId) && o.Distance <= this._maxDistance && o.SizeDifference < 30)
                    .OrderBy(o => o.Distance)
                    .FirstOrDefault();

                if (bestMatch == null || bestMatch.Item.ProcessIndex + 5 < this._index)
                {
                    var trackingItem1 = new YoloTrackingItem(item, this.GetObjectId());
                    trackingItem1.ProcessIndex = this._index;
                    trackingItems.Add(trackingItem1);
                    this._lastItems.Add(trackingItem1.ObjectId, trackingItem1);
                    continue;
                }

                bestMatch.Item.X = item.X;
                bestMatch.Item.Y = item.Y;
                bestMatch.Item.Width = item.Width;
                bestMatch.Item.Height = item.Height;
                bestMatch.Item.ProcessIndex = this._index;

                var trackingItem = new YoloTrackingItem(item, bestMatch.Item.ObjectId);
                trackingItems.Add(trackingItem);
            }

            return trackingItems;
        }

        private string GetObjectId()
        {
            return $"O{this._lastItems.Count:00000}";
        }

        private double GetSizeDifferencePercentage(YoloTrackingItem item1, YoloItem item2)
        {
            var area1 = item1.Width * item1.Height;
            var area2 = item2.Width * item2.Height;

            if (area1 == area2)
            {
                return 0;
            }

            if (area1 > area2)
            {
                var change1 = 100.0 * area2 / area1;
                return 100 - change1;
            }

            var change = 100.0 * area1 / area2;
            return 100 - change;
        }

        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(this.Pow2(p2.X - p1.X) + Pow2(p2.Y - p1.Y));
        }

        private double Pow2(double x)
        {
            return x * x;
        }
    }
}
