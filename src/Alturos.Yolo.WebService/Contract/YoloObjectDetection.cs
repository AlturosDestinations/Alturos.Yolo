using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;

namespace Alturos.Yolo.WebService.Contract
{
    public class YoloObjectDetection : IObjectDetection, IDisposable
    {
        private YoloWrapper _yoloWrapper;

        public YoloObjectDetection()
        {
            var configurationDetector = new ConfigurationDetector();
            var configuration = configurationDetector.Detect();
            this._yoloWrapper = new YoloWrapper(configuration);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this._yoloWrapper?.Dispose();
        }

        public IEnumerable<YoloItem> Detect(byte[] imageData)
        {
            return this._yoloWrapper.Detect(imageData);
        }

        public IEnumerable<YoloItem> Detect(string filePath)
        {
            return this._yoloWrapper.Detect(filePath);
        }
    }
}
