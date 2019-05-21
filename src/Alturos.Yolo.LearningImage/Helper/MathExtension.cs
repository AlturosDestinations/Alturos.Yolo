namespace Alturos.Yolo.LearningImage.Helper
{
    public static class MathExtension
    {
        public static double Clamp(this double val, double min, double max)
        {
            if (val < min) { return min; }
            if (val > max) { return max; }
            return val;
        }
    }
}
