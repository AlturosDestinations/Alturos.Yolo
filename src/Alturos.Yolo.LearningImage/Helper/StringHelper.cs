using System.Linq;

namespace Alturos.Yolo.LearningImage.Helper
{
    public static class StringExtensions
    {
        public static int GetFirstNumber(this string text)
        {
            return int.Parse(new string(
                text.SkipWhile(c => !char.IsDigit(c)).
                TakeWhile(c => char.IsDigit(c)).
                ToArray()));
        }

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
