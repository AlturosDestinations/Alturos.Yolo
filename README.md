# Alturos.Yolo

Send an image to yolo and receive the position of the detected objects.

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
