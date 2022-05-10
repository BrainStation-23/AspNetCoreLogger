using System;

namespace WebApp.Core.Exceptions
{
    public class EmptyQueryException : Exception
    {
        public EmptyQueryException(string message) : base(message)
        {
        }
    }
}
