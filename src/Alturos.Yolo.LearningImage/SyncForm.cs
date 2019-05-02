using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class SyncForm : Form
    {
        private IAnnotationPackageProvider _annotationPackageProvider;

        public SyncForm(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;

            this.InitializeComponent();
        }

        public async Task Sync(AnnotationPackage[] packages)
        {
            var zippedPackages = new List<AnnotationPackage>();
            foreach (var package in packages)
            {
                package.Info.IsAnnotated = true;

                if (!package.Extracted)
                {
                    continue;
                }

                var zippedPackage = this.ZipPackage(package);
                zippedPackages.Add(zippedPackage);
            }

            if (zippedPackages.Count == 0)
            {
                this.Close();
                return;
            }

            _ = Task.Run(() => this._annotationPackageProvider.SyncPackages(zippedPackages.ToArray()));

            while (!this._annotationPackageProvider.IsSyncing)
            {
                await Task.Delay(100);
            }

            while (this._annotationPackageProvider.IsSyncing)
            {
                var progress = this._annotationPackageProvider.GetSyncProgress();
                this.progressBar.Invoke((MethodInvoker)delegate { this.progressBar.Value = (int)progress; });

                await Task.Delay(100);
            }

            this.Close();
        }

        public AnnotationPackage ZipPackage(AnnotationPackage package)
        {
            var zipFilePath = Path.Combine(package.PackagePath, @"..\", $"{package.DisplayName}.zip");

            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
            }

            ZipFile.CreateFromDirectory(package.PackagePath, zipFilePath);

            var zippedPackage = new AnnotationPackage(package)
            {
                PackagePath = zipFilePath,
                Extracted = false
            };

            return zippedPackage;
        }
    }
}
