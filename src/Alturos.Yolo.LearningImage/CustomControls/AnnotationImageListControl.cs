using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationImageListControl : UserControl
    {
        public event Action<AnnotationImage> ImageSelected;

        public AnnotationImageListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }

        public AnnotationImage[] GetAll()
        {
            var items = this.dataGridView1.DataSource as List<AnnotationImage>;
            return items.ToArray();
        }

        public void Reset()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Refresh();
        }

        public void SetPackage(AnnotationPackage package)
        {
            this.dataGridView1.DataSource = package.Images;
            this.dataGridView1.Refresh();
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var image = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationImage;
            if (image == null)
            {
                return;
            }

            this.ImageSelected?.Invoke(image);
        }

        private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var item = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as AnnotationImage;

            if (item == null)
            {
                return;
            }

            if (item.BoundingBoxes != null)
            {
                this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.GreenYellow;
                return;
            }

            this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
        }
    }
}
