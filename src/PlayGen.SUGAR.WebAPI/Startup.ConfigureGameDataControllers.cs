using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.GameData;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureGameDataControllers(IServiceCollection services)
        {
            // TODO set category types for GameDataControllers used by other controllers
            services.AddScoped<GameData.AchievementController>();
            services.AddScoped<GameData.SkillController>();
            services.AddScoped<RewardController>();
            services.AddScoped<GameData.LeaderboardController>();
        }
    }
}
