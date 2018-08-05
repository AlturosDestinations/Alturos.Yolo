using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alturos.Yolo
{
    class YoloTracking
    {
    }
}


//using Alturos.Yolo;
//using Alturos.Yolo.Model;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;

//namespace Test.YoloTracking
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var configurationDetector = new ConfigurationDetector();
//            var config = configurationDetector.Detect();
//            var sw = new Stopwatch();
//            Directory.CreateDirectory("result");


//            //var trackingObject = new Point(216, 343);
//            //var files = Directory.GetFiles(@"C:\Users\tinoh\Desktop\Test Yolo Tracking\test");

//            //var trackingObject = new Point(907, 215);
//            //var files = Directory.GetFiles(@"C:\Users\tinoh\Desktop\Test Yolo Tracking\test2");

//            //var trackingObject = new Point(120, 420); //Second car
//            var trackingObject = new Point(385, 480); //First car
//            var files = Directory.GetFiles(@"C:\Users\tinoh\Desktop\Test Yolo Tracking\test3");


//            var yoloWrapper = new YoloWrapper(config);

//            foreach (var file in files)
//            {
//                var imageData = File.ReadAllBytes(file);
//                var items = yoloWrapper.Detect(imageData);

//                sw.Start();
//                var probableObject = FindBestMatch(items, trackingObject, 10000);
//                sw.Stop();

//                if (probableObject == null)
//                {
//                    continue;
//                }

//                trackingObject = probableObject.Center();
//                DrawImage(file, probableObject);
//            }

//            Console.WriteLine(sw.Elapsed.TotalMilliseconds);
//            yoloWrapper.Dispose();
//            Console.ReadLine();
//        }

//        private static void DrawImage(string file, YoloItem item)
//        {
//            using (var image = Image.FromFile(file))
//            using (var canvas = Graphics.FromImage(image))
//            using (var pen = new Pen(Brushes.Pink, 3))
//            {

//                canvas.DrawRectangle(pen, item.X, item.Y, item.Width, item.Height);
//                canvas.Flush();


//                var fileInfo = new FileInfo(file);

//                image.Save($@"result\{fileInfo.Name}");
//            }
//        }

//        static YoloItem FindBestMatch(IEnumerable<YoloItem> items, Point trackingObject, int maxDistance)
//        {
//            var distanceItems = items.Select(o => new { Distance = Distance2(o.Center(), trackingObject), Item = o }).Where(o => o.Distance <= maxDistance).OrderBy(o => o.Distance);
//            var bestMatch = distanceItems.FirstOrDefault();
//            return bestMatch?.Item;
//        }

//        private static double Distance2(Point p1, Point p2)
//        {
//            return Pow2(p2.X - p1.X) + Pow2(p2.Y - p1.Y);
//        }

//        private static double Pow2(double x)
//        {
//            return x * x;
//        }
//    }

//    public static class YoloItemExtension
//    {
//        public static Point Center(this YoloItem item)
//        {
//            return new Point(item.X + item.Width / 2, item.Y + item.Height / 2);
//        }
//    }
//}
