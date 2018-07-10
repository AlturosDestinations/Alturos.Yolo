using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Alturos.Yolo.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start yolo detection");

            Console.WriteLine("Done, press enter for quit");
            Console.ReadLine();
        }

        static void TestLogic1()
        {
            var yoloWrapper = new YoloWrapper("yolov2-tiny-voc.cfg", "yolov2-tiny-voc.weights", "voc.names");
            var files = Directory.GetFiles(@".\Images");

            var retrys = 100;
            for (var i = 0; i < retrys; i++)
            {
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var imageData = File.ReadAllBytes(file);

                    var sw = new Stopwatch();
                    sw.Start();
                    var items = yoloWrapper.Detect(imageData).ToList();
                    sw.Stop();
                    Console.WriteLine($"{fileInfo.Name} found {items.Count} results, elapsed {sw.Elapsed.TotalMilliseconds:0.00}ms");

                    if (items.Count > 0)
                    {
                        Console.WriteLine("------------------DETAILS-----------------");

                        foreach (var item in items)
                        {
                            Console.WriteLine($"Type:{item.Type} Confidence:{item.Confidence:0.00}");
                        }

                        Console.WriteLine("------------------------------------------");
                    }
                }
            }
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
    }
}
