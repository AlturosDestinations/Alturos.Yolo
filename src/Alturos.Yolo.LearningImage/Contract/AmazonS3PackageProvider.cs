using Alturos.Yolo.LearningImage.Model;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alturos.Yolo.LearningImage.Contract
{
    public class AmazonS3PackageProvider : IAnnotationPackageProvider
    {
        public bool IsSyncing { get; set; }

        private readonly IAmazonS3 _client;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string _bucketName;
        private readonly string _extractionFolder;
        private readonly string _accessKeyId;
        private readonly string _secretAccessKey;
        private readonly Dictionary<string, double> _uploadPercentages;

        public AmazonS3PackageProvider()
        {
            this._accessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            this._secretAccessKey = ConfigurationManager.AppSettings["secretAccessKey"];

            this._bucketName = ConfigurationManager.AppSettings["bucketName"];
            this._extractionFolder = ConfigurationManager.AppSettings["extractionFolder"];

            this._client = new AmazonS3Client(this._accessKeyId, this._secretAccessKey, RegionEndpoint.EUWest1);
            this._dynamoDbClient = new AmazonDynamoDBClient(this._accessKeyId, this._secretAccessKey, RegionEndpoint.EUWest1);

            this._uploadPercentages = new Dictionary<string, double>();
        }

        public AnnotationPackage[] GetPackages()
        {
            var packageInfos = new List<S3FileInfo>();

            var packages = new List<AnnotationPackage>();

            var dir = new S3DirectoryInfo(this._client, this._bucketName);
            foreach (var file in dir.GetFiles())
            {
                packageInfos.Add(file);

                var package = new AnnotationPackage
                {
                    Extracted = false,
                    PackagePath = file.Name,
                    DisplayName = Path.GetFileNameWithoutExtension(file.Name),
                };

                var context = new DynamoDBContext(this._dynamoDbClient);
                var info = context.Load<AnnotationPackageInfo>(file.Name);
                package.Info = info;

                packages.Add(package);
            }

            return packages.ToArray();
        }

        public AnnotationPackage DownloadPackage(AnnotationPackage package)
        {
            if (!Directory.Exists(this._extractionFolder))
            {
                Directory.CreateDirectory(this._extractionFolder);
            }

            var dir = new S3DirectoryInfo(this._client, this._bucketName);
            var file = dir.GetFile(package.PackagePath);

            var zipFilePath = Path.Combine(this._extractionFolder, file.Name);
            var fileInfo = file.CopyToLocal(zipFilePath, true);

            package.PackagePath = fileInfo.FullName;
            package.DisplayName = Path.GetFileNameWithoutExtension(fileInfo.FullName);

            return package;
        }

        public async Task SyncPackages(AnnotationPackage[] packages)
        {
            this.IsSyncing = true;
            this._uploadPercentages.Clear();
            
            var tasks = new List<Task>();
            foreach (var package in packages)
            {
                this._uploadPercentages[package.PackagePath] = 0;
                tasks.Add(Task.Run(() => this.UploadAsync(package)));
            }

            await Task.WhenAll(tasks);

            this.IsSyncing = false;
        }

        private async Task UploadAsync(AnnotationPackage package)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                BucketName = this._bucketName,
                FilePath = package.PackagePath
            };

            uploadRequest.UploadProgressEvent += this.UploadRequest_UploadProgressEvent;

            using (var fileTransferUtility = new TransferUtility(this._client))
            {
                await fileTransferUtility.UploadAsync(uploadRequest);

                var context = new DynamoDBContext(this._dynamoDbClient);
                context.Save(package.Info);
            }

            uploadRequest.UploadProgressEvent -= this.UploadRequest_UploadProgressEvent;
        }

        private void UploadRequest_UploadProgressEvent(object sender, UploadProgressArgs e)
        {
            this._uploadPercentages[e.FilePath] = e.PercentDone;
        }

        public double GetSyncProgress()
        {
            if (!this.IsSyncing)
            {
                return 0;
            }

            var totalPercentage = 0.0;
            foreach (var value in this._uploadPercentages.Values)
            {
                totalPercentage += value;
            }

            return totalPercentage / this._uploadPercentages.Count;
        }
    }
}
