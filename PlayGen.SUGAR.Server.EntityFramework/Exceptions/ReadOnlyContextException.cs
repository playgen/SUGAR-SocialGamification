using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PlayGen.SUGAR.Server.EntityFramework.Exceptions
{
    public class ReadOnlyContextException : Exception
    {
        public ReadOnlyContextException() : base("The Context is in ReadOnly mode!")
        {
        }

        public ReadOnlyContextException(string message) : base(message)
        {
        }

        public ReadOnlyContextException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReadOnlyContextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
