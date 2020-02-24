docker pull mcr.microsoft.com/dotnet/core/sdk
docker run -it mcr.microsoft.com/dotnet/core/sdk /bin/bash

apt-get update
apt-get install vim -y
wget https://github.com/AlturosDestinations/Alturos.Yolo/blob/master/Images/Bird1.png

mkdir test
cd test
dotnet new console
dotnet add package Alturos.Yolo --version 3.0.3-alpha
dotnet add package Alturos.YoloV2TinyVocData --version 1.0.0
apt-get install libgomp1



works only with jpg files failures with png

using System;
using System.IO;
using Alturos.Yolo;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Yolo Test - Choose file:");
            var fileName = Console.ReadLine();
            using (var yoloWrapper = new YoloWrapper("yolov2-tiny-voc.cfg", "yolov2-tiny-voc.weights", "voc.names"))
            {
                var imageData = File.ReadAllBytes(fileName);
                var items = yoloWrapper.Detect(imageData);
                foreach (var item in items)
                {
                    Console.WriteLine(item.Type);
                }
            }
        }
    }
}
