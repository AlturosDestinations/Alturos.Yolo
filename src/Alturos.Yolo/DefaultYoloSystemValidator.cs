using Alturos.Yolo.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Alturos.Yolo
{
    public class DefaultYoloSystemValidator : IYoloSystemValidator
    {
        public SystemValidationReport Validate()
        {
            var report = new SystemValidationReport();

#if NETSTANDARD

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                report.MicrosoftVisualCPlusPlusRedistributableExists = this.IsMicrosoftVisualCPlusPlus2017Available();
            }
            else
            {
                report.MicrosoftVisualCPlusPlusRedistributableExists = true;
            }

#endif

#if NET461
            report.MicrosoftVisualCPlusPlusRedistributableExists = this.IsMicrosoftVisualCPlusPlus2017Available();
#endif

            if (File.Exists("cudnn64_7.dll"))
            {
                report.CudnnExists = true;
            }

            var envirormentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
            if (envirormentVariables.Contains("CUDA_PATH"))
            {
                report.CudaExists = true;
            }
            if (envirormentVariables.Contains("CUDA_PATH_V10_2"))
            {
                report.CudaExists = true;
            }

            return report;
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
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.25,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" },
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.26,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" },
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.27,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" },
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.28,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" },
                { @"Installer\Dependencies\VC,redist.x64,amd64,14.29,bundle", "Microsoft Visual C++ 2015-2019 Redistributable (x64)" }
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
    }
}
