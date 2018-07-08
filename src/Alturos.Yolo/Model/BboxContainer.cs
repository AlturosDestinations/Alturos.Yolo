using System.Runtime.InteropServices;

namespace Alturos.Yolo.Model
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct BboxContainer
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YoloWrapper.MaxObjects)]
        internal BboxT[] candidates;
    }
}
