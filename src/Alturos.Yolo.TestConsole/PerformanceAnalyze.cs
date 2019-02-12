using System;
using System.Diagnostics;
using System.IO;

namespace Alturos.Yolo.TestConsole
{
    public class PerformanceAnalyze
    {
        /// <summary>
        /// Compare yolo speed with optimial images in the correct size
        /// Resize before and resize in yolo
        /// </summary>
        public void Start()
        {
            Console.WriteLine("CPU");
            Console.WriteLine("---------------------------------------------------------------");
            this.Check(true);
            Console.WriteLine("GPU");
            Console.WriteLine("---------------------------------------------------------------");
            this.Check(false);
        }

        private void Check(bool ignoreGpu)
        {
            var yoloWrapper = new YoloWrapper("yolov2-tiny-voc.cfg", "yolov2-tiny-voc.weights", "voc.names", ignoreGpu: ignoreGpu);
            var files = Directory.GetFiles(@".\Images");

            var retrys = 10;

            var sw = new Stopwatch();
            var elapsed = 0.0;

            foreach (var file in files)
            {
                elapsed = 0.0;

                var fileInfo = new FileInfo(file);
                var imageData = File.ReadAllBytes(file);

                for (var i = 0; i < retrys; i++)
                {
                    sw.Restart();
                    yoloWrapper.Detect(imageData);
                    sw.Stop();

                    elapsed += sw.Elapsed.TotalMilliseconds;
                }

                var average = elapsed / retrys;
                Console.WriteLine($"{fileInfo.Name} {average}ms");
            }

            yoloWrapper.Dispose();
        }
    }
}
