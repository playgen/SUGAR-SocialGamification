using System;
using PlayGen.SUGAR.Common.Shared.Exceptions;

namespace PlayGen.SUGAR.Core.Exceptions
{
    public class InvalidDataException : SUGARException
    {
        public InvalidDataException()
        {
        }

        public InvalidDataException(string message) : base(message)
        {
        }

        public InvalidDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
