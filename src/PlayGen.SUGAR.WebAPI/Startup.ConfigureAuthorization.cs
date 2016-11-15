using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Core.Authorization;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureAuthorization(IServiceCollection services)
        {
        
            services.AddScoped<ClaimController>();
            var container = services.BuildServiceProvider();
            var claimController = container.GetService<ClaimController>();
            claimController.GetAuthorizationClaims();
        }
    }
}
