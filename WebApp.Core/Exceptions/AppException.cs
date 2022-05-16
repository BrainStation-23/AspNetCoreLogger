using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace WebApp.Core.Exceptions
{
    [Serializable]
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
