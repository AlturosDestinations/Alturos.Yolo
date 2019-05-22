using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class TagListControl : UserControl
    {
        private AnnotationPackage _package;

        public TagListControl()
        {
            this.InitializeComponent();
        }

        public void SetTags(AnnotationPackage package)
        {
            if (package == null || package.Info == null)
            {
                return;
            }

            this._package = package;

            var items = package.Info.Tags;
            if (items == null)
            {
                items = new List<string>();
            }

            var bindingSource = new BindingSource(items, null);

            this.dataGridView1.DataSource = bindingSource;
        }

        private void DataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            var list = new List<string>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var value = row.Cells[0].Value?.ToString();
                if (!string.IsNullOrEmpty(value)) { list.Add(value); }
            }

            //TODO: Verify tags exist

            this._package.IsDirty = true;
            this._package.Info.Tags = list;
        }
    }
}
