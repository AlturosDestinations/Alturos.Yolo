using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class ConfigurationForm : Form
    {
        private IAnnotationPackageProvider _provider;
        private AnnotationConfig _config;
        private BindingSource _bindingSourceObjectClasses;
        private BindingSource _bindingSourceTags;

        public ConfigurationForm()
        {
            this.InitializeComponent();

            this.dataGridViewObjectClasses.AutoGenerateColumns = false;
            this.dataGridViewTags.AutoGenerateColumns = false;
        }

        public void Setup(IAnnotationPackageProvider provider, AnnotationConfig config)
        {
            this._provider = provider;
            this._config = config;

            this._bindingSourceObjectClasses = new BindingSource();
            this._bindingSourceObjectClasses.DataSource = config.ObjectClasses;
            this.dataGridViewObjectClasses.DataSource = this._bindingSourceObjectClasses;

            this._bindingSourceTags = new BindingSource();
            this._bindingSourceTags.DataSource = config.Tags;
            this.dataGridViewTags.DataSource = this._bindingSourceTags;
        }

        private void ButtonAddObjectClass_Click(object sender, EventArgs e)
        {
            var text = this.textBoxObjectClass.Text;
            if (!string.IsNullOrEmpty(text) && !this._config.ObjectClasses.Any(o => o.Name == text))
            {
                var objectClass = new ObjectClass()
                {
                    Id = this._config.ObjectClasses.Count,
                    Name = text
                };

                this._config.ObjectClasses.Add(objectClass);
            }

            this._bindingSourceObjectClasses.ResetBindings(false);
            this.dataGridViewObjectClasses.Refresh();

            this.textBoxObjectClass.Text = string.Empty;
            this.textBoxObjectClass.Focus();
        }

        private void ButtonAddTag_Click(object sender, EventArgs e)
        {
            var text = this.textBoxTag.Text;
            if (!string.IsNullOrEmpty(text) && !this._config.Tags.Any(o => o.Value == text))
            {
                var tag = new Tag(text);
                this._config.Tags.Add(tag);
            }

            this._bindingSourceTags.ResetBindings(false);
            this.dataGridViewTags.Refresh();

            this.textBoxTag.Text = string.Empty;
            this.textBoxTag.Focus();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            this._provider.SetAnnotationConfig(this._config);

            this.Close();
        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = this.dataGridViewTags.Rows[this.dataGridViewTags.CurrentCell.RowIndex];
            var tag = row.DataBoundItem as Tag;

            this._config.Tags.Remove(tag);

            this._bindingSourceTags.ResetBindings(false);
            this.dataGridViewTags.Refresh();
        }
    }
}
