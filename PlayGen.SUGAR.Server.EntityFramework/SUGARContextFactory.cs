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
			var optionsBuilder = CreateOptionsBuilder();
			var context = new SUGARContext(optionsBuilder.Options);
			return context;
		}

		public SUGARContext CreateReadOnly()
		{
			var optionsBuilder = CreateOptionsBuilder();
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			var context = new SUGARContext(optionsBuilder.Options, true);
			context.ChangeTracker.AutoDetectChangesEnabled = false;

			return context;
		}

		public DbContextOptionsBuilder ApplyOptions(DbContextOptionsBuilder options)
		{
			options.UseMySql(ConnectionString);
			return options;
		}

		private DbContextOptionsBuilder<SUGARContext> CreateOptionsBuilder()
		{
			var optionsBuilder = new DbContextOptionsBuilder<SUGARContext>();
			optionsBuilder.UseMySql(ConnectionString);
			return optionsBuilder;
		}
    }
}
