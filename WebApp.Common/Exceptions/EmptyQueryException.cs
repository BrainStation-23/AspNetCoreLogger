﻿using System;
using System.Runtime.Serialization;

namespace WebApp.Common.Exceptions
{
    [Serializable]
    public class EmptyQueryException : Exception
    {
        public EmptyQueryException(string message) : base(message)
        {
        }

        protected EmptyQueryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
