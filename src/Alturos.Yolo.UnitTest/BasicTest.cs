﻿using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alturos.Yolo.UnitTest
{
    [TestClass]
    public class BasicTest
    {
        private readonly string _imagePath = @"..\..\..\..\Images\Motorbike1.png";

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileNotFoundTest()
        {
            var configuration = new YoloConfigurationDetector().Detect();
            using (var yoloWrapper = new YoloWrapper(configuration))
            {
                yoloWrapper.Detect("image-not-exists.jpg");
            }
        }

        [TestMethod]
        public void DetectFromFilePath()
        {
            var configuration = new YoloConfigurationDetector().Detect();
            using (var yoloWrapper = new YoloWrapper(configuration))
            {
                var items = yoloWrapper.Detect(this._imagePath);
                Assert.IsTrue(items.Any());
            }
        }

        [TestMethod]
        public void DetectFromFileData()
        {
            var configuration = new YoloConfigurationDetector().Detect();
            using (var yoloWrapper = new YoloWrapper(configuration))
            {
                var imageData = File.ReadAllBytes(this._imagePath);
                var items = yoloWrapper.Detect(imageData);
                Assert.IsTrue(items.Any());
            }
        }
    }
}
