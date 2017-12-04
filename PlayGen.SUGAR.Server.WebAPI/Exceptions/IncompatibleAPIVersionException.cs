using System;

namespace PlayGen.SUGAR.Server.WebAPI.Exceptions
{
    public class IncompatibleAPIVersionException : Exception
    {
		public IncompatibleAPIVersionException(string message) : base(message)
		{
		}

		public IncompatibleAPIVersionException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
