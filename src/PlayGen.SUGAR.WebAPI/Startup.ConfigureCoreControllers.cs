using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Core.Controllers;
using PlayGen.SUGAR.Core.Utilities;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureCoreControllers(IServiceCollection services)
        {
            // Set EntityFramework's DBContext's connection string
            services.AddScoped<AccountController>();
            services.AddScoped<EvaluationController>();
            services.AddScoped<GameController>();
        }
    }
}
