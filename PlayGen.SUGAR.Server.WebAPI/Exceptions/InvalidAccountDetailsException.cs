using System;

namespace PlayGen.SUGAR.Server.WebAPI.Exceptions
{
	public class InvalidAccountDetailsException : Exception
	{
		public InvalidAccountDetailsException()
		{
		}

		public InvalidAccountDetailsException(string message) 
			: base(message)
		{
		}

		public InvalidAccountDetailsException(string message, Exception inner) 
			: base(message, inner)
		{   
		}
	}
}
