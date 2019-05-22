using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
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

            var items = package.Info.Tags?.Select(o => new AnnotationPackageTag { Value = o });
            if (items == null)
            {
                items = new List<AnnotationPackageTag>();
            }

            var bindingSource = new BindingSource();
            bindingSource.DataSource = items;

            this.dataGridView1.DataSource = bindingSource;
        }
    }
}
