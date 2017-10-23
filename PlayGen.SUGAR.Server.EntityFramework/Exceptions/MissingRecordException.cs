using System;

namespace PlayGen.SUGAR.Data.EntityFramework.Exceptions
{
	public class MissingRecordException : Exception
	{
		public MissingRecordException()
		{
		}

		public MissingRecordException(string message)
			: base(message)
		{
		}

		public MissingRecordException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
