using Microsoft.EntityFrameworkCore;

namespace PlayGen.SUGAR.Server.EntityFramework
{
	public class SUGARContextFactory
	{
		public readonly string ConnectionString;

		public SUGARContextFactory(string connectionString = null)
		{
			ConnectionString = connectionString;
		}

		public SUGARContext Create()
		{
			var optionsBuilder = new DbContextOptionsBuilder<SUGARContext>();
			optionsBuilder.UseMySql(ConnectionString);
			var context = new SUGARContext(optionsBuilder.Options);

			return context;
		}

		public DbContextOptionsBuilder ApplyOptions(DbContextOptionsBuilder options)
		{
			options.UseMySql(ConnectionString);
			return options;
		}
	}
}
