namespace Alturos.Yolo.LearningImage.Contract
{
    public interface IBoundingBoxReader
    {
        AnnotationInfo[] GetBoxes(string dataPath);
        string GetDataPath(string imagePath);
    }
}
