using Alturos.Yolo.LearningImage.Helper;
using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class AnnotationImageControl : UserControl
    {
        public AnnotationImageControl()
        {
            this.InitializeComponent();
        }

        public void SetImage(AnnotationImage image, List<ObjectClass> objectClasses)
        {
            var oldImage = this.pictureBox1.Image;

            if (image == null)
            {
                this.pictureBox1.Image = null;
            }
            else
            {
                this.pictureBox1.Image = DrawHelper.DrawBoxes(image, objectClasses);
            }

            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }
    }
}
