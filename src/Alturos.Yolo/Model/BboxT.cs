namespace Alturos.Yolo.Model
{
    internal struct BboxT
    {
        internal uint x, y, w, h;         // (x,y) - top-left corner, (w, h) - width & height of bounded box
        internal float prob;              // confidence - probability that the object was found correctly
        internal uint obj_id;             // class of object - from range [0, classes-1]
        internal uint track_id;           // tracking id for video (0 - untracked, 1 - inf - tracked object)
        internal uint frames_counter;
        internal float x_3d, y_3d, z_3d;  // 3-D coordinates, if there is used 3D-stereo camera
    };
}
