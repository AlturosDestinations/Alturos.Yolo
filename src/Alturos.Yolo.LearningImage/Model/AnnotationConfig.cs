using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationConfig
    {
        public List<ObjectClass> ObjectClasses { get; set; }
        public List<AnnotationPackageTag> Tags { get; set; }

        public AnnotationConfig()
        {
            this.ObjectClasses = new List<ObjectClass>();
            this.Tags = new List<AnnotationPackageTag>();
        }
    }
}
