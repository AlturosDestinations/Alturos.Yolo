namespace Alturos.Yolo.Model
{
    public class YoloTrackingItem : YoloItem
    {
        public YoloTrackingItem(YoloItem yoloItem, int index, byte[] imageData)
        {
            this.X = yoloItem.X;
            this.Y = yoloItem.Y;
            this.Width = yoloItem.Width;
            this.Height = yoloItem.Height;
            this.Type = yoloItem.Type;
            this.Confidence = yoloItem.Confidence;

            this.Index = index;
            this.TaggedImageData = imageData;
        }

        public int Index { get; set; }
        public byte[] TaggedImageData { get; set; }
    }
}
