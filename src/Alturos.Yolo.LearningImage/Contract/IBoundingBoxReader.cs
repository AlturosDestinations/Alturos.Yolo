using Alturos.Yolo.LearningImage.Model;

namespace Alturos.Yolo.LearningImage.Contract
{
    public interface IBoundingBoxReader
    {
        AnnotationBoundingBox[] GetBoxes(string dataPath);
        string GetDataPath(string imagePath);
    }
}
