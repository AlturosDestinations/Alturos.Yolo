using Alturos.Yolo.LearningImage.Model;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.IO;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace Alturos.Yolo.LearningImage.Contract
{
    public class AmazonPackageInitializationService
    {
        private readonly string _bucketName;
        private readonly AmazonS3Client _client;
        private readonly AmazonDynamoDBClient _dynamoDbClient;

        public AmazonPackageInitializationService()
        {
            var accessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            var secretAccessKey = ConfigurationManager.AppSettings["secretAccessKey"];

            this._bucketName = ConfigurationManager.AppSettings["bucketName"];

            this._client = new AmazonS3Client(accessKeyId, secretAccessKey, RegionEndpoint.EUWest1);
            this._dynamoDbClient = new AmazonDynamoDBClient(accessKeyId, secretAccessKey, RegionEndpoint.EUWest1);
        }

        public void AddMissingInfos()
        {
            var dir = new S3DirectoryInfo(this._client, this._bucketName);
            var files = dir.GetFiles();

            var context = new DynamoDBContext(this._dynamoDbClient);
            var allPackages = context.Scan<AnnotationPackageInfo>(null).ToList();

            var allFileNames = files.Select(o => o.Name).ToList();
            var missingPackageNames = allFileNames.Where(o => !allPackages.Select(x => x.Id).Contains(o)).ToList();

            var missingPackages = missingPackageNames.Select(o => new AnnotationPackageInfo { Id = o, IsAnnotated = false }).ToList();
            var existingPackages = context.Scan<AnnotationPackageInfo>(new ScanCondition("IsAnnotated", ScanOperator.IsNull)).ToList();

            foreach (var existingPackage in existingPackages)
            {
                existingPackage.IsAnnotated = false;
            }

            var packagesToPatch = missingPackages.Union(existingPackages).ToList();

            var batchSize = 25;

            for (var i = 0; i < packagesToPatch.Count; i += batchSize)
            {
                var infoBatch = context.CreateBatchWrite<AnnotationPackageInfo>();

                var items = packagesToPatch.GetRange(i, Math.Min(packagesToPatch.Count - i, batchSize));

                infoBatch.AddPutItems(items);
                infoBatch.Execute();

                Thread.Sleep(1000);
            }
        }
    }
}