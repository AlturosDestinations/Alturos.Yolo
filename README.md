![Alturos.Yolo](doc/logo-banner.png)

# Alturos.Yolo

A C# wrapper project for using [AlexeyAB/darknet](https://github.com/AlexeyAB/darknet) dll project.

Send an image to [yolo](https://github.com/pjreddie/darknet) and receive the position of the detected objects. Our project is meant to return the object-type and -position as processable data.

![object detection result](doc/objectdetection.jpg)

Type | Confidence | X | Y | Width | Height |
--- | --- | --- | --- | --- | --- |
motorbike | 44.71 | 1932 | 699 | 411 | 441 |

## System requirements
- .NET Framework 4.6.1
- [Microsoft Visual C++ Visual Studio 2017](https://go.microsoft.com/fwlink/?LinkId=746572)

## Build requirements
- Visual Studio 2017

## GPU Requirements (optional)
1) [Install Nvidia CUDA Toolkit 9.2](https://developer.nvidia.com/cuda-downloads) (must be installed add a hardware driver for cuda support)
2) [Download Nvidia cuDNN v7.1.4 for CUDA 9.2](https://developer.nvidia.com/rdp/cudnn-download) (DLL cudnn64_7.dll required for gpu processing)

## nuget
The package is available on [nuget](https://www.nuget.org/packages/Alturos.Yolo)
```
PM> install-package Alturos.Yolo
PM> install-package Alturos.YoloV2TinyVocData
```

## Examples

### Detect the type and the position of an image
```cs
var configurationDetector = new ConfigurationDetector();
var config = configurationDetector.Detect();
//using (var yoloWrapper = new YoloWrapper("yolov2-tiny-voc.cfg", "yolov2-tiny-voc.weights", "voc.names"))
using (var yoloWrapper = new YoloWrapper(config))
{
	var items = yoloWrapper.Detect(@"image.jpg");
	//items[0].Type -> "Person, Car, ..."
	//items[0].Confidence -> 0.0 (low) -> 1.0 (high)
	//items[0].X -> bounding box
	//items[0].Y -> bounding box
	//items[0].Width -> bounding box
	//items[0].Height -> bounding box
}
```

## Benchmark / Performance (processing of one image 1024x683)

### CPU

CPU | yolo v2 tiny voc | yolo v2  | yolo 9000 |
--- | --- | --- | --- | 
Intel i7 3770 (with OpenMP) | 557 ms | - | - | 
Intel i7 3770 (without OpenMP) | 1155 ms | - | - | 

### GPU

Graphic card | Single precision | yolo v2 tiny voc | yolo v2  | yolo 9000 |
--- | --- | --- | --- | --- |
NVIDIA Quadro K420 | 300 GFLOPS | 94 ms | 296 ms | 640 ms | 
NVIDIA Quadro K620 | 768 GFLOPS | - | - | - | 
NVIDIA Quadro K1200 | 1151 GFLOPS | - | - | - | 
NVIDIA GeForce GT 710 | 366 GFLOPS | - | - | - | 
NVIDIA GeForce GT 730 | 693 GFLOPS | - | - | - | 
NVIDIA GeForce GT 1030 | 1098 GFLOPS | 23 ms | 64 ms| 180 ms | 
NVIDIA GeForce GTX 1060 | 4372 GFLOPS | - | - | - | 
