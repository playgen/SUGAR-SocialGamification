using System;

namespace PlayGen.SUGAR.Common.Exceptions
{
	public abstract class SUGARException : Exception
	{
		protected SUGARException()
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