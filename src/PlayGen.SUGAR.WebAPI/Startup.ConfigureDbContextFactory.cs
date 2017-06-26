using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Data.EntityFramework;

namespace PlayGen.SUGAR.WebAPI
{
	public partial class Startup
	{
		private void ConfigureDbContextFactory(IServiceCollection services)
		{
			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			services.AddSingleton(new SUGARContextFactory(connectionString));
		}
	}
}