using Alturos.Yolo.Model;
using System.Collections.Generic;

namespace Alturos.Yolo.WebService.Contract
{
    public interface IObjectDetection
    {
        IEnumerable<YoloItem> Detect(byte[] imageData);
        IEnumerable<YoloItem> Detect(string filePath);
    }
}
