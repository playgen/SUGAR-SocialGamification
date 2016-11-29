using System;

namespace PlayGen.SUGAR.WebAPI.Exceptions
{
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException(string message) : base(message)
        {
        }
    }
}
