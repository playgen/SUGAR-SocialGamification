using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;

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
			optionsBuilder.UseMySQL(ConnectionString);

			var context = new SUGARContext(optionsBuilder.Options);

			return context;
		}
	}
}
