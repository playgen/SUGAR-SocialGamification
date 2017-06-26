using System;

namespace PlayGen.SUGAR.Data.EntityFramework.Exceptions
{
	public class DuplicateRecordException : Exception
	{
		public DuplicateRecordException()
			: base("There was a duplicate!")
		{
		}

		public DuplicateRecordException(string message)
			: base(message)
		{
		}

		public DuplicateRecordException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}