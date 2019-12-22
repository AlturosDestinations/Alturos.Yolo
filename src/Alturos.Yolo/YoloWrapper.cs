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

        private readonly ImageAnalyzer _imageAnalyzer = new ImageAnalyzer();
        private YoloObjectTypeResolver _objectTypeResolver;

        public DetectionSystem DetectionSystem { get; private set; } = DetectionSystem.Unknown;
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

        /// <summary>
        /// Initialize Yolo
        /// </summary>
        /// <param name="yoloConfiguration"></param>
        /// <param name="ignoreGpu">Disable automatic gpu detection</param>
        /// <exception cref="NotSupportedException">Thrown when the process not run in 64bit</exception>
        /// <exception cref="DllNotFoundException">Thrown when Microsoft Visual C++ 2017 Redistributable not available</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when the gpu index is not valid</exception>
        public YoloWrapper(YoloConfiguration yoloConfiguration, bool ignoreGpu = false)
        {
            this.Initialize(yoloConfiguration.ConfigFile, yoloConfiguration.WeightsFile, yoloConfiguration.NamesFile, 0, ignoreGpu);
        }

        /// <summary>
        /// Initialize Yolo
        /// </summary>
        /// <param name="configurationFilename">Yolo configuration (.cfg) file path</param>
        /// <param name="weightsFilename">Yolo trainded data (.weights) file path</param>
        /// <param name="namesFilename">Yolo object names (.names) file path</param>
        /// <param name="gpu">Gpu Index if multiple graphic devices available</param>
        /// <param name="ignoreGpu">Disable automatic gpu detection</param>
        /// <exception cref="NotSupportedException">Thrown when the process not run in 64bit</exception>
        /// <exception cref="DllNotFoundException">Thrown when Microsoft Visual C++ 2017 Redistributable not available</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when the gpu index is not valid</exception>
        public YoloWrapper(string configurationFilename, string weightsFilename, string namesFilename, int gpu = 0, bool ignoreGpu = false)
        {
            this.Initialize(configurationFilename, weightsFilename, namesFilename, gpu, ignoreGpu);
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

        private void Initialize(string configurationFilename, string weightsFilename, string namesFilename, int gpu = 0, bool ignoreGpu = false, bool ignoreCPlusPlusDetection = false)
        {
            if (IntPtr.Size != 8)
            {
                throw new NotSupportedException("Only 64-bit processes are supported");
            }

            this.EnvironmentReport = this.GetEnvironmentReport();
            this.DetectionSystem = DetectionSystem.CPU;

            if (!ignoreCPlusPlusDetection && !this.EnvironmentReport.MicrosoftVisualCPlusPlusRedistributableExists)
            {
                throw new DllNotFoundException("Microsoft Visual C++ 2017-2019 Redistributable (x64)");
            }

            if (!ignoreGpu && this.EnvironmentReport.CudaExists && this.EnvironmentReport.CudnnExists)
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
                    if (deviceCount == 0)
                    {
                        throw new NotSupportedException("No graphic device is available");
                    }

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

            this._objectTypeResolver = new YoloObjectTypeResolver(namesFilename);
        }

        private bool IsMicrosoftVisualCPlusPlus2017Available()
        {
            //Detect if Visual C++ Redistributable for Visual Studio is installed
            //https://stackoverflow.com/questions/12206314/detect-if-visual-c-redistributable-for-visual-studio-2012-is-installed/
            var checkKeys = new Dictionary<string, string>
            {
                { @"Installer\Dependencies\,,amd64,14.0,bundle", "Microsoft Visual C++ 2017 Redistributable (x64)" },
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.16,bundle", "Microsoft Visual C++ 2017 Redistributable (x64)" },
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.20,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" }, 
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.21,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" },
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.22,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" },
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.23,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" },
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.24,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" },
            };

            foreach (var checkKey in checkKeys)
            {
                using (var registryKey = Registry.ClassesRoot.OpenSubKey(checkKey.Key, false))
                {
                    if (registryKey == null)
                    {
                        continue;
                    }

                    var displayName = registryKey.GetValue("DisplayName") as string;
                    if (string.IsNullOrEmpty(displayName))
                    {
                        continue;
                    }

                    if (displayName.StartsWith(checkKey.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private EnvironmentReport GetEnvironmentReport()
        {
            var report = new EnvironmentReport
            {
                MicrosoftVisualCPlusPlusRedistributableExists = this.IsMicrosoftVisualCPlusPlus2017Available()
            };

            if (File.Exists(@"x64\cudnn64_7.dll"))
            {
                report.CudnnExists = true;
            }

            var envirormentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
            if (envirormentVariables.Contains("CUDA_PATH"))
            {
                report.CudaExists = true;
            }
            if (envirormentVariables.Contains("CUDA_PATH_V10_1"))
            {
                report.CudaExists = true;
            }

            return report;
        }

        /// <summary>
        /// Detect objects on an image
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Thrown when the filepath is wrong</exception>
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
                throw new NotImplementedException("C++ dll compiled incorrectly");
            }

            return this.Convert(container);
        }

        /// <summary>
        /// Detect objects on an image
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Thrown when the yolo_cpp dll is wrong compiled</exception>
        public IEnumerable<YoloItem> Detect(byte[] imageData)
        {
            if (!this._imageAnalyzer.IsValidImageFormat(imageData))
            {
                throw new Exception("Invalid image data, wrong image format");
            }

            var container = new BboxContainer();
            var size = Marshal.SizeOf(imageData[0]) * imageData.Length;
            var pnt = Marshal.AllocHGlobal(size);

            var count = 0;
            try
            {
                // Copy the array to unmanaged memory.
                Marshal.Copy(imageData, 0, pnt, imageData.Length);
                switch (this.DetectionSystem)
                {
                    case DetectionSystem.CPU:
                        count = DetectImageCpu(pnt, imageData.Length, ref container);
                        break;
                    case DetectionSystem.GPU:
                        count = DetectImageGpu(pnt, imageData.Length, ref container);
                        break;
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pnt);
            }

            if (count == -1)
            {
                throw new NotImplementedException("C++ dll compiled incorrectly");
            }

            return this.Convert(container);
        }

        private IEnumerable<YoloItem> Convert(BboxContainer container)
        {
            var yoloItems = new List<YoloItem>();
            foreach (var item in container.candidates.Where(o => o.h > 0 || o.w > 0))
            {
                var yoloItem = new YoloItem
                {
                    X = (int)item.x,
                    Y = (int)item.y,
                    Height = (int)item.h,
                    Width = (int)item.w,
                    Confidence = item.prob,
                    Type = this._objectTypeResolver.Resolve((int)item.obj_id)
                };

                yoloItems.Add(yoloItem);
            }

            return yoloItems;
        }
    }
}
