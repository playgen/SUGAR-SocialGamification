using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using Microsoft.Extensions.Configuration;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureDbControllers(IServiceCollection services)
        {
            // Set EntityFramework's DBContext's connection string
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddScoped((_) => new AccountController(connectionString));
            services.AddScoped((_) => new GameController(connectionString));
            services.AddScoped((_) => new GroupController(connectionString));
            services.AddScoped((_) => new UserController(connectionString));
            services.AddScoped((_) => new ActorController(connectionString));
            services.AddScoped((_) => new GameDataController(connectionString));
            services.AddScoped((_) => new Data.EntityFramework.Controllers.AchievementController(connectionString));
            services.AddScoped((_) => new Data.EntityFramework.Controllers.SkillController(connectionString));
            services.AddScoped((_) => new Data.EntityFramework.Controllers.LeaderboardController(connectionString));
            services.AddScoped((_) => new GameData.ResourceController(connectionString));
            services.AddScoped((_) => new GroupRelationshipController(connectionString));
            services.AddScoped((_) => new UserRelationshipController(connectionString));
        }
    }
}
