using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Core.Exceptions
{
    public static class ExceptionMessage
    {
        public static Exception GetOriginalException(this Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;

            return exception;
        }

        public static string ToInnerMessage(this Exception exception)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("Exception type: " + exception.GetType().FullName);
            s.AppendLine("Message       : " + exception.Message);
            s.AppendLine("Stacktrace    : " + exception.StackTrace);
            s.AppendLine();

            return s.ToString();
        }

        public static string ToInnerString(this Exception exception)
        {
            Exception e = exception;
            StringBuilder s = new StringBuilder();

            while (e != null)
            {
                s.AppendLine("Exception type: " + e.GetType().FullName);
                s.AppendLine("Message       : " + e.Message);
                s.AppendLine("Stacktrace    : " + e.StackTrace);
                s.AppendLine();
                e = e.InnerException;
            }

            return s.ToString();
        }
    }
}
