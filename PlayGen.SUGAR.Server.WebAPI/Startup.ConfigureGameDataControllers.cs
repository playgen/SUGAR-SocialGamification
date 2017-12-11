using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Server.Core.Controllers;

namespace PlayGen.SUGAR.Server.WebAPI
{
    public partial class Startup
    {
        private void ConfigureGameDataControllers(IServiceCollection services)
        {
            // TODO set category types for GameDataControllers used by other controllers
            services.AddSingleton<EvaluationController>();
            services.AddSingleton<ResourceController>();
            services.AddSingleton<RewardController>();
            services.AddSingleton<LeaderboardController>();
        }
    }
}
