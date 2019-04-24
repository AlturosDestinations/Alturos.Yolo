using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationImageList : UserControl
    {
        public Action<AnnotationImage> ImageSelected { get; set; }

        public AnnotationImageList()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
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
            this.dataGridView1.DataSource = images;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var image = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationImage;
            this.ImageSelected?.Invoke(image);
        }
    }
}
