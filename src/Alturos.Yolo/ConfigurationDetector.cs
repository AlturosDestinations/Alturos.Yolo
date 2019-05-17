using System.IO;
using System.Linq;

namespace Alturos.Yolo
{
    public class ConfigurationDetector
    {
        /// <summary>
        /// Automatict detect the yolo configuration on the given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Thrown when cannot found one of the required yolo files</exception>
        public YoloConfiguration Detect(string path = ".")
        {
            var files = this.GetYoloFiles(path);
            var yoloConfiguration = this.MapFiles(files);
            var configValid = this.AreValidYoloFiles(yoloConfiguration);

            if (configValid)
            {
                return yoloConfiguration;
            }

            throw new FileNotFoundException("Cannot found pre-trained model, check all config files available (.cfg, .weights, .names)");
        }

        private string[] GetYoloFiles(string path)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly).Where(o => o.EndsWith(".names") || o.EndsWith(".cfg") || o.EndsWith(".weights")).ToArray();
        }

        private YoloConfiguration MapFiles(string[] files)
        {
            var configurationFile = files.FirstOrDefault(o => o.EndsWith(".cfg"));
            var weightsFile = files.FirstOrDefault(o => o.EndsWith(".weights"));
            var namesFile = files.FirstOrDefault(o => o.EndsWith(".names"));

            return new YoloConfiguration(configurationFile, weightsFile, namesFile);
        }

        private bool AreValidYoloFiles(YoloConfiguration config)
        {
            if (string.IsNullOrEmpty(config.ConfigFile) ||
                string.IsNullOrEmpty(config.WeightsFile) ||
                string.IsNullOrEmpty(config.NamesFile))
            {
                return false;
            }

            return true;
        }
    }
}
