using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Server.EntityFramework;

namespace PlayGen.SUGAR.Server.WebAPI
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
