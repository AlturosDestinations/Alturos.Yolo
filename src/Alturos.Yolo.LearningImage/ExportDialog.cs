using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class ExportDialog : Form
    {
        public List<ObjectClass> ObjectClasses { get; private set; }

        public ExportDialog()
        {
            this.InitializeComponent();
        }

        public void CreateObjectClasses(int count)
        {
            var items = new List<ObjectClass>();
            for (var i = 0; i < count; i++)
            {
                items.Add(new ObjectClass
                {
                    Selected = true,
                    Id = i,
                    Name = $"object{i}"
                });
            }

            this.dataGridViewObjects.DataSource = items;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            this.ObjectClasses = new List<ObjectClass>();
            foreach (DataGridViewRow row in this.dataGridViewObjects.Rows)
            {
                var objectClass = row.DataBoundItem as ObjectClass;
                if (objectClass.Selected)
                {
                    this.ObjectClasses.Add(objectClass);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
