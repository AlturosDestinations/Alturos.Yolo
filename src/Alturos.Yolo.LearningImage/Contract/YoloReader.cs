using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Alturos.Yolo.LearningImage.Contract
{
    public class YoloReader : IBoundingBoxReader
    {
        public AnnotationInfo[] GetBoxes(string dataPath)
        {
            if (!File.Exists(dataPath))
            {
                return new AnnotationInfo[0];
            }

            var data = File.ReadAllText(dataPath);
            var lines = data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var items = new List<AnnotationInfo>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');

                var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                int.TryParse(parts[0], out int index);
                float.TryParse(parts[1], NumberStyles.Any, ci, out var x);
                float.TryParse(parts[2], NumberStyles.Any, ci, out var y);
                float.TryParse(parts[3], NumberStyles.Any, ci, out var width);
                float.TryParse(parts[4], NumberStyles.Any, ci, out var heigth);

                items.Add(new AnnotationInfo { ObjectIndex = index, CenterX = x, CenterY = y, Width = width, Height = heigth });
            }

            return items.ToArray();
        }

        public string GetDataPath(string imagePath)
        {
            var dataPath = Path.ChangeExtension(imagePath, "txt");
            return dataPath;
        }
    }
}
