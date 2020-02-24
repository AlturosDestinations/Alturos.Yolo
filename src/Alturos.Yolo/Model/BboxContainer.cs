using System.Runtime.InteropServices;

namespace Alturos.Yolo.Model
{
    /// <summary>
    /// C++ Communication object
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct BboxContainer
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YoloWrapper.MaxObjects)]
        internal BboxT[] candidates;
    }
}
