using System;

namespace PlayGen.SUGAR.Common.Shared.Exceptions
{
    public abstract class SUGARException : Exception
    {
        protected SUGARException() : base()
        {
        }

        protected SUGARException(string message) : base(message)
	    {
        }

        protected SUGARException(string message, Exception innerException) : base(message, innerException)
	    {
        }
    }
}
