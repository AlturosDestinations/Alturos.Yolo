using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class TagListControl : UserControl
    {
        public event Func<List<string>> TagsRequested;

        private AnnotationPackage _package;

        public TagListControl()
        {
            this.InitializeComponent();
        }

        public void SetTags(AnnotationPackage package)
        {
            if (package == null)
            {
                return;
            }

            this._package = package;

            this.RefreshDatagrid();
        }

        private void ButtonAdd_Click(object sender, System.EventArgs e)
        {
            var results = this.TagsRequested?.Invoke();

            if (results.Count > 0)
            {
                if (this._package.Tags == null)
                {
                    this._package.Tags = new List<string>();
                }

                foreach (var tag in results)
                {
                    if (!this._package.Tags.Contains(tag))
                    {
                        this._package.Tags.Add(tag);
                        this._package.IsDirty = true;
                    }
                }
            }

            this.RefreshDatagrid();
        }

        private void RemoveTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = this.dataGridView1.Rows[this.dataGridView1.CurrentCell.RowIndex];
            var tag = row.DataBoundItem as AnnotationPackageTag;

            this._package.Tags?.Remove(tag.Value);

            this.RefreshDatagrid();
        }

        private void RefreshDatagrid()
        {
            var items = this._package.Tags?.Select(o => new AnnotationPackageTag { Value = o });
            this.dataGridView1.DataSource = items?.ToList();
        }
    }
}
