using System.Runtime.InteropServices;

namespace Alturos.Yolo.Model
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CoreRectangle
    {
        public Point2D topLeftCorner { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
