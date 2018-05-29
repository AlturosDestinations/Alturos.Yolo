using System.Runtime.InteropServices;

namespace Alturos.Yolo.Model
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CoreRectangleCandidate
    {
        public CoreRectangle rectangle;
        public double confidence;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string objectType;

        //public int objectType;
    }
}
