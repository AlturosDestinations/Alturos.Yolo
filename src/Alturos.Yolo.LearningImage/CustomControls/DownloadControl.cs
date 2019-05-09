using Alturos.Yolo.LearningImage.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class DownloadControl : UserControl
    {
        public Func<AnnotationPackage, Task> ExtractionRequested { get; set; }

        private AnnotationPackage _packageToExtract;

        public DownloadControl()
        {
            this.InitializeComponent();
        }

        public void ShowDownloadDialog(AnnotationPackage package)
        {
            this.buttonDownload.Visible = !package.Downloading;
            this.progressBarDownload.Visible = package.Downloading;

            this._packageToExtract = package;
            this.Show();

            Task.Run(() => ShowDownloadProgress(package));
        }

        private async Task ShowDownloadProgress(AnnotationPackage package)
        {
            while (this.progressBarDownload.Value < 100 && package.Downloading && _packageToExtract == package)
            {
                await Task.Delay(20);
                this.progressBarDownload.Invoke((MethodInvoker)delegate { this.progressBarDownload.Value = (int)package.DownloadProgress; });
            }
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            this.buttonDownload.Visible = false;
            this.progressBarDownload.Visible = true;

            await this.ExtractionRequested?.Invoke(this._packageToExtract);
            this.Hide();
        }
    }
}
