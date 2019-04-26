using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationImageList : UserControl
    {
        private AnnotationPackage _packageToExtract;

        public Action<AnnotationImage> ImageSelected { get; set; }
        public Action<AnnotationPackage> ExtractionRequested { get; set; }

        public AnnotationImageList()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.panelExtractNotification.Hide();
        }

        public AnnotationImage[] GetAll()
        {
            var items = new List<AnnotationImage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var item = row.DataBoundItem as AnnotationImage;
                items.Add(item);
            }

            return items.ToArray();
        }

        public AnnotationImage[] GetSelected()
        {
            var items = new List<AnnotationImage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var item = row.DataBoundItem as AnnotationImage;
                if (item.Selected)
                {
                    items.Add(item);
                }
            }

            return items.ToArray();
        }

        public void SetImages(List<AnnotationImage> images)
        {
            this.panelExtractNotification.Hide();
            this.dataGridView1.DataSource = images;
        }

        public void ShowExtractionWarning(AnnotationPackage package)
        {
            this._packageToExtract = package;
            this.panelExtractNotification.Show();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var image = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationImage;
            this.ImageSelected?.Invoke(image);
        }

        private void buttonExtract_Click(object sender, EventArgs e)
        {
            ExtractionRequested?.Invoke(this._packageToExtract);
            this.panelExtractNotification.Hide();
        }
    }
}
