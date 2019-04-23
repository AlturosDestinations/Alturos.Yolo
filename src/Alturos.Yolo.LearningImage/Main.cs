using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class Main : Form
    {
        public Main()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFolderDialog = new CommonOpenFileDialog())
            {
                openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                openFolderDialog.IsFolderPicker = true;
                if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var imagePath = openFolderDialog.FileName;

                    var files = Directory.GetFiles(imagePath, "*.*", SearchOption.TopDirectoryOnly);
                    var items = files.Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).Select(o => new AnnotationImage { FilePath = o, FileName = new FileInfo(o).Name }).ToList();

                    this.dataGridView1.DataSource = items;
                }
            }
        }

        private void allImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = new List<AnnotationImage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var item = row.DataBoundItem as AnnotationImage;
                items.Add(item);
            }

            this.Export(items);
        }

        private void selectedImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = new List<AnnotationImage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var item = row.DataBoundItem as AnnotationImage;
                if (item.Selected) { items.Add(item); }
            }

            this.Export(items);
        }

        private void Export(List<AnnotationImage> images)
        {
            var exportDialog = new ExportDialog();

            int boxCount = 0;
            foreach (var image in images)
            {
                boxCount = Math.Max(boxCount, this.GetBoxes(this.GetDataPath(image.FilePath)).Length);
            }
            exportDialog.CreateObjectClasses(boxCount);

            var dialogResult = exportDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            // Create folders
            var dataPath = "data";
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            var imagePath = Path.Combine(dataPath, "img");
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            // Create list of objects
            var sb = new StringBuilder();
            foreach (var image in images)
            {
                var boxes = this.GetBoxes(this.GetDataPath(image.FilePath));
                if (boxes.Length == 0)
                {
                    continue;
                }

                sb.AppendLine(image.FilePath);
            }
            File.WriteAllText(Path.Combine(dataPath, "train.txt"), sb.ToString());

            // Copy image and info files
            foreach (var image in images)
            {
                File.Copy(image.FilePath, Path.Combine(imagePath, image.FileName), true);
                File.Copy(this.GetDataPath(image.FilePath), Path.Combine(imagePath, this.GetDataPath(image.FileName)), true);
            }

            // Open folder
            Process.Start(dataPath);
        }

        private string[] GetColorCodes()
        {
            return new string[] { "#E3330E", "#48E10F", "#D40FE1", "#24ECE3", "#EC2470" };
        }

        private AnnotationInfo[] GetBoxes(string dataPath)
        {
            if (!File.Exists(dataPath))
            {
                return new AnnotationInfo[0];
            }

            var data = File.ReadAllText(dataPath);
            var lines = data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var items = new List<AnnotationInfo>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');

                var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                int.TryParse(parts[0], out int index);
                float.TryParse(parts[1], NumberStyles.Any, ci, out var x);
                float.TryParse(parts[2], NumberStyles.Any, ci, out var y);
                float.TryParse(parts[3], NumberStyles.Any, ci, out var width);
                float.TryParse(parts[4], NumberStyles.Any, ci, out var heigth);

                items.Add(new AnnotationInfo { ObjectIndex = index, CenterX = x, CenterY = y, Width = width, Height = heigth });
            }

            return items.ToArray();
        }

        private string GetDataPath(string imagePath)
        {
            var fileInfo = new FileInfo(imagePath);
            return Path.Combine(fileInfo.DirectoryName, $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}.txt");
        }

        private Image DrawBoxes(string imagePath)
        {
            var dataPath = this.GetDataPath(imagePath);
            if (!File.Exists(dataPath))
            {
                return null;
            }

            var colorCodes = this.GetColorCodes();

            var items = this.GetBoxes(dataPath);

            var image = new Bitmap(imagePath);
            using (var canvas = Graphics.FromImage(image))
            {
                foreach (var item in items)
                {
                    var width = item.Width * image.Width;
                    var heigth = item.Height * image.Height;
                    var x = (item.CenterX * image.Width) - (width / 2);
                    var y = (item.CenterY * image.Height) - (heigth / 2);

                    var color = ColorTranslator.FromHtml(colorCodes[item.ObjectIndex]);
                    var pen = new Pen(color, 3);

                    canvas.DrawRectangle(pen, x, y, width, heigth);
                }

                canvas.Flush();
            }

            return image;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var image = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationImage;
            var oldImage = this.pictureBox1.Image;
            this.pictureBox1.Image = this.DrawBoxes(image.FilePath);
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }
    }
}
