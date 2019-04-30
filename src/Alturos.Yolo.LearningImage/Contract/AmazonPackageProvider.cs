using Alturos.Yolo.LearningImage.Model;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alturos.Yolo.LearningImage.Contract
{
    public class AmazonPackageProvider : IAnnotationPackageProvider
    {
        public bool IsSyncing { get; set; }

        private readonly IAmazonS3 _client;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string _bucketName;
        private readonly string _extractionFolder;
        private readonly Dictionary<string, double> _uploadPercentages;

        public AmazonPackageProvider()
        {
            var accessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            var secretAccessKey = ConfigurationManager.AppSettings["secretAccessKey"];

            this._bucketName = ConfigurationManager.AppSettings["bucketName"];
            this._extractionFolder = ConfigurationManager.AppSettings["extractionFolder"];

            this._client = new AmazonS3Client(accessKeyId, secretAccessKey, RegionEndpoint.EUWest1);
            this._dynamoDbClient = new AmazonDynamoDBClient(accessKeyId, secretAccessKey, RegionEndpoint.EUWest1);

            this._uploadPercentages = new Dictionary<string, double>();
        }

        public AnnotationPackage[] GetPackages()
        {
            // Retrieve unannotated metadata
            var context = new DynamoDBContext(this._dynamoDbClient);

            var results1 = context.Scan<AnnotationPackageInfo>(new ScanCondition("IsAnnotated", ScanOperator.IsNull));
            var results2 = context.Scan<AnnotationPackageInfo>(new ScanCondition("IsAnnotated", ScanOperator.Equal, false));

            var packageInfos = results1.Union(results2).ToList();

            // Create packages
            var packages = packageInfos.Select(o => new AnnotationPackage {
                Extracted = false,
                PackagePath = o.Id,
                DisplayName = Path.GetFileNameWithoutExtension(o.Id),
                Info = o
            }).ToList();

            // Get local folder if the package was already downloaded
            foreach (var package in packages)
            {
                var path = Path.Combine(this._extractionFolder, Path.GetFileNameWithoutExtension(package.DisplayName));
                if (Directory.Exists(path))
                {
                    package.Extracted = true;
                    package.PackagePath = path;
                }
            }

            return packages.ToArray();
        }

        public AnnotationPackage RefreshPackage(AnnotationPackage package)
        {
            package.Extracted = false;
            package.PackagePath = $"{package.DisplayName}.zip";
            return this.DownloadPackage(package);
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
                try
                {
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
                catch (Exception exception)
                {

                }

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
