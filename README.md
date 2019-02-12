![Alturos.Yolo](doc/logo-banner.png)

# Alturos.Yolo

A state of the art real-time object detection system for C# (Visual Studio). This project has CPU and GPU support, with GPU the detection works much faster. The primary goal of this project is an easy use of yolo, this package is available on nuget and you must only install two packages to start detection. In the background we are use the Windows Yolo version of [AlexeyAB/darknet](https://github.com/AlexeyAB/darknet). Send an image path or the byte array to [yolo](https://github.com/pjreddie/darknet) and receive the position of the detected objects. Our project is meant to return the object-type and -position as processable data. This library supports [YoloV3 and YoloV2 Pre-Trained Datasets](#pre-trained-dataset)

## nuget
Quick install Alturos.Yolo over [nuget](https://www.nuget.org/packages/Alturos.Yolo)
```
PM> install-package Alturos.Yolo (C# wrapper and C++ dlls 22MB)
PM> install-package Alturos.YoloV2TinyVocData (YOLOv2-tiny Pre-Trained Dataset 56MB)
```

## Object Detection

![object detection result](doc/objectdetectionanimated.png)

## Example code

### Detect the type and the position of an image (Automatic configuration)
```cs
var configurationDetector = new ConfigurationDetector();
var config = configurationDetector.Detect();
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

### Detect the type and the position of an image (Manual configuration)
```cs
using (var yoloWrapper = new YoloWrapper("yolov2-tiny-voc.cfg", "yolov2-tiny-voc.weights", "voc.names"))
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

## System requirements
- .NET Framework 4.6.1
- [Microsoft Visual C++ 2017 Redistributable x64](https://go.microsoft.com/fwlink/?LinkId=746572)

## GPU requirements (optional)
1) [Install Nvidia CUDA Toolkit 10.0](https://developer.nvidia.com/cuda-downloads) (must be installed add a hardware driver for cuda support)
2) [Download Nvidia cuDNN v7.4.2.24 for CUDA 10.0](https://developer.nvidia.com/rdp/cudnn-download)
3) Copy the `cudnn64_7.dll` from the output directory of point 2. into the `x64` folder of your project.

## Build requirements
- Visual Studio 2017

## Benchmark / Performance
Average processing speed of test images bird1.png, bird2.png, car1.png, motorbike1.png

### CPU

Processor | YOLOv2-tiny | YOLOv3 | yolo9000 |
--- | --- | --- | --- | 
Intel i7 3770 | 400 ms | 2380 ms | - | 
Intel Xeon E5-1620 v3 | 207 ms | 4327 ms | - | 
Intel Xeon E3-1240 v6 | 182 ms | 3213 ms | - | 

### GPU

Graphic card | Single precision | Memory | Slot | YOLOv2-tiny | YOLOv3 | yolo9000 |
--- | --- | --- | --- | --- | --- | --- |
NVIDIA Quadro K420 | 300 GFLOPS | 2 GB | Single | - | - | - |
NVIDIA Quadro K620 | 768 GFLOPS | 2 GB | Single | - | - | - |
NVIDIA Quadro K1200 | 1151 GFLOPS | 4 GB | Single | - | - | - |
NVIDIA Quadro P400 | 599 GFLOPS | 2 GB | Single | - | - | - |
NVIDIA Quadro P600 | 1117 GFLOPS | 2 GB | Single | - | - | - |
NVIDIA Quadro P620 | 1386 GFLOPS | 2 GB | Single | - | - | - |
NVIDIA Quadro P1000 | 1862 GFLOPS | 4 GB | Single | - | - | - |
NVIDIA Quadro P2000 | 3011 GFLOPS | 5 GB | Single | - | - | - |
NVIDIA Quadro P4000 | 5304 GFLOPS | 8 GB | Single | - | - | - |
NVIDIA Quadro P5000 | 8873 GFLOPS | 16 GB | Dual | - | - | - |
NVIDIA GeForce GT 710 | 366 GFLOPS | 2 GB | Single | - | - | - |
NVIDIA GeForce GT 730 | 693 GFLOPS | 2-4 GB | Single | - | - | - |
NVIDIA GeForce GT 1030 | 1098 GFLOPS | 2 GB | Single | 40 ms | 160 ms | - |
NVIDIA GeForce GTX 1060 | 4372 GFLOPS | 6 GB | Dual | 25 ms | 100 ms | - |


## Pre-Trained Dataset

You can download the datasets manually or integrate them automatically into the code.

```cs
//The download takes some time depending on the internet connection.
var repository = new YoloPreTrainedDatasetRepository();
await repository.DownloadDatasetAsync("YOLOv3", ".");
```

Model | Processing Resolution | Cfg | Weights | Names |
--- | --- | --- | --- | --- |
YOLOv3 | 608x608 | [yolov3.cfg](https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/yolov3.cfg) | [yolov3.weights](https://pjreddie.com/media/files/yolov3.weights) | [coco.names](https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/coco.names) |
YOLOv3-tiny | 416x416 | [yolov3-tiny.cfg](https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/yolov3-tiny.cfg) | [yolov3.weights](https://pjreddie.com/media/files/yolov3.weights) | [coco.names](https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/coco.names) |
YOLOv2 | 608x608 | [yolov2.cfg](https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/yolov2.cfg) | [yolov2.weights](https://pjreddie.com/media/files/yolov2.weights) | [coco.names](https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/coco.names) |
YOLOv2-tiny | 416x416 | [yolov2-tiny.cfg](https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/yolov2-tiny.cfg) | [yolov2-tiny.weights](https://pjreddie.com/media/files/yolov2-tiny.weights) | [voc.names](https://raw.githubusercontent.com/pjreddie/darknet/master/data/voc.names) |
yolo9000 | 448x448 | [darknet9000.cfg](https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/yolo9000.cfg) | [yolo9000.weights](https://github.com/philipperemy/yolo-9000/tree/master/yolo9000-weights) | [9k.names](https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/9k.names) |

## Annotation Tool

To marking bounded boxes of objects in images for training neural network you can use [VoTT](https://github.com/Microsoft/VoTT)

## Debugging Gpu Tool

Check graphic device usage `"%PROGRAMFILES%\NVIDIA Corporation\NVSMI\nvidia-smi.exe"`

## Troubleshooting

If you have some error like `DllNotFoundException` use [dependencywalker](http://www.dependencywalker.com) to check all references are available for `yolo_cpp_dll_gpu.dll`

## Dataset of tagged images

http://cocodataset.org/

## Directory Structure
    .
    ├── Alturos.Yolo.dll              # C# yolo wrapper
    ├── x64/
    │   ├── yolo_cpp_dll_cpu.dll      # yolo runtime for cpu
    │   ├── yolo_cpp_dll_gpu.dll      # yolo runtime for gpu
    │   ├── cudnn64_7.dll             # required by yolo_cpp_dll_gpu (optional only required for gpu processig)
    │   ├── opencv_world340.dll       # required by yolo_cpp_dll_xxx (process image as byte data detect_mat)
    │   ├── pthreadGC2.dll            # required by yolo_cpp_dll_xxx (POSIX Threads)
    │   ├── pthreadVC2.dll            # required by yolo_cpp_dll_xxx (POSIX Threads)
    │   ├── msvcr100.dll              # required by pthread (POSIX Threads)
