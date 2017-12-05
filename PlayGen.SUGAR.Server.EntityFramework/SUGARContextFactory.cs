using Microsoft.EntityFrameworkCore;

namespace PlayGen.SUGAR.Server.EntityFramework
{
	public class SUGARContextFactory
	{
		public readonly string ConnectionString;
		private readonly DbContextOptions _options;

		public SUGARContextFactory(string connectionString = null)
		{
			ConnectionString = connectionString;
			_options = ApplyOptions(new DbContextOptionsBuilder()).Options;
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
