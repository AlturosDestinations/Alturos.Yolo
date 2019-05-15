using System.Drawing;

namespace Alturos.Yolo.LearningImage.Model
{
    public class DragPoint
    {
        public Point Point { get; private set; }
        public DragPointPosition Position { get; private set; }
        public DragPointType Type { get; private set; }

        public DragPoint(Point point, DragPointPosition position, DragPointType type)
        {
            this.Point = point;
            this.Position = position;
            this.Type = type;
        }
    }
}
