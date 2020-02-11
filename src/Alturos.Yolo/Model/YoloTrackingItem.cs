using System;

namespace Alturos.Yolo.Model
{
    public class YoloTrackingItem : YoloItem
    {
        public string ObjectId { get; private set; }

        public YoloTrackingItem(YoloItem item, string objectId)
        {
            this.ObjectId = objectId;

            this.Type = item.Type;
            this.Confidence = item.Confidence;
            this.X = item.X;
            this.Y = item.Y;
            this.Width = item.Width;
            this.Height = item.Height;
        }
    }
}
