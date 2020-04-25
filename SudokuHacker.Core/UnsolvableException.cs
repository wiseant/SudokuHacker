using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SudokuHacker
{
    public class UnsolvableException : Exception
    {

        public UnsolvableException()
        {
        }

        public UnsolvableException(string message) : base(message)
        {
        }

        public UnsolvableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsolvableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
