using System;
using PlayGen.SUGAR.Common.Shared.Exceptions;

namespace PlayGen.SUGAR.Client.Exceptions
{
	public class ClientException : SUGARException
	{
	    public ClientException()
	    {
	    }

	    public ClientException(string message) : base(message)
		{
		}

		public ClientException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
