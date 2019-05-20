using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using Mapster;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class SyncForm : Form
    {
        private IAnnotationPackageProvider _annotationPackageProvider;
        private bool _syncing;

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

            this._syncing = true;

            _ = Task.Run(() => this.UpdateProgressBar());
            await this._annotationPackageProvider.SyncPackagesAsync(packages);

            this._syncing = false;

            this.Invoke((MethodInvoker)delegate { this.Close(); });
        }

        private async Task UpdateProgressBar()
        {
            while (this._syncing)
            {
                var progress = this._annotationPackageProvider.GetSyncProgress();
                this.progressBar.Invoke((MethodInvoker)delegate { this.progressBar.Value = (int)progress; });

                await Task.Delay(100);
            }
        }

        private void AddImageDtos(AnnotationPackage package)
        {
            var info = package.Info;
            info.ImageDtos = new List<AnnotationImageDto>();

            foreach (var image in package.Images)
            {
                info.ImageDtos.Add(image.Adapt<AnnotationImageDto>());
            }
        }
    }
}
