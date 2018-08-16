using Alturos.Yolo.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Alturos.Yolo
{
    public class YoloWrapper : IDisposable
    {
        public const int MaxObjects = 1000;
        private const string YoloLibraryCpu = @"x64\yolo_cpp_dll_cpu.dll";
        private const string YoloLibraryGpu = @"x64\yolo_cpp_dll_gpu.dll";

        private Dictionary<int, string> _objectType = new Dictionary<int, string>();
        private ImageAnalyzer _imageAnalyzer = new ImageAnalyzer();

        public DetectionSystem DetectionSystem = DetectionSystem.Unknown;
        public EnvironmentReport EnvironmentReport { get; private set; }

        #region DllImport Cpu

        [DllImport(YoloLibraryCpu, EntryPoint = "init")]
        private static extern int InitializeYoloCpu(string configurationFilename, string weightsFilename, int gpu);

        [DllImport(YoloLibraryCpu, EntryPoint = "detect_image")]
        internal static extern int DetectImageCpu(string filename, ref BboxContainer container);

        [DllImport(YoloLibraryCpu, EntryPoint = "detect_mat")]
        internal static extern int DetectImageCpu(IntPtr pArray, int nSize, ref BboxContainer container);

        [DllImport(YoloLibraryCpu, EntryPoint = "dispose")]
        internal static extern int DisposeYoloCpu();

        #endregion

        #region DllImport Gpu

        [DllImport(YoloLibraryGpu, EntryPoint = "init")]
        internal static extern int InitializeYoloGpu(string configurationFilename, string weightsFilename, int gpu);

        [DllImport(YoloLibraryGpu, EntryPoint = "detect_image")]
        internal static extern int DetectImageGpu(string filename, ref BboxContainer container);

        [DllImport(YoloLibraryGpu, EntryPoint = "detect_mat")]
        internal static extern int DetectImageGpu(IntPtr pArray, int nSize, ref BboxContainer container);

        [DllImport(YoloLibraryGpu, EntryPoint = "dispose")]
        internal static extern int DisposeYoloGpu();

        [DllImport(YoloLibraryGpu, EntryPoint = "get_device_count")]
        internal static extern int GetDeviceCount();

        [DllImport(YoloLibraryGpu, EntryPoint = "get_device_name")]
        internal static extern int GetDeviceName(int gpu, StringBuilder deviceName);

        #endregion

        public YoloWrapper(YoloConfiguration yoloConfiguration)
        {
            this.Initialize(yoloConfiguration.ConfigFile, yoloConfiguration.WeightsFile, yoloConfiguration.NamesFile, 0);
        }

        public YoloWrapper(string configurationFilename, string weightsFilename, string namesFilename, int gpu = 0)
        {
            this.Initialize(configurationFilename, weightsFilename, namesFilename, gpu);
        }

        public void Dispose()
        {
            switch (this.DetectionSystem)
            {
                case DetectionSystem.CPU:
                    DisposeYoloCpu();
                    break;
                case DetectionSystem.GPU:
                    DisposeYoloGpu();
                    break;
            }
        }

        private void Initialize(string configurationFilename, string weightsFilename, string namesFilename, int gpu = 0)
        {
            if (IntPtr.Size != 8)
            {
                throw new NotSupportedException("Only 64-bit process are supported");
            }

            this.EnvironmentReport = this.GetEnvironmentReport();
            if (!this.EnvironmentReport.MicrosoftVisualCPlusPlus2017RedistributableExists)
            {
                throw new DllNotFoundException("Microsoft Visual C++ 2017 Redistributable (x64)");
            }

            this.DetectionSystem = DetectionSystem.CPU;
            if (this.EnvironmentReport.CudaExists && this.EnvironmentReport.CudnnExists)
            {
                this.DetectionSystem = DetectionSystem.GPU;
            }

            switch (this.DetectionSystem)
            {
                case DetectionSystem.CPU:
                    InitializeYoloCpu(configurationFilename, weightsFilename, 0);
                    break;
                case DetectionSystem.GPU:
                    var deviceCount = GetDeviceCount();
                    if (gpu > (deviceCount - 1))
                    {
                        throw new IndexOutOfRangeException("Graphic device index is out of range");
                    }

                    var deviceName = new StringBuilder(); //allocate memory for string
                    GetDeviceName(gpu, deviceName);
                    this.EnvironmentReport.GraphicDeviceName = deviceName.ToString();

                    InitializeYoloGpu(configurationFilename, weightsFilename, gpu);
                    break;
            }

            var lines = File.ReadAllLines(namesFilename);
            for (var i = 0; i< lines.Length; i++)
            {
                this._objectType.Add(i, lines[i]);
            }
        }

        private EnvironmentReport GetEnvironmentReport()
        {
            var report = new EnvironmentReport();

            //https://stackoverflow.com/questions/12206314/detect-if-visual-c-redistributable-for-visual-studio-2012-is-installed/34209692#34209692
            using (var registryKey = Registry.ClassesRoot.OpenSubKey(@"Installer\Dependencies\,,amd64,14.0,bundle", false))
            {
                var displayName = registryKey.GetValue("DisplayName") as string;
                if (displayName.StartsWith("Microsoft Visual C++ 2017 Redistributable (x64)", StringComparison.OrdinalIgnoreCase))
                {
                    report.MicrosoftVisualCPlusPlus2017RedistributableExists = true;
                }
            }

            if (File.Exists(@"x64\cudnn64_7.dll"))
            {
                report.CudnnExists = true;
            }

            var envirormentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
            //if (envirormentVariables.Contains("cudnn"))
            //{
            //    //["CUDA_PATH"] = "C:\\Program Files\\NVIDIA GPU Computing Toolkit\\CUDA\\v9.2"
            //}

            if (envirormentVariables.Contains("CUDA_PATH"))
            {
                //var cudaVersion = envirormentVariables["CUDA_PATH"];
                report.CudaExists = true;
            }

            return report;
        }

        public IEnumerable<YoloItem> Detect(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("Cannot find the file", filepath);
            }

            var container = new BboxContainer();
            var count = 0;
            switch (this.DetectionSystem)
            {
                case DetectionSystem.CPU:
                    count = DetectImageCpu(filepath, ref container);
                    break;
                case DetectionSystem.GPU:
                    count = DetectImageGpu(filepath, ref container);
                    break;
            }

            if (count == -1)
            {
                throw new NotImplementedException("c++ dll compiled incorrectly");
            }

            return this.Convert(container);
        }

        public IEnumerable<YoloItem> Detect(byte[] imageData)
        {
            if (!this._imageAnalyzer.IsValidImageFormat(imageData))
            {
                throw new Exception("Invalid image data, wrong image format");
            }

            var container = new BboxContainer();
            var size = Marshal.SizeOf(imageData[0]) * imageData.Length;
            var pnt = Marshal.AllocHGlobal(size);

            try
            {
                // Copy the array to unmanaged memory.
                Marshal.Copy(imageData, 0, pnt, imageData.Length);
                var count = 0;
                switch (this.DetectionSystem)
                {
                    case DetectionSystem.CPU:
                        count = DetectImageCpu(pnt, imageData.Length, ref container);
                        break;
                    case DetectionSystem.GPU:
                        count = DetectImageGpu(pnt, imageData.Length, ref container);
                        break;
                }

                if (count == -1)
                {
                    throw new NotImplementedException("c++ dll compiled incorrectly");
                }
            }
            catch (Exception exception)
            {
                return null;
            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pnt);
            }

            return this.Convert(container);
        }

        private IEnumerable<YoloItem> Convert(BboxContainer container)
        {
            var yoloItems = new List<YoloItem>();
            foreach (var item in container.candidates.Where(o => o.h > 0 || o.w > 0))
            {
                var objectType = this._objectType[(int)item.obj_id];
                var yoloItem = new YoloItem() { X = (int)item.x, Y = (int)item.y, Height = (int)item.h, Width = (int)item.w, Confidence = item.prob, Type = objectType };
                yoloItems.Add(yoloItem);
            }

            return yoloItems;
        }
    }
}
