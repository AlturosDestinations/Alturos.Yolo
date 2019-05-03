using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationImageListControl : UserControl
    {
        public Action<AnnotationImage> ImageSelected { get; set; }
        public Action<AnnotationPackage> ExtractionRequested { get; set; }

        public DataGridView DataGridView { get { return this.dataGridView1; } }

        private AnnotationPackage _packageToExtract;

        public AnnotationImageListControl()
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

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var package = row.DataBoundItem as AnnotationImage;
                package.Selected = true;
            }

            this.dataGridView1.Refresh();
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var package = row.DataBoundItem as AnnotationImage;
                package.Selected = false;
            }

            this.dataGridView1.Refresh();
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var item = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as AnnotationImage;

            if (item.BoundingBoxes?.Count > 0)
            {
                this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.GreenYellow;
                return;
            }

            this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
        }
    }
}
