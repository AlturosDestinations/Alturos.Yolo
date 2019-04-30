using Alturos.Yolo.LearningImage.Model;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class TagListControl : UserControl
    {
        public TagListControl()
        {
            this.InitializeComponent();
        }

        public void SetTags(AnnotationPackageInfo info)
        {
            if (info == null)
            {
                return;
            }

            var properties = info.GetType().GetProperties();
            this.dataGridView1.DataSource = properties.Select(o => new { Key = o.Name, Value = o.GetValue(info) }).ToList();
        }
    }
}
