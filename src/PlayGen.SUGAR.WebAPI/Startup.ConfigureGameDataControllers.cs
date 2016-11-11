using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Core;
using PlayGen.SUGAR.Core.Controllers;
using PlayGen.SUGAR.Core.Utilities;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureGameDataControllers(IServiceCollection services)
        {
            // TODO set category types for GameDataControllers used by other controllers
            services.AddScoped<EvaluationController>();
            services.AddScoped<ResourceController>();
            services.AddScoped<RewardController>();
            services.AddScoped<LeaderboardController>();
        }
    }
}
