using System;
using System.Drawing;
using System.IO;

namespace Alturos.Yolo.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start yolo detection");

            //TestTracking();
            new PerformanceAnalyze().Start();
            //new PerformanceResizeAnalyze().Start();

            Console.WriteLine("Done, press enter for quit");
            Console.ReadLine();
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
