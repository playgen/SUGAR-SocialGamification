using System;
using System.Data.Entity;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.ContextScope.Interfaces;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public abstract class DbController
	{
		protected SGAContext context => _ambientContextLocator.Get<SGAContext>();

		private readonly IAmbientContextLocator _ambientContextLocator;

		public DbController(IAmbientContextLocator ambientContextLocator)
		{
			_ambientContextLocator = ambientContextLocator;
		}

		// TODO Remove below
		#region Remove
		protected readonly DbExceptionHandler DbExceptionHandler = new DbExceptionHandler();

		protected readonly string NameOrConnectionString;

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
		#endregion
	}
}
