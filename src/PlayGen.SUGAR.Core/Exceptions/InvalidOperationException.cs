using System;
using PlayGen.SUGAR.Common.Exceptions;

namespace PlayGen.SUGAR.Core.Exceptions
{
    public class InvalidOperationException : SUGARException
    {
        public InvalidOperationException()
        {
        }

        public InvalidOperationException(string message) : base(message)
        {
        }

        public InvalidOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
