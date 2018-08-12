using Alturos.Yolo.TestUI.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Alturos.Yolo.TestUI
{
    public class DirectoryImageReader
    {
        public IEnumerable<ImageInfo> Analyze(string path)
        {
            var allowedFileExtensions = new string[] { ".bmp", ".jpg", ".png" };

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (!allowedFileExtensions.Any(o => file.EndsWith(o, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var fileInfo = new FileInfo(file);
                var resolution = this.GetImageResolution(file);

                var imageInfo = new ImageInfo();
                imageInfo.Name = fileInfo.Name;
                imageInfo.Path = file;
                imageInfo.Width = resolution.Item1;
                imageInfo.Height = resolution.Item2;

                yield return imageInfo;
            }
        }

        private Tuple<int, int> GetImageResolution(string imagePath)
        {
            try
            {
                using (var image = Image.FromFile(imagePath))
                {
                    return new Tuple<int, int>(image.Width, image.Height);
                }
            }
            catch (Exception)
            {
                return new Tuple<int, int>(0, 0);
            }
        }
    }
}
