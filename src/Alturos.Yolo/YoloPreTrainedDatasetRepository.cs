using Alturos.Yolo.Model;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alturos.Yolo
{
    public class YoloPreTrainedDatasetRepository
    {
        private readonly YoloPreTrainedData[] _preTrainedDatas;

        public YoloPreTrainedDatasetRepository()
        {
            this._preTrainedDatas = new YoloPreTrainedData[]
            {
                new YoloPreTrainedData
                {
                    Name = "YOLOv3",
                    ConfigFileUrl = "https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/yolov3.cfg",
                    NamesFileUrl = "https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/coco.names",
                    WeightsFileUrl = "https://pjreddie.com/media/files/yolov3.weights"
                },
                new YoloPreTrainedData
                {
                    Name = "YOLOv3-tiny",
                    ConfigFileUrl = "https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/yolov3-tiny.cfg",
                    NamesFileUrl = "https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/coco.names",
                    WeightsFileUrl = "https://pjreddie.com/media/files/yolov3-tiny.weights"
                },
                new YoloPreTrainedData
                {
                    Name = "YOLOv2",
                    ConfigFileUrl = "https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/yolov2.cfg",
                    NamesFileUrl = "https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/coco.names",
                    WeightsFileUrl = "https://pjreddie.com/media/files/yolov2.weights"
                },
                new YoloPreTrainedData
                {
                    Name = "YOLOv2-tiny",
                    ConfigFileUrl = "https://raw.githubusercontent.com/AlexeyAB/darknet/master/cfg/yolov2-tiny.cfg",
                    NamesFileUrl = "https://raw.githubusercontent.com/pjreddie/darknet/master/data/voc.names",
                    WeightsFileUrl = "https://pjreddie.com/media/files/yolov2-tiny.weights"
                }
            };
        }

        public async Task<string[]> GetDatasetsAsync()
        {
            var names = this._preTrainedDatas.Select(o => o.Name).ToArray();
            return await Task.FromResult(names);
        }

        public async Task<bool> DownloadDatasetAsync(string name, string destinationPath)
        {
            Directory.CreateDirectory(destinationPath);

            var preTrainedData = this._preTrainedDatas.Where(o => o.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (preTrainedData == null)
            {
                return false;
            }

            if (!await this.DownloadAsync(preTrainedData.ConfigFileUrl, destinationPath).ConfigureAwait(false))
            {
                return false;
            }

            if (!await this.DownloadAsync(preTrainedData.NamesFileUrl, destinationPath).ConfigureAwait(false))
            {
                return false;
            }

            if (!await this.DownloadAsync(preTrainedData.WeightsFileUrl, destinationPath).ConfigureAwait(false))
            {
                return false;
            }

            if (preTrainedData.OptionalFileUrls != null)
            {
                foreach (var optionalFile in preTrainedData.OptionalFileUrls)
                {
                    if (!await this.DownloadAsync(optionalFile, destinationPath).ConfigureAwait(false))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private async Task<bool> DownloadAsync(string url, string destinationPath)
        {
            var uri = new Uri(url);
            var fileName = Path.GetFileName(uri.LocalPath);
            var filePath = Path.Combine(destinationPath, fileName);

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(10);

                using (var httpResponseMessage = await httpClient.GetAsync(url).ConfigureAwait(false))
                {
                    if (!httpResponseMessage.IsSuccessStatusCode)
                    {
                        return false; ;
                    }

                    var fileContentStream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    using (var sourceStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                    {
                        fileContentStream.Seek(0, SeekOrigin.Begin);
                        await fileContentStream.CopyToAsync(sourceStream);
                    }

                    return true;
                }
            }
        }
    }
}
