using Alturos.Yolo.LearningImage.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class DownloadControl : UserControl
    {
        public event Func<AnnotationPackage, Task> ExtractionRequested;

        private AnnotationPackage _packageToExtract;

        public DownloadControl()
        {
            this.InitializeComponent();
            this.labelDownload.Text = "";
        }

        public void ShowDownloadDialog(AnnotationPackage package)
        {
            this.buttonDownload.Visible = !package.Downloading;
            this.progressBarDownload.Visible = package.Downloading;
            this.labelPercentage.Visible = package.Downloading;

            this._packageToExtract = package;

            this.labelPercentage.Text = "";
            this.labelDownload.Text = "";
            this.progressBarDownload.Value = (int)package.DownloadProgress;

            this.Show();

            Task.Run(() => this.ShowDownloadProgress(package));
        }

        private async Task ShowDownloadProgress(AnnotationPackage package)
        {
            while (package.DownloadProgress < 100 && package.Downloading && this._packageToExtract == package)
            {
                this.labelPercentage.Invoke((MethodInvoker)delegate
                {
                    this.labelPercentage.Text = $"{(int)package.DownloadProgress}%";
                });
                this.labelDownload.Invoke((MethodInvoker)delegate
                {
                    this.labelDownload.Text = $"{package.TransferredBytes / 1024.0 / 1024.0:0.00} MB of {package.TotalBytes / 1024.0 / 1024.0:0.00} MB";
                });
                this.progressBarDownload.Invoke((MethodInvoker)delegate { this.progressBarDownload.Value = (int)package.DownloadProgress; });
                await Task.Delay(200);
            }
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            this.buttonDownload.Visible = false;
            this.progressBarDownload.Visible = true;
            this.labelPercentage.Visible = true;

            this.labelPercentage.BringToFront();

            this._packageToExtract.Downloading = true;
            _ = Task.Run(() => this.ShowDownloadProgress(this._packageToExtract));

            await this.ExtractionRequested?.Invoke(this._packageToExtract);
        }
    }
}
