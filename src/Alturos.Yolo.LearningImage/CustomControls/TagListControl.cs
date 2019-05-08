using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
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

            dynamic tags = new ExpandoObject();
            tags.Weather = info.Weather;
            tags.Driver = info.Driver;
            tags.Device = info.Device;
            tags.Flag = info.Flag;

            var items = tags as IDictionary<string, object>;
            for (var i = 0; i < info.Color?.Count; i++)
            {
                items.Add($"Color {(i + 1).ToString()}", info.Color[i]);
            }

            this.dataGridView1.DataSource = items.Select(o => new { o.Key, o.Value }).ToList();
        }
    }
}
