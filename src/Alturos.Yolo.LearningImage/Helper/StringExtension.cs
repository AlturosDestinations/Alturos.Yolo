using System.Linq;

namespace Alturos.Yolo.LearningImage.Helper
{
    public static class StringExtension
    {
        public static int GetFirstNumber(this string text)
        {
            var tempNumber = new string(
                text.SkipWhile(c => !char.IsDigit(c)).
                TakeWhile(c => char.IsDigit(c)).
                ToArray());

            int.TryParse(tempNumber, out var number);

            return number;
        }
    }
}
