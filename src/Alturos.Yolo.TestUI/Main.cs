using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.Yolo.TestUI
{
    public partial class Main : Form
    {
        private YoloWrapper _yoloWrapper;

        public Main()
        {
            this.InitializeComponent();
            this.buttonSendImage.Enabled = false;
            this.Text = $"Alturos Yolo TestUI {Application.ProductVersion}";

            var files = Directory.GetFiles(@".\Images");
            //var files = Directory.GetFiles(@"C:\Users\tinoh\Desktop\Fotos iPhone");
            this.dataGridViewFiles.DataSource = files.Select(o => new { Name = o }).ToList();

            var configurationDetector = new ConfigurationDetector();
            var config = configurationDetector.Detect();
            if (config == null)
            {
                MessageBox.Show($"Yolo configuration detection failure", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Task.Run(() => this.Initialize(config));
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._yoloWrapper.Dispose();
        }

        private string GetCurrentImage()
        {
            var fileName = ((dynamic)this.dataGridViewFiles.CurrentRow.DataBoundItem).Name;
            return fileName;
        }

        private void dataGridViewFiles_SelectionChanged(object sender, EventArgs e)
        {
            var oldImage = this.pictureBox1.Image;
            var fileName = this.GetCurrentImage();
            if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            {
                this.pictureBox1.Image = Image.FromFile(fileName);
            }
            oldImage?.Dispose();
        }

        private void buttonSendImage_Click(object sender, EventArgs e)
        {
            this.Detect();
        }

        private void DrawImage(List<YoloItem> items)
        {
            var fileName = this.GetCurrentImage();
            //Load the image(probably from your stream)
            var image = Image.FromFile(fileName);

            using (var canvas = Graphics.FromImage(image))
            {
                // Modify the image using g here... 
                // Create a brush with an alpha value and use the g.FillRectangle function
                foreach (var item in items)
                {
                    var x = item.X;
                    var y = item.Y;
                    var width = item.Width;
                    var height = item.Height;

                    using (var pen = this.GetBrush(item.Confidence, image.Width))
                    {
                        canvas.DrawRectangle(pen, x, y, width, height);
                        canvas.Flush();
                    }
                }
            }

            var oldImage = this.pictureBox1.Image;
            this.pictureBox1.Image = image;
            oldImage?.Dispose();
        }

        private Pen GetBrush(double confidence, int width)
        {
            var size = width / 100;

            if (confidence > 50)
            {
                return new Pen(Brushes.DarkRed, size);
            }
            else if (confidence > 20 && confidence <= 50)
            {
                return new Pen(Brushes.Orange, size);
            }

            return new Pen(Brushes.GreenYellow, size);
        }

        private void Initialize(YoloConfiguration config)
        {
            var sw = new Stopwatch();
            sw.Start();
            this._yoloWrapper = new YoloWrapper(config.ConfigFile, config.WeightsFile, config.NamesFile, 0);
            sw.Stop();

            this.statusStrip1.Invoke(new MethodInvoker(delegate () { this.toolStripStatusLabel1.Text = $"Initialize elapsed in {sw.Elapsed.TotalMilliseconds}ms DetectionSystem:{this._yoloWrapper.DetectionSystem}"; }));
            this.buttonSendImage.Invoke(new MethodInvoker(delegate () { this.buttonSendImage.Enabled = true; }));
        }        

        private void Detect()
        {
            var memoryTransfer = true;

            var fileName = this.GetCurrentImage();
            var imageData = File.ReadAllBytes(fileName);

            var sw = new Stopwatch();
            sw.Start();
            List<YoloItem> items;
            if (memoryTransfer)
            {
                items = this._yoloWrapper.Detect(imageData).ToList();
            }
            else
            {
                items = this._yoloWrapper.Detect(fileName).ToList();
            }
            sw.Stop();
            this.toolStripStatusLabel1.Text = $"processing in {sw.Elapsed.TotalMilliseconds}ms";

            this.dataGridView1.DataSource = items;
            this.DrawImage(items);
        }
    }
}
