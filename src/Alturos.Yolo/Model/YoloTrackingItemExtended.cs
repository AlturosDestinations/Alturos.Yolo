using System;

namespace Alturos.Yolo.Model
{
    internal class YoloTrackingItemExtended : YoloItem
    {
        public string ObjectId { get; private set; }
        public int ProcessIndex { get; set; }
        public double TrackingConfidence { get; private set; }

        public YoloTrackingItemExtended(YoloItem item, string objectId)
        {
            this.ObjectId = objectId;

            this.Type = item.Type;
            this.Confidence = item.Confidence;
            this.X = item.X;
            this.Y = item.Y;
            this.Width = item.Width;
            this.Height = item.Height;
        }

        public void IncreaseTrackingConfidence()
        {
            if (this.TrackingConfidence < 100)
            {
                this.TrackingConfidence += 5;
            }
        }

        public void DecreaseTrackingConfidence()
        {
            if (this.TrackingConfidence > 0)
            {
                this.TrackingConfidence -= 5;
            }
        }
    }
}
