using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class ExportDialog : Form
    {
        private readonly IAnnotationPackageProvider _annotationPackageProvider;
        private AnnotationConfig _config;

        public ExportDialog(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;
            this.InitializeComponent();

            this._config = this._annotationPackageProvider.GetAnnotationConfigAsync().GetAwaiter().GetResult();
            this.dataGridViewTags.DataSource = this._config.Tags;

            this.dataGridViewTags.AutoGenerateColumns = false;
            this.dataGridViewResult.AutoGenerateColumns = false;
        }

        private async void ButtonSearch_Click(object sender, EventArgs e)
        {
            var tags = this.dataGridViewTags.SelectedRows.Cast<DataGridViewRow>().Select(o => o.DataBoundItem as AnnotationPackageTag);

            var items = await this._annotationPackageProvider.GetPackagesAsync(tags.ToArray());
            this.dataGridViewResult.DataSource = items;
            this.labelPackageCount.Text = items.Length.ToString();
        }

        private void ButtonExport_Click(object sender, EventArgs e)
        {
            this.Export();
            this.Close();
        }

        private void Export()
        {
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
            this.CreateFiles(dataPath, imagePath);

            // Open folder
            Process.Start(dataPath);
        }

        /// <summary>
        /// Creates the images, the annotation info and a list for every object listing each image that features it
        /// </summary>
        private void CreateFiles(string dataPath, string imagePath)
        {
            var stringBuilderDict = new Dictionary<int, StringBuilder>();
            foreach (var objectClass in this._config.ObjectClasses)
            {
                stringBuilderDict[objectClass.Id] = new StringBuilder();
            }

            var usedFileNames = new List<string>();
            var packages = this.dataGridViewResult.DataSource as List<AnnotationPackage>;
            var images = packages.SelectMany(o => o.GetImages());

            foreach (var image in images)
            {
                var boxes = image.BoundingBoxes;
                boxes.RemoveAll(box => !this._config.ObjectClasses.Select(objectClass => objectClass.Id).Contains(box.ObjectIndex));

                if (boxes.Count == 0)
                {
                    continue;
                }

                var newFileName = Path.GetFileName(image.FilePath);
                while (usedFileNames.Contains(newFileName))
                {
                    newFileName = Path.GetFileNameWithoutExtension(image.FilePath) + "(1)" + Path.GetExtension(image.FilePath);
                }

                usedFileNames.Add(newFileName);

                var newFilePath = Path.Combine(imagePath, newFileName);

                for (var i = 0; i < boxes.Count; i++)
                {
                    if (boxes[i] != null)
                    {
                        stringBuilderDict[boxes[i].ObjectIndex].AppendLine(Path.GetFullPath(newFilePath));
                    }
                }

                // Copy image
                File.Copy(image.FilePath, newFilePath, true);

                //Create bounding boxes
                this.CreateBoundingBoxes(image.BoundingBoxes, Path.ChangeExtension(newFilePath, "txt"));
            }

            // Write object lists to file
            foreach (var objectClass in this._config.ObjectClasses)
            {
                File.WriteAllText(Path.Combine(dataPath, $"{objectClass.Name}.txt"), stringBuilderDict[objectClass.Id].ToString());
            }
        }

        /// <summary>
        /// Writes the bounding boxes to a file
        /// </summary>
        private void CreateBoundingBoxes(List<AnnotationBoundingBox> boundingBoxes, string filePath)
        {
            var sb = new StringBuilder();
            foreach (var box in boundingBoxes)
            {
                sb.Append(box.ObjectIndex).Append(" ");
                sb.Append(box.CenterX).Append(" ");
                sb.Append(box.CenterY).Append(" ");
                sb.Append(box.Width).Append(" ");
                sb.Append(box.Height).Append(" ");
                sb.AppendLine();
            }

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
