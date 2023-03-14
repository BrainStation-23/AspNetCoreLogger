using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace WebApp.Logger.Exceptions
{
    [Serializable]
    public class AppException : Exception
    {
        public virtual IEnumerable<object> Errors { get; }

        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, IEnumerable<object> errors) : base(message)
        {
            Errors = errors;
        }

        public AppException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
