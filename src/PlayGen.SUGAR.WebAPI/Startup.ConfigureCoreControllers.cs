using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Core.Controllers;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureCoreControllers(IServiceCollection services)
        {
            // Set EntityFramework's DBContext's connection string
            services.AddScoped<AccountController>();
        }
    }
}
