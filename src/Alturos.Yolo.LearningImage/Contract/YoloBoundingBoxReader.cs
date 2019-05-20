using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Alturos.Yolo.LearningImage.Contract
{
    public class YoloBoundingBoxReader : IBoundingBoxReader
    {
        public AnnotationBoundingBox[] GetBoxes(string dataPath)
        {
            if (!File.Exists(dataPath))
            {
                return null;
            }

            var data = File.ReadAllText(dataPath);
            var lines = data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var items = new List<AnnotationBoundingBox>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                if (parts.Length != 5)
                {
                    continue;
                }

                var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                int.TryParse(parts[0], out int index);
                float.TryParse(parts[1], NumberStyles.Any, ci, out var x);
                float.TryParse(parts[2], NumberStyles.Any, ci, out var y);
                float.TryParse(parts[3], NumberStyles.Any, ci, out var width);
                float.TryParse(parts[4], NumberStyles.Any, ci, out var heigth);

                items.Add(new AnnotationBoundingBox { ObjectIndex = index, CenterX = x, CenterY = y, Width = width, Height = heigth });
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
