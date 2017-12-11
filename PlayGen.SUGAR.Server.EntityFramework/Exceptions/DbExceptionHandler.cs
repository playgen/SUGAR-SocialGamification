using System;

namespace PlayGen.SUGAR.Server.EntityFramework.Exceptions
{
	public class DbExceptionHandler
	{
		public void Handle(Exception exception)
		{
			var message = exception.GetBaseException().Message;

			if (message.ToLower().Contains("duplicate"))
			{
				throw new DuplicateRecordException(message);
			}
			throw exception;
		}
	}
}
