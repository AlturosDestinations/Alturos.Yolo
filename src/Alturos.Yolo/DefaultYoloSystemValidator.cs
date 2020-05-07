using Alturos.Yolo.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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

            var minVer = 14.16;
            //I don't now whether to use this version, but it was in list of versions.
            var minVerWithoutRedist = 14.0;

            using (var parentKey = Registry.ClassesRoot.OpenSubKey(@"Installer\Dependencies", false))
            {
                if (parentKey != null)
                {
                    var subKeys = new List<string>();
                    foreach (var key in parentKey.GetSubKeyNames())
                    {
                        if (key.Contains(",amd64,"))
                            subKeys.Add(key);
                    }

                    var regex = new Regex(@",(?<redist>redist\.x64)?,amd64,(?<version>[\d]+\.[\d]+),bundle");
                    foreach (var key in subKeys)
                    {
                        var match = regex.Match(key);
                        if (match.Success &&
                            double.TryParse(match.Groups["version"].Value,
                                            NumberStyles.Number, CultureInfo.InvariantCulture,
                                            out var version))
                        {
                            if ((!string.IsNullOrWhiteSpace(match.Groups["redist"].Value) && version >= minVer)
                             || version >= minVerWithoutRedist)
                                return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
