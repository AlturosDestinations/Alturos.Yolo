namespace Alturos.Yolo.Model
{
    public class EnvironmentReport
    {
        //Microsoft Visual C++ 2017/2019 Redistributable
        public bool MicrosoftVisualCPlusPlusRedistributableExists { get; set; }
        //Nvida CUDA Toolkit 9.2
        public bool CudaExists { get; set; }
        //Nvida cuDNN v7.1.4 for CUDA 9.2
        public bool CudnnExists { get; set; }
        //Graphic device name
        public string GraphicDeviceName { get; set; }
    }
}
