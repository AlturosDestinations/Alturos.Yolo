using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Alturos.Yolo.TestConsole
{
    public class PerformanceResizeAnalyze
    {
        /// <summary>
        /// Compare yolo speed with optimial images in the correct size
        /// Resize before and resize in yolo
        /// </summary>
        public void Start()
        {
            var yoloWrapper = new YoloWrapper("yolov2-tiny-voc.cfg", "yolov2-tiny-voc.weights", "voc.names");
            var files = Directory.GetFiles(@".\Images");
            var imageResizer = new ImageResizer();

            var retrys = 10;

            Console.WriteLine(string.Format("|{0,20}|{1,29}|{2,43}|", "", "Resize with yolo", "Resize before yolo"));
            Console.WriteLine(string.Format("|{0,20}|{1,15}|{2,13}|{3,15}|{4,13}|{5,13}|{6,10}|{7,10}|", "Image", "detected items", "elapsed (ms)", " detected items", "resize (ms)", "yolo (ms)", "diff (ms)", "faster"));

            foreach (var file in files)
            {
                for (var i = 0; i < retrys; i++)
                {
                    var fileInfo = new FileInfo(file);
                    var imageData = File.ReadAllBytes(file);

                    var result1 = this.ProcessResizeAfter(yoloWrapper, imageData);
                    var result2 = this.ProcessResizeBefore(yoloWrapper, imageResizer, imageData);
                    var diff = result1.Item3 - result2.Item4;

                    Console.WriteLine(string.Format("|{0,20}|{1,15}|{2,13}|{3,15}|{4,13}|{5,13}|{6,10}|{7,10}|", fileInfo.Name, result1.Item1.Count, result1.Item2, result2.Item1.Count, result2.Item2, result2.Item3, diff.ToString("0.00"), diff > 0));
                }
            }

            yoloWrapper.Dispose();
        }

        private Tuple<List<YoloItem>, string, double> ProcessResizeAfter(YoloWrapper yoloWrapper, byte[] imageData)
        {
            var sw = new Stopwatch();
            sw.Start();
            var items = yoloWrapper.Detect(imageData).ToList();
            sw.Stop();

            return new Tuple<List<YoloItem>, string, double>(items, $"{sw.Elapsed.TotalMilliseconds:0.00}", sw.Elapsed.TotalMilliseconds);
        }

        private Tuple<List<YoloItem>, string, string, double> ProcessResizeBefore(YoloWrapper yoloWrapper, ImageResizer imageResize, byte[] imageData)
        {
            var sw = new Stopwatch();
            sw.Start();
            imageData = imageResize.Resize(imageData, 416, 416);
            sw.Stop();
            var resizeElapsed = sw.Elapsed.TotalMilliseconds;
            sw.Restart();
            var items = yoloWrapper.Detect(imageData).ToList();
            sw.Stop();

            return new Tuple<List<YoloItem>, string, string, double>(items, $"{resizeElapsed:0.00}", $"{sw.Elapsed.TotalMilliseconds:0.00}", sw.Elapsed.TotalMilliseconds + resizeElapsed);
        }
    }
}
