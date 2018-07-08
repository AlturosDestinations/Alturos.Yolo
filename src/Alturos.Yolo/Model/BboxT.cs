using System;

namespace Alturos.Yolo.Model
{
    internal struct BboxT
    {
        internal UInt32 x, y, w, h;    // (x,y) - top-left corner, (w, h) - width & height of bounded box
        internal float prob;                 // confidence - probability that the object was found correctly
        internal UInt32 obj_id;        // class of object - from range [0, classes-1]
        internal UInt32 track_id;      // tracking id for video (0 - untracked, 1 - inf - tracked object)
        internal UInt32 frames_counter;
    };
}
