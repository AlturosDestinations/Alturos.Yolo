using System;
using System.Runtime.Serialization;

namespace Alturos.Yolo
{
    [Serializable]
    public class YoloInitializeException : Exception
    {
        public YoloInitializeException()
        {
        }

        public YoloInitializeException(string message) : base(message)
        {
        }

        public YoloInitializeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected YoloInitializeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
