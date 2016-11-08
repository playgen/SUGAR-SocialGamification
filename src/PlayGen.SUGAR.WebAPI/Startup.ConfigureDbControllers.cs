using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureDbControllers(IServiceCollection services)
        {
            // Set EntityFramework's DBContext's connection string
            services.AddScoped<AccountController>();
            services.AddScoped<GameController>();
            services.AddScoped<GroupController>();
            services.AddScoped<UserController>();
            services.AddScoped<ActorController>();
            services.AddScoped<GameDataController>();
            services.AddScoped<AchievementController>();
            services.AddScoped<SkillController>();
            services.AddScoped<LeaderboardController>();
            services.AddScoped<GroupRelationshipController>();
            services.AddScoped<UserRelationshipController>();
        }
    }
}
