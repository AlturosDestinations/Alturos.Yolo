Thanks for using the Alturos.Yolo package (https://github.com/AlturosDestinations/Alturos.Yolo)
----------------------------------------------------------------
Please star (★) this project on github!


This package deliver all dependencies for cpu detection.

If you need gpu detection please install cuda and cudnn.
Nvidia CUDA Toolkit 10.0 (must be installed add a hardware driver for cuda support)
Nvidia cuDNN v7.4.2.24 for CUDA 10.0 (DLL cudnn64_7.dll required for gpu processing)
And copy the cudnn64_7.dll in the x64 directory. (%cudnn%\bin)

If all dependencies available Alturos.Yolo switch automatic in gpu mode.
For an easy start please install-package Alturos.YoloV2TinyVocData (Yolo Pre-trained model)

Example code:
----------------------------------------------------------------

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
