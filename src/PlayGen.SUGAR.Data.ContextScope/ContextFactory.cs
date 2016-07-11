using PlayGen.SUGAR.Data.ContextScope.Interfaces;
using System;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.ContextScope
{
	public class ContextFactory : IContextFactory
	{
		private readonly string _connectionString;

		public ContextFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		public T CreateContext<T>() where T : DbContext
		{
			return (T)Activator.CreateInstance(typeof(T), _connectionString);
		}
	}
}