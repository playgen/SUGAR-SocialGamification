using System;

namespace PlayGen.SUGAR.Server.WebAPI.Exceptions
{
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException(string message) : base(message)
        {
        }
    }
}
