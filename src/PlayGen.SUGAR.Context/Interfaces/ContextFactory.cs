using PlayGen.SUGAR.Data.EntityFramework;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.Context.Interfaces
{
	public class ContextFactory : IContextFactory
	{
		private readonly string _connectionString;

		public ContextFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		public DbContext CreateContext()
		{
			return new SGAContext(_connectionString);
		}
	}
}