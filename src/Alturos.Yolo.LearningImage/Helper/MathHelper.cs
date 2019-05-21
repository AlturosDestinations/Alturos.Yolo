namespace Alturos.Yolo.LearningImage.Helper
{
    public static class MathHelper
    {
        public static double Clamp(this double val, double min, double max)
        {
            if (val < min) { return min; }
            if (val > max) { return max; }
            return val;
        }

        public static double Clamp01(this double val)
        {
            return val.Clamp(0, 1);
        }
    }
}
