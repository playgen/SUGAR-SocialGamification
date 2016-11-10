using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Gore;

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
