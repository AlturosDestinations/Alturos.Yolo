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

            this.Export(items.ToArray());
        }

        private void selectedImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = new List<AnnotationImage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var item = row.DataBoundItem as AnnotationImage;
                if (item.Selected) { items.Add(item); }
            }

            this.Export(items.ToArray());
        }

        private void Export(AnnotationImage[] images)
        {
            var exportDialog = new ExportDialog();

            int boxCount = 0;
            foreach (var image in images)
            {
                var boxes = this.GetBoxes(this.GetDataPath(image.FilePath));
                foreach (var box in boxes)
                {
                    boxCount = Math.Max(boxCount, box.ObjectIndex + 1);
                }
            }
            exportDialog.CreateObjectClasses(boxCount);

            var dialogResult = exportDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            // Create folders
            var rootPath = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            var dataPath = Path.Combine(rootPath, "data");
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            var imagePath = Path.Combine(dataPath, "img");
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            // Copy images and create file lists
            CreateFiles(dataPath, imagePath, images, exportDialog.ObjectClasses.ToArray());

            // Create meta data
            CreateMetaData(dataPath, exportDialog.ObjectClasses.Select(o => o.Name).ToArray());

            // Open folder
            Process.Start(dataPath);
        }

        /// <summary>
        /// Creates the images, the annotation info and a list for every object listing each image that features it
        /// </summary>
        private void CreateFiles(string dataPath, string imagePath, AnnotationImage[] images, ObjectClass[] objectClasses)
        {
            var stringBuilderDict = new Dictionary<int, StringBuilder>();
            foreach (var objectClass in objectClasses)
            {
                stringBuilderDict[objectClass.Id] = new StringBuilder();
            }

            foreach (var image in images)
            {
                var boxes = this.GetBoxes(this.GetDataPath(image.FilePath)).ToList();
                boxes.RemoveAll(box => !objectClasses.Select(objectClass => objectClass.Id).Contains(box.ObjectIndex));

                if (boxes.Count == 0)
                {
                    continue;
                }

                for (var i = 0; i < boxes.Count; i++) {
                    if (boxes[i] != null)
                    {
                        stringBuilderDict[boxes[i].ObjectIndex].AppendLine(image.FilePath);
                    }
                }

                // Copy files
                File.Copy(image.FilePath, Path.Combine(imagePath, image.FileName), true);
                File.Copy(this.GetDataPath(image.FilePath), Path.Combine(imagePath, this.GetDataPath(image.FileName)), true);
            }

            // Write object lists to file
            foreach (var objectClass in objectClasses)
            {
                File.WriteAllText(Path.Combine(dataPath, $"{objectClass.Name}.txt"), stringBuilderDict[objectClass.Id].ToString());
            }
        }

        /// <summary>
        /// Creates the obj.names and obj.data files
        /// </summary>
        private void CreateMetaData(string dataPath, string[] objectNames)
        {
            var namesFile = "obj.names";
            var dataFile = "obj.data";

            // Create obj.names
            var namesBuilder = new StringBuilder();
            foreach (var name in objectNames)
            {
                namesBuilder.AppendLine(name);
            }
            File.WriteAllText(Path.Combine(dataPath, $"{namesFile}"), namesBuilder.ToString());

            // Create obj.data
            var relativeFolder = new DirectoryInfo(dataPath).Name;

            var dataBuilder = new StringBuilder();
            dataBuilder.AppendLine($"classes = {objectNames.Length}");
            foreach (var name in objectNames)
            {
                dataBuilder.AppendLine($"{name} = {relativeFolder}/{name}.txt");
            }
            dataBuilder.AppendLine($"names = {relativeFolder}/{namesFile}");
            File.WriteAllText(Path.Combine(dataPath, $"{dataFile}"), dataBuilder.ToString());
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
            var dataPath = Path.ChangeExtension(imagePath, "txt");
            return dataPath;
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
