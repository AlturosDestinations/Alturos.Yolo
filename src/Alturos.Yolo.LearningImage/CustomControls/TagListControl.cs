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

            this.dataGridView1.DataSource = info.Tags?.Select(o => new { Value = o }).ToList();
        }
    }
}
