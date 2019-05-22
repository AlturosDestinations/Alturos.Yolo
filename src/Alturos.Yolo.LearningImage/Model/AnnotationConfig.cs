using System.Collections.Generic;

namespace Alturos.Yolo.LearningImage.Model
{
    public class AnnotationConfig
    {
        public List<ObjectClass> ObjectClasses { get; set; }
        public List<Tag> Tags { get; set; }

        public AnnotationConfig()
        {
            this.ObjectClasses = new List<ObjectClass>();
            this.Tags = new List<Tag>();
        }
    }
}
