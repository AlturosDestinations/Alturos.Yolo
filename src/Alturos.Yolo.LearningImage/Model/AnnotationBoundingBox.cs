namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationBoundingBox
    {
        public int ObjectIndex { get; set; }
        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
