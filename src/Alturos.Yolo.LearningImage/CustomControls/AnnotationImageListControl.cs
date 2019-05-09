using Alturos.Yolo.LearningImage.Helper;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationImageListControl : UserControl
    {
        public Action<AnnotationImage> ImageSelected { get; set; }

        public DataGridView DataGridView { get { return this.dataGridView1; } }
        public DownloadControl DownloadControl { get { return this.downloadControl; } }

        public AnnotationImageListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.downloadControl.Hide();
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

        public void SetImages(List<AnnotationImage> images)
        {
            this.downloadControl.Hide();
            this.dataGridView1.DataSource = images?.OrderBy(o => o.DisplayName.GetFirstNumber()).ToList();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var image = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationImage;
            this.ImageSelected?.Invoke(image);
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
