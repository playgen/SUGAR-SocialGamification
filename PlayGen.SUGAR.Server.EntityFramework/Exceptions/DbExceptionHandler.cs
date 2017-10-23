using System;

namespace PlayGen.SUGAR.Data.EntityFramework.Exceptions
{
	public class DbExceptionHandler
	{
		public void Handle(Exception exception)
		{
			string message = exception.GetBaseException().Message;

			if (message.ToLower().Contains("duplicate"))
			{
				throw new DuplicateRecordException(message);
			}
			else
			{
				throw exception;
			}
		}
	}
}
