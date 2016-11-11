using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Core.Authorization;
using PlayGen.SUGAR.Core.Controllers;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureAuthorization(IServiceCollection services)
        {
        
            services.AddScoped<ClaimController>();
            var container = services.BuildServiceProvider();
            var claimController = container.GetService<ClaimController>();
            claimController.GetAuthOperations();
        }
    }
}
