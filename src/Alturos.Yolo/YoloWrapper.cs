using Alturos.Yolo.Model;
using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace Alturos.Yolo
{
    public class YoloWrapper
    {
        [DllImport("yolo")]
        internal static extern int tr_context_new(string cfgfile, string namelistfile, string weightfile);

        [DllImport("yolo")]
        internal static extern int tr_process_image(IntPtr pArray, int nSize, ref CoreRectangleCandidateContainer container);

        [HandleProcessCorruptedStateExceptions]
        public bool Initialize(YoloConfiguration configuration)
        {
            try
            {
                var result = tr_context_new(configuration.ConfigFile, configuration.NamesFile, configuration.WeightsFile);
                if (result == -1)
                {
                    return false;
                }

                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public CoreRectangleCandidate[] ProcessImage(byte[] imageData)
        {
            var container = new CoreRectangleCandidateContainer();

            var size = Marshal.SizeOf(imageData[0]) * imageData.Length;
            IntPtr pnt = Marshal.AllocHGlobal(size);
            try
            {
                // Copy the array to unmanaged memory.
                Marshal.Copy(imageData, 0, pnt, imageData.Length);
                tr_process_image(pnt, imageData.Length, ref container);
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

            return container.candidates.Where(o => o.confidence > 0).ToArray();
        }
    }
}
