using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
            this.dataGridViewFiles.DataSource = files.Select(o => new { Name = o }).ToList();

            var configurationDetector = new ConfigurationDetector();
            var config = configurationDetector.Detect();

            if (config == null)
            {
                MessageBox.Show($"Yolo configuration detection failure", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._yoloWrapper = new YoloWrapper();
            this.Initialize(config);
        }

        private string GetCurrentImage()
        {
            var fileName = ((dynamic)this.dataGridViewFiles.CurrentRow.DataBoundItem).Name;
            return fileName;
        }

        private void dataGridViewFiles_SelectionChanged(object sender, EventArgs e)
        {
            var fileName = this.GetCurrentImage();
            this.pictureBox1.Image = Image.FromFile(fileName);
        }

        private void buttonSendImage_Click(object sender, EventArgs e)
        {
            try
            {
                var fileName = this.GetCurrentImage();
                var imageData = File.ReadAllBytes(fileName);

                var sw = new Stopwatch();
                sw.Start();
                var data = this._yoloWrapper.ProcessImage(imageData);
                sw.Stop();

                this.toolStripStatusLabel1.Text = $"processing in {sw.Elapsed.TotalMilliseconds}ms";

                var items = data.Select(o => new YoloItem
                {
                    Type = o.objectType,
                    Confidence = o.confidence,
                    X = o.rectangle.topLeftCorner.x,
                    Y = o.rectangle.topLeftCorner.y,
                    Width = o.rectangle.width,
                    Height = o.rectangle.height
                }).ToList();

                this.dataGridView1.DataSource = items;
                this.DrawImage(items);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Process image exception:{exception}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                    var pen = new Pen(Brushes.GreenYellow, 2);

                    if (item.Confidence > 50)
                    {
                        pen = new Pen(Brushes.DarkRed, 4);
                    }
                    if (item.Confidence > 20 && item.Confidence <= 50)
                    {
                        pen = new Pen(Brushes.Orange, 3);
                    }

                    canvas.DrawRectangle(pen, x, y, width, height);
                    canvas.Flush();
                }
            }

            this.pictureBox1.Image = image;
        }

        private void Initialize(YoloConfiguration yoloConfiguration)
        {
            var sw = new Stopwatch();
            sw.Start();
            var successful = this._yoloWrapper.Initialize(yoloConfiguration);
            sw.Stop();

            if (successful)
            {
                this.toolStripStatusLabel1.Text = $"Initialize elapsed in {sw.Elapsed.TotalMilliseconds}ms";
            }
            else
            {
                this.toolStripStatusLabel1.Text = $"Initialize failure";
            }

            this.buttonSendImage.Enabled = true;
        }
    }
}
