using Alturos.Yolo.Model;
using Alturos.Yolo.TestUI.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
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
            this.menuStrip1.Visible = false;

            this.toolStripStatusLabelYoloInfo.Text = string.Empty;

            this.Text = $"Alturos Yolo TestUI {Application.ProductVersion}";
            this.dataGridViewFiles.AutoGenerateColumns = false;
            this.dataGridViewResult.AutoGenerateColumns = false;

            var imageInfos = new DirectoryImageReader().Analyze(@".\Images");
            this.dataGridViewFiles.DataSource = imageInfos.ToList();

            Task.Run(() => this.Initialize("."));
            this.LoadAvailableConfigurations();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._yoloWrapper?.Dispose();
        }

        private void LoadAvailableConfigurations()
        {
            var configPath = "config";

            if (!Directory.Exists(configPath))
            {
                return;
            }

            var configs = Directory.GetDirectories(configPath);
            if (configs.Length == 0)
            {
                return;
            }

            this.menuStrip1.Visible = true;

            foreach (var config in configs)
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = config;
                menuItem.Click += (object sender, EventArgs e) => { this.Initialize(config); };
                this.configurationToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        private ImageInfo GetCurrentImage()
        {
            var item = this.dataGridViewFiles.CurrentRow?.DataBoundItem as ImageInfo;
            return item;
        }

        private void dataGridViewFiles_SelectionChanged(object sender, EventArgs e)
        {
            var oldImage = this.pictureBox1.Image;
            var imageInfo = this.GetCurrentImage();           
            this.pictureBox1.Image = Image.FromFile(imageInfo.Path);            
            oldImage?.Dispose();

            this.dataGridViewResult.DataSource = null;
            this.groupBoxResult.Text = $"Result";
        }

        private void dataGridViewFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                this.Detect();
            }
        }

        private void dataGridViewResult_SelectionChanged(object sender, EventArgs e)
        {
            if (!this.dataGridViewResult.Focused)
            {
                return;
            }

            var items = this.dataGridViewResult.DataSource as List<YoloItem>;
            var selectedItem = this.dataGridViewResult.CurrentRow?.DataBoundItem as YoloItem;
            this.DrawBorder2Image(items, selectedItem);
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogResult = this.folderBrowserDialog1.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            var imageInfos = new DirectoryImageReader().Analyze(this.folderBrowserDialog1.SelectedPath);
            this.dataGridViewFiles.DataSource = imageInfos.ToList();
        }

        private void buttonSendImage_Click(object sender, EventArgs e)
        {
            this.Detect();
        }

        private void DrawBorder2Image(List<YoloItem> items, YoloItem selectedItem = null)
        {
            var imageInfo = this.GetCurrentImage();
            //Load the image(probably from your stream)
            var image = Image.FromFile(imageInfo.Path);

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

                    using (var overlayBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 102)))
                    using (var pen = this.GetBrush(item.Confidence, image.Width))
                    {
                        if (item.Equals(selectedItem))
                        {
                            canvas.FillRectangle(overlayBrush, x, y, width, height);
                        }

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

            if (confidence > 0.5)
            {
                return new Pen(Brushes.GreenYellow, size);
            }
            else if (confidence > 0.2 && confidence <= 0.5)
            {
                return new Pen(Brushes.Orange, size);
            }

            return new Pen(Brushes.DarkRed, size);
        }

        private void Initialize(string path)
        {
            var configurationDetector = new ConfigurationDetector();
            var config = configurationDetector.Detect(path);

            if (config == null)
            {
                return;
            }

            this.Initialize(config);
        }

        private void Initialize(YoloConfiguration config)
        {
            try
            {
                if (this._yoloWrapper != null)
                {
                    this._yoloWrapper.Dispose();
                }

                var useOnlyCpu = this.cpuToolStripMenuItem.Checked;

                var sw = new Stopwatch();
                sw.Start();
                this._yoloWrapper = new YoloWrapper(config.ConfigFile, config.WeightsFile, config.NamesFile, 0, useOnlyCpu);
                sw.Stop();

                var action = new MethodInvoker(delegate ()
                {
                    var detectionSystemDetail = string.Empty;
                    if (!string.IsNullOrEmpty(this._yoloWrapper.EnvironmentReport.GraphicDeviceName))
                    {
                        detectionSystemDetail = $"({this._yoloWrapper.EnvironmentReport.GraphicDeviceName})";
                    }
                    this.toolStripStatusLabelYoloInfo.Text = $"Initialize Yolo in {sw.Elapsed.TotalMilliseconds:0} ms - Detection System:{this._yoloWrapper.DetectionSystem} {detectionSystemDetail} Weights:{config.WeightsFile}";
                });

                this.statusStrip1.Invoke(action);
                this.buttonSendImage.Invoke(new MethodInvoker(delegate () { this.buttonSendImage.Enabled = true; }));
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{nameof(Initialize)} - {exception}", "Error Initialize", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        

        private void Detect()
        {
            if (this._yoloWrapper == null)
            {
                return;
            }

            var memoryTransfer = true;

            var imageInfo = this.GetCurrentImage();
            var imageData = File.ReadAllBytes(imageInfo.Path);

            var sw = new Stopwatch();
            sw.Start();
            List<YoloItem> items;
            if (memoryTransfer)
            {
                items = this._yoloWrapper.Detect(imageData).ToList();
            }
            else
            {
                items = this._yoloWrapper.Detect(imageInfo.Path).ToList();
            }
            sw.Stop();
            this.groupBoxResult.Text = $"Result [ processed in {sw.Elapsed.TotalMilliseconds:0} ms ]";

            this.dataGridViewResult.DataSource = items;
            this.DrawBorder2Image(items);
        }

        private void gpuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.cpuToolStripMenuItem.Checked = !this.cpuToolStripMenuItem.Checked;
        }
    }
}
