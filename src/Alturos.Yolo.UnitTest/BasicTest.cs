using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Alturos.Yolo.UnitTest
{
    [TestClass]
    public class BasicTest
    {
        private string _imagePath = @"..\..\..\..\Images\Motorbike1.jpg";

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileNotFoundTest()
        {
            var configuration = new ConfigurationDetector().Detect();
            using (var yoloWrapper = new YoloWrapper(configuration))
            {
                yoloWrapper.Detect("image-not-exists.jpg");
            }
        }

        [TestMethod]
        public void DetectFromFilePath()
        {
            var configuration = new ConfigurationDetector().Detect();
            using (var yoloWrapper = new YoloWrapper(configuration))
            {
                var items = yoloWrapper.Detect(this._imagePath);
                Assert.IsTrue(items.Count() > 0);
            }
        }

        [TestMethod]
        public void DetectFromFileData()
        {
            var configuration = new ConfigurationDetector().Detect();
            using (var yoloWrapper = new YoloWrapper(configuration))
            {
                var imageData = File.ReadAllBytes(this._imagePath);
                var items = yoloWrapper.Detect(imageData);
                Assert.IsTrue(items.Count() > 0);
            }
        }
    }
}
