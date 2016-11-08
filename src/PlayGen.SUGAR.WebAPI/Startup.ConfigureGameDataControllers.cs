using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.GameData;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureGameDataControllers(IServiceCollection services)
        {
            // TODO set category types for GameDataControllers used by other controllers
            services.AddScoped<AchievementController>();
            services.AddScoped<ResourceController>();
            services.AddScoped<SkillController>();
            services.AddScoped<RewardController>();
            services.AddScoped<LeaderboardController>();
        }
    }
}
