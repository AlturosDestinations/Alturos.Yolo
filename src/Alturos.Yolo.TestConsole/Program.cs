using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Alturos.Yolo.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start yolo detection");

            TestTracking();
            //CheckPerformance();

            Console.WriteLine("Done, press enter for quit");
            Console.ReadLine();
        }

        static void CheckPerformance()
        {
            var yoloWrapper = new YoloWrapper("yolov2-tiny-voc.cfg", "yolov2-tiny-voc.weights", "voc.names");
            var files = Directory.GetFiles(@".\Images");
            var imageResizer = new ImageResizer();

            var retrys = 3;

            Console.WriteLine(string.Format("|{0,40}|{1,5}|{2,25}|{3,5}|{4,25}|{5,10}|{6,10}|", "Image", "items", "elapsed (ms)", "items", "elapsed (ms)", "diff (ms)", "faster"));

            foreach (var file in files)
            {
                for (var i = 0; i < retrys; i++)
                {
                    var fileInfo = new FileInfo(file);
                    var imageData = File.ReadAllBytes(file);

                    var result1 = ProcessResizeAfter(yoloWrapper, imageData);
                    var result2 = ProcessResizeBefore(yoloWrapper, imageResizer, imageData);
                    var diff = result1.Item3 - result2.Item3;

                    Console.WriteLine(string.Format("|{0,40}|{1,5}|{2,25}|{3,5}|{4,25}|{5,10}|{6,10}|", fileInfo.Name, result1.Item1.Count, result1.Item2, result2.Item1.Count, result2.Item2, diff.ToString("0.00"), diff > 0));
                }
            }
        }

        static Tuple<List<YoloItem>, string, double> ProcessResizeAfter(YoloWrapper yoloWrapper, byte[] imageData)
        {
            var sw = new Stopwatch();
            sw.Start();
            var items = yoloWrapper.Detect(imageData).ToList();
            sw.Stop();

            return new Tuple<List<YoloItem>, string, double>(items, $"yolo & resize:{sw.Elapsed.TotalMilliseconds:0.00}", sw.Elapsed.TotalMilliseconds);
        }

        static Tuple<List<YoloItem>, string, double> ProcessResizeBefore(YoloWrapper yoloWrapper, ImageResizer imageResize, byte[] imageData)
        {
            var sw = new Stopwatch();
            sw.Start();
            imageData = imageResize.Resize(imageData, 416, 416);
            sw.Stop();
            var resizeElapsed = sw.Elapsed.TotalMilliseconds;
            sw.Restart();
            var items = yoloWrapper.Detect(imageData).ToList();
            sw.Stop();

            return new Tuple<List<YoloItem>, string, double>(items, $"resize:{resizeElapsed:0.00} yolo:{sw.Elapsed.TotalMilliseconds:0.00}", sw.Elapsed.TotalMilliseconds + resizeElapsed);
        }

        static void TestLogic2()
        {
            var configurationDetector = new ConfigurationDetector();
            var config = configurationDetector.Detect();

            using (var yoloWrapper = new YoloWrapper(config))
            {
                var result = yoloWrapper.Detect(@"image.jpg");
            }
        }

        static void TestTracking()
        {
            Directory.CreateDirectory("trackingImages");

            var configurationDetector = new ConfigurationDetector();
            var config = configurationDetector.Detect();
            using (var yoloWrapper = new YoloWrapper(config))
            {
                var yoloTracking = new YoloTracking(yoloWrapper, 200);

                yoloTracking.SetTrackingObject(new Point(216, 343));
                var files = Directory.GetFiles(@"test");

                //yoloTracking.SetTrackingObject(new Point(385, 480));
                //var files = Directory.GetFiles(@"test3");

                foreach (var file in files)
                {
                    var imageData = File.ReadAllBytes(file);
                    var trackingItem = yoloTracking.Analyse(imageData);
                    if (trackingItem == null)
                    {
                        continue;
                    }

                    var fileInfo = new FileInfo(file);
                    File.WriteAllBytes($@"trackingImages\{trackingItem.Index}.bmp", trackingItem.TaggedImageData);
                }
            }

            Console.ReadLine();
        }
    }
}
