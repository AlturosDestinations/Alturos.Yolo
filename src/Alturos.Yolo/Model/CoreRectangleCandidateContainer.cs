using System.Runtime.InteropServices;

namespace Alturos.Yolo.Model
{
    [StructLayout(LayoutKind.Sequential, Size = 10)]
    struct CoreRectangleCandidateContainer
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public CoreRectangleCandidate[] candidates;
    }
}
