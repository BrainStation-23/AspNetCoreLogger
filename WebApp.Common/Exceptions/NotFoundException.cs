using System;
using System.Runtime.Serialization;

namespace WebApp.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {

        public NotFoundException(string message) : base(string.Format(ApplicationExceptionMessage.NotFound, message))
        {

        }


        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
