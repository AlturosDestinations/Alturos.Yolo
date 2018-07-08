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
            var retrys = 100;

            var yoloWrapper = new YoloWrapper("yolov2-tiny-voc.cfg", "yolov2-tiny-voc.weights", "voc.names");

            Console.WriteLine("Start yolo detection");

            var files = Directory.GetFiles(@".\Images");

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

            Console.WriteLine("Done, press enter for quit");
            Console.ReadLine();
        }
    }
}
