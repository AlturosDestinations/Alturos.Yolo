using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.Forms
{
    public partial class TagSelectionForm : Form
    {
        public List<string> SelectedTags;
        private AnnotationConfig _config;

        public TagSelectionForm()
        {
            this.InitializeComponent();
            this.SelectedTags = new List<string>();
        }

        public void Setup(AnnotationConfig config)
        {
            this._config = config;
            this.dataGridViewTags.DataSource = this._config.Tags;
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            this.SelectedTags.Clear();

            var rowsSelected = this.dataGridViewTags.SelectedRows;
            foreach (DataGridViewRow row in rowsSelected)
            {
                var tag = row.DataBoundItem as AnnotationPackageTag;
                this.SelectedTags.Add(tag.Value);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            var filter = this.textBoxFilter.Text;
            if (!string.IsNullOrEmpty(filter))
            {
                this.dataGridViewTags.DataSource = this._config.Tags.Where(o => o.Value.Contains(filter)).ToList();
            }
            else
            {
                this.dataGridViewTags.DataSource = this._config.Tags;
            }
        }
    }
}
