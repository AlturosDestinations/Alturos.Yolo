using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
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
            foreach (var package in packages)
            {
                package.IsDirty = false;
                this.AddImageDtos(package);
            }

            _ = Task.Run(() => this._annotationPackageProvider.SyncPackagesAsync(packages));

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

        private void AddImageDtos(AnnotationPackage package)
        {
            var info = package.Info;
            info.ImageDtos = new List<AnnotationImageDto>();

            foreach (var image in package.Images)
            {
                info.ImageDtos.Add(new AnnotationImageDto
                {
                    FilePath = image.FilePath,
                    BoundingBoxes = image.BoundingBoxes
                });
            }
        }
    }
}
