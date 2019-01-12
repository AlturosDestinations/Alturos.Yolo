namespace Alturos.Yolo.Model
{
    public class YoloPreTrainedData
    {
        public string Name { get; set; }
        public string ConfigFileUrl { get; set; }
        public string NamesFileUrl { get; set; }
        public string WeightsFileUrl { get; set; }
        public string[] OptionalFileUrls { get; set; }
    }
}
