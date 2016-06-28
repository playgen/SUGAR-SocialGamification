using System;
using System.Data.Entity;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public abstract class DbController
	{
		protected readonly string NameOrConnectionString;
		protected readonly DbExceptionHandler DbExceptionHandler = new DbExceptionHandler();

		protected DbController(string nameOrConnectionString)
		{
			NameOrConnectionString = nameOrConnectionString;
		}
		
		protected void SetLog(DbContext context)
		{
			//TODO: replace with some proper logging
			context.Database.Log = Console.WriteLine;
		}

		protected void SaveChanges(DbContext context)
		{
			try
			{
				context.SaveChanges();
			}
			catch (Exception exception)
			{
				DbExceptionHandler.Handle(exception);
			}
		}
	}
}
