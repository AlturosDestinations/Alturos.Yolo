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
        public ExportDialog()
        {
            this.InitializeComponent();
        }

        public void CreateImages(List<AnnotationImage> images)
        {
            var newImages = new List<AnnotationImage>();
            foreach (var image in images)
            {
                newImages.Add(new AnnotationImage(image));
            }

            this.annotationImageListControl.SetImages(newImages);
        }

        public void SetObjectClasses(List<ObjectClass> objectClasses)
        {
            this.objectClassListControl.SetObjectClasses(objectClasses);
        }

        private void buttonExport_Click(object sender, EventArgs e)
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

            // Create meta data
            this.CreateMetaData(dataPath);

            // Open folder
            Process.Start(dataPath);
        }

        /// <summary>
        /// Creates the images, the annotation info and a list for every object listing each image that features it
        /// </summary>
        private void CreateFiles(string dataPath, string imagePath)
        {
            var images = this.annotationImageListControl.GetSelected();
            var objectClasses = this.objectClassListControl.GetSelected();

            var stringBuilderDict = new Dictionary<int, StringBuilder>();
            foreach (var objectClass in objectClasses)
            {
                stringBuilderDict[objectClass.Id] = new StringBuilder();
            }

            var usedFileNames = new List<string>();

            foreach (var image in images)
            {
                var boxes = image.BoundingBoxes;
                boxes.RemoveAll(box => !objectClasses.Select(objectClass => objectClass.Id).Contains(box.ObjectIndex));

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
            foreach (var objectClass in objectClasses)
            {
                File.WriteAllText(Path.Combine(dataPath, $"{objectClass.Name}.txt"), stringBuilderDict[objectClass.Id].ToString());
            }
        }

        /// <summary>
        /// Creates the obj.names and obj.data files
        /// </summary>
        private void CreateMetaData(string dataPath)
        {
            var objectNames = this.objectClassListControl.GetSelected().Select(o => o.Name).ToArray();

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
