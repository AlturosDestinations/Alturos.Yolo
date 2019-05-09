using Alturos.Yolo.LearningImage.Model;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Transfer;
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
        }

        public AnnotationPackage[] GetPackages()
        {
            // Retrieve unannotated metadata
            var context = new DynamoDBContext(this._dynamoDbClient);

            var packageInfos = context.Scan<AnnotationPackageInfo>(new ScanCondition("IsAnnotated", ScanOperator.Equal, false));
            
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

            FileInfo fileInfo = null;
            await Task.Run(() =>
            {
                fileInfo = file.CopyToLocal(zipFilePath, true);
            });

            package.Downloading = false;
            package.PackagePath = fileInfo.FullName;
            package.DisplayName = Path.GetFileNameWithoutExtension(fileInfo.FullName);

            return await Task.FromResult(package);
        }

        public async Task SyncPackagesAsync(AnnotationPackage[] packages)
        {
            this.IsSyncing = true;

            this._packagesToSync = packages.Length;
            this._syncedPackages = 0;

            var tasks = new List<Task>();
            foreach (var package in packages)
            {
                tasks.Add(Task.Run(() => this.UploadAsync(package)));
            }

            await Task.WhenAll(tasks);

            this.IsSyncing = false;
        }

        private async Task UploadAsync(AnnotationPackage package)
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
