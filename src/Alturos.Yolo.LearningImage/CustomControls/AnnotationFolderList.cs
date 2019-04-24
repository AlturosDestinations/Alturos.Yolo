using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationFolderList : UserControl
    {
        public Action<AnnotationFolder> FolderSelected { get; set; }

        public AnnotationFolderList()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }

        public AnnotationImage[] GetAll()
        {
            var items = new List<AnnotationImage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var folder = row.DataBoundItem as AnnotationFolder;
                items.AddRange(folder.Images);
            }

            return items.ToArray();
        }

        public AnnotationImage[] GetSelected()
        {
            var items = new List<AnnotationImage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var folder = row.DataBoundItem as AnnotationFolder;
                if (folder.Selected) {
                    items.AddRange(folder.Images.Where(o => o.Selected));
                }
            }

            return items.ToArray();
        }

        public void LoadFolders()
        {
            using (var openFolderDialog = new CommonOpenFileDialog())
            {
                openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                openFolderDialog.IsFolderPicker = true;
                if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var path = openFolderDialog.FileName;
                    var directories = Directory.GetDirectories(path).ToList();
                    directories.Add(path);

                    var rootPath = Directory.GetParent(path).FullName;
                    var folders = this.CreateFolders(rootPath, directories.ToArray());

                    this.dataGridView1.DataSource = folders;
                }
            }
        }

        private List<AnnotationFolder> CreateFolders(string rootPath, string[] paths)
        {
            var annotationFolders = new List<AnnotationFolder>();

            foreach (var path in paths)
            {
                var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                var items = files.Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).Select(o => new AnnotationImage { FilePath = o, FileName = new FileInfo(o).Name }).ToList();
                
                if (items.Count == 0)
                {
                    continue;
                }

                var annotationFolder = new AnnotationFolder
                {
                    DirectoryPath = path
                };

                var uri1 = new Uri(rootPath);
                var uri2 = new Uri(path);

                annotationFolder.DirectoryName = uri1.MakeRelativeUri(uri2).ToString();
                annotationFolder.Images = items;

                annotationFolders.Add(annotationFolder);
            }

            return annotationFolders;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var folder = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationFolder;
            this.FolderSelected?.Invoke(folder);
        }
    }
}
