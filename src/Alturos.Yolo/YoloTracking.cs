using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Alturos.Yolo
{
    public class YoloTracking
    {
        private readonly int _frameWidth;
        private readonly int _frameHeight;
        private readonly Dictionary<string, YoloTrackingItemExtended> _trackingItems;

        private int _processIndex;
        private int _nextObjectId;

        public YoloTracking(int frameWidth, int frameHeight)
        {
            this._frameWidth = frameWidth;
            this._frameHeight = frameHeight;
            this._trackingItems = new Dictionary<string, YoloTrackingItemExtended>();
        }

        public void Reset()
        {
            this._processIndex = 0;
            this._trackingItems.Clear();
        }

        public IEnumerable<YoloTrackingItem> Analyse(IEnumerable<YoloItem> items)
        {
            this._processIndex++;

            if (this._trackingItems.Count == 0)
            {
                foreach (var item in items)
                {
                    var trackingItem = new YoloTrackingItemExtended(item, this.GetObjectId());
                    this._trackingItems.Add(trackingItem.ObjectId, trackingItem);
                }

                return new YoloTrackingItem[0];
            }

            var trackingItems = new List<YoloTrackingItem>();

            foreach (var item in items)
            {
                var bestMatch = this._trackingItems.Values.Select(o => new
                {
                    Item = o,
                    DistancePercentage = this.DistancePercentage(o.Center(), item.Center()),
                    SizeDifference = this.GetSizeDifferencePercentage(o, item)
                })
                .Where(o => !trackingItems.Select(x => x.ObjectId).Contains(o.Item.ObjectId) && o.DistancePercentage <= 15 && o.SizeDifference < 30)
                .OrderBy(o => o.DistancePercentage)
                .FirstOrDefault();

                if (bestMatch == null || bestMatch.Item.ProcessIndex + 25 < this._processIndex)
                {
                    var trackingItem1 = new YoloTrackingItemExtended(item, this.GetObjectId())
                    {
                        ProcessIndex = this._processIndex
                    };

                    this._trackingItems.Add(trackingItem1.ObjectId, trackingItem1);
                    continue;
                }

                bestMatch.Item.X = item.X;
                bestMatch.Item.Y = item.Y;
                bestMatch.Item.Width = item.Width;
                bestMatch.Item.Height = item.Height;
                bestMatch.Item.ProcessIndex = this._processIndex;
                bestMatch.Item.IncreaseTrackingConfidence();

                if (bestMatch.Item.TrackingConfidence >= 60)
                {
                    var trackingItem = new YoloTrackingItem(item, bestMatch.Item.ObjectId);
                    trackingItems.Add(trackingItem);
                }
            }

            var itemsWithoutHits = this._trackingItems.Values.Where(o => o.ProcessIndex != this._processIndex);
            foreach (var item in itemsWithoutHits)
            {
                item.DecreaseTrackingConfidence();
            }

            return trackingItems;
        }

        private string GetObjectId()
        {
            this._nextObjectId++;
            return $"O{this._nextObjectId:00000}";
        }

        private double GetSizeDifferencePercentage(YoloTrackingItemExtended item1, YoloItem item2)
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

        private double DistancePercentage(Point p1, Point p2)
        {
            var max = this.Distance(new Point(0, 0), new Point(this._frameWidth, this._frameHeight));
            var current = this.Distance(p1, p2);

            return 100.0 * current / max;
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
