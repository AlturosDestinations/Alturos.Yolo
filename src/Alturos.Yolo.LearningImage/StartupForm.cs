using Alturos.Yolo.LearningImage.Contract;
using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    public partial class StartupForm : Form
    {
        public IAnnotationPackageProvider AnnotationPackageProvider { get; private set; }
        public List<ObjectClass> ObjectClasses { get; private set; }

        public StartupForm()
        {
            this.InitializeComponent();

            var packageProviders = new List<IAnnotationPackageProvider>()
            {
                new AmazonPackageProvider(),
                new WindowsFileSystemPackageProvider()
            };

            this.comboBoxAnnotationPackageProvider.DataSource = packageProviders;

            this.checkBoxObject1.Checked = true;
            this.checkBoxObject2.Checked = true;
        }

        private void checkBoxObject_CheckedChanged(object sender, System.EventArgs e)
        {
            var checkedBoxCount = this.groupBoxObjectClasses.Controls.OfType<CheckBox>().Count(o => o.Checked);
            this.buttonConfirm.Enabled = checkedBoxCount > 0;
        }

        private void comboBoxAnnotationPackageProvider_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.AnnotationPackageProvider = this.comboBoxAnnotationPackageProvider.SelectedItem as IAnnotationPackageProvider;
        }

        private void buttonConfirm_Click(object sender, System.EventArgs e)
        {
            this.ObjectClasses = new List<ObjectClass>();

            var checkBoxCount = this.groupBoxObjectClasses.Controls.OfType<CheckBox>().Count();
            for (var i = 0; i < checkBoxCount; i++)
            {
                var fieldName = $"checkBoxObject{(i + 1).ToString()}";
                var fieldInfo = this.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                var checkBox = fieldInfo.GetValue(this) as CheckBox;

                if (checkBox != null && checkBox.Checked)
                {
                    this.ObjectClasses.Add(new ObjectClass
                    {
                        Id = i,
                        Name = checkBox.Text,
                        Selected = true
                    });
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
