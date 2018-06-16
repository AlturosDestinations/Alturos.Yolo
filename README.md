# Alturos.Yolo

In this project you will find the source code to compile yolo so it can be accessed through C#. Also you will find a C# wrapper project with some test programs.

Send an image to [yolo](https://github.com/pjreddie/darknet) and receive the position of the detected objects. Our project is meant to return the object-type and -position as processable data.

![object detection result](doc/objectdetection.jpg)

Type | Confidence | X | Y | Width | Height |
--- | --- | --- | --- | --- | --- |
motorbike | 44.71 | 1932 | 699 | 411 | 441 |

### System requirements
- .NET Framework 4.6.1
- [Microsoft Visual C++ Visual Studio 2017](https://go.microsoft.com/fwlink/?LinkId=746572)

### Build requirements
- Visual Studio 2017
- x64 build

### nuget
The package is available on [nuget](https://www.nuget.org/packages/Alturos.Yolo)
```
PM> install-package Alturos.Yolo
PM> install-package Alturos.YoloV2TinyVocData
```

### Examples

#### Detect the type and the position of an image
```cs
var imageData = File.ReadAllBytes("myimage.jpg");
var yoloWrapper = new YoloWrapper();
yoloWrapper.Initialize(new YoloConfiguration("yolov2-tiny-voc.cfg", "yolov2-tiny-voc.weights", "voc.names"));
var items = yoloWrapper.ProcessImage(imageData);
```

### Performance (processing of one image 1024x683)

CPU | yolo v2 tiny voc | yolo v2  | yolo 9000 |
--- | --- | --- | --- | 
Intel i7 3770 (with OpenMP) | 557 ms | - | - | 
Intel i7 3770 (without OpenMP) | 1155 ms | - | - | 

Graphic card | Single precision | yolo v2 tiny voc | yolo v2  | yolo 9000 |
--- | --- | --- | --- | --- |
NVIDIA Quadro K420 | 300 GFLOPS | 94 ms | 296 ms | 640 ms | 
NVIDIA Quadro K620 | 768 GFLOPS | - | - | - | 
NVIDIA Quadro K1200 | 1151 GFLOPS | - | - | - | 
NVIDIA GeForce GT 710 | 366 GFLOPS | - | - | - | 
NVIDIA GeForce GT 730 | 693 GFLOPS | - | - | - | 
NVIDIA GeForce GT 1030 | 1098 GFLOPS | - | - | - | 
NVIDIA GeForce GTX 1060 | 4372 GFLOPS | - | - | - | 
