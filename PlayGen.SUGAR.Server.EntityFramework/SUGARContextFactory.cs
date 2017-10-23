using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using PlayGen.SUGAR.Server.EntityFramework.Extensions;

namespace PlayGen.SUGAR.Server.EntityFramework
{
	public class SUGARContextFactory
	{
		private readonly string _connectionString;

		public SUGARContextFactory(string connectionString = null)
		{
			_connectionString = connectionString;
		}

		public SUGARContext Create()
		{
			var optionsBuilder = new DbContextOptionsBuilder<SUGARContext>();
			optionsBuilder.UseMySQL(_connectionString);

			var context = new SUGARContext(optionsBuilder.Options);
			var newlyCreated = context.Database.EnsureCreated();

			if (newlyCreated)
			{
				context.Seed();
			}

			return context;
		}
	}
}
