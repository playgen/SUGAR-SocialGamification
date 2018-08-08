using System;

namespace PlayGen.SUGAR.Server.EntityFramework.Exceptions
{
    public class DuplicateRelationshipException : Exception
    {
        public DuplicateRelationshipException()
        {
        }

        public DuplicateRelationshipException(string message) : base(message)
        {
        }

        public DuplicateRelationshipException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
