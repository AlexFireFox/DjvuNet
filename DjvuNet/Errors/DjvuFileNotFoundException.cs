﻿using System;
using System.IO;

namespace DjvuNet.Errors
{
    public class DjvuFileNotFoundException : FileNotFoundException
    {
        public DjvuFileNotFoundException() : base()
        {
        }

        public DjvuFileNotFoundException(string message) : base(message)
        {
        }

        public DjvuFileNotFoundException(string message, Exception innerException) : base (message, innerException)
        {
        }

        public DjvuFileNotFoundException(string message, string fileName) : base (message, fileName)
        {
        }

#if !NETSTANDARD2_0
        public DjvuFileNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base (info, context)
        {
        }
#endif

        public DjvuFileNotFoundException(string message, string fileName, Exception innerException)
            : base(message, fileName, innerException)
        {
        }
    }
}
