using System;

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
            var configurationDetector = new YoloConfigurationDetector();
            var config = configurationDetector.Detect();

            using (var yoloWrapper = new YoloWrapper(config))
            {
                var result = yoloWrapper.Detect(@"image.jpg");
            }
        }
    }
}
