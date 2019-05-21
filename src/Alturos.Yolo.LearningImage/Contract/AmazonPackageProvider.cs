using Alturos.Yolo.LearningImage.Model;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
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
        private readonly List<AnnotationPackage> _currentlyDownloadedPackages;

        private int _packagesToSync;
        private int _syncedPackages;

        public AmazonPackageProvider()
        {
            var accessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            var secretAccessKey = ConfigurationManager.AppSettings["secretAccessKey"];

            this._bucketName = ConfigurationManager.AppSettings["bucketName"];
            this._extractionFolder = ConfigurationManager.AppSettings["extractionFolder"];

            this._client = new AmazonS3Client(accessKeyId, secretAccessKey, RegionEndpoint.EUWest1);
            this._dynamoDbClient = new AmazonDynamoDBClient(accessKeyId, secretAccessKey, RegionEndpoint.EUWest1);

            this._currentlyDownloadedPackages = new List<AnnotationPackage>();
        }

        public async Task<AnnotationPackage[]> GetPackagesAsync()
        {
            // Retrieve unannotated metadata
            using (var context = new DynamoDBContext(this._dynamoDbClient))
            {
                try
                {
                    var packageInfos = context.ScanAsync<AnnotationPackageInfo>(new ScanCondition[] { new ScanCondition("IsAnnotated", ScanOperator.Equal, false) });

                    // Create packages
                    var packages = (await packageInfos.GetNextSetAsync()).Select(o => new AnnotationPackage
                    {
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
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<AnnotationPackage> RefreshPackageAsync(AnnotationPackage package)
        {
            package.Extracted = false;
            package.PackagePath = $"{package.DisplayName}.zip";
            return await this.DownloadPackageAsync(package);
        }

        public async Task<AnnotationPackage> DownloadPackageAsync(AnnotationPackage package)
        {
            if (!Directory.Exists(this._extractionFolder))
            {
                Directory.CreateDirectory(this._extractionFolder);
            }

            var dir = new S3DirectoryInfo(this._client, this._bucketName);
            var file = dir.GetFile(package.PackagePath);

            var zipFilePath = Path.Combine(this._extractionFolder, file.Name);

            package.Downloading = true;

            var request = new GetObjectRequest
            {
                BucketName = this._bucketName,
                Key = file.Name
            };
            using (var response = this._client.GetObject(request))
            {
                this._currentlyDownloadedPackages.Add(package);
                response.WriteObjectProgressEvent += this.WriteObjectProgressEvent;

                await response.WriteResponseStreamToFileAsync(zipFilePath, false, new System.Threading.CancellationToken());

                this._currentlyDownloadedPackages.Remove(package);
                response.WriteObjectProgressEvent -= this.WriteObjectProgressEvent;
            }

            package.Downloading = false;
            package.PackagePath = zipFilePath;
            package.DisplayName = Path.GetFileNameWithoutExtension(zipFilePath);

            return await Task.FromResult(package);
        }

        private void WriteObjectProgressEvent(object sender, WriteObjectProgressArgs e)
        {
            var item = this._currentlyDownloadedPackages.FirstOrDefault(o => o.PackagePath == ((GetObjectResponse)sender).Key);
            if (item == null)
            {
                return;
            }

            item.TotalBytes = e.TotalBytes;
            item.TransferredBytes = e.TransferredBytes;
            item.DownloadProgress = e.PercentDone;
        }

        public async Task UploadPackageAsync(string packagePath)
        {
            var fileTransferUtility = new TransferUtility(this._client);
            var uploadRequest = new TransferUtilityUploadRequest
            {
                FilePath = packagePath,
                BucketName = this._bucketName
            };
            uploadRequest.UploadProgressEvent += this.UploadProgress;
            await fileTransferUtility.UploadAsync(uploadRequest);
        }

        private void UploadProgress(object sender, UploadProgressArgs e)
        {
            //TODO: Show upload progress
        }

        public async Task SyncPackagesAsync(AnnotationPackage[] packages)
        {
            this.IsSyncing = true;

            this._packagesToSync = packages.Length;
            this._syncedPackages = 0;

            var tasks = new List<Task>();
            foreach (var package in packages)
            {
                tasks.Add(Task.Run(() => this.SyncPackageAsync(package)));
            }

            await Task.WhenAll(tasks);

            this.IsSyncing = false;
        }

        private async Task SyncPackageAsync(AnnotationPackage package)
        {
            var context = new DynamoDBContext(this._dynamoDbClient);
            await context.SaveAsync(package.Info);

            this._syncedPackages++;
        }

        public double GetSyncProgress()
        {
            if (!this.IsSyncing)
            {
                return 0;
            }

            return this._syncedPackages / (double)this._packagesToSync * 100;
        }
    }
}
