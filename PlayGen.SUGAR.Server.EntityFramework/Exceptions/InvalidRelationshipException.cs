using System;

namespace PlayGen.SUGAR.Server.EntityFramework.Exceptions
{
    public class InvalidRelationshipException : Exception
    {
        public InvalidRelationshipException()
        {
        }

        public InvalidRelationshipException(string message) : base(message)
        {
        }

        public InvalidRelationshipException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
