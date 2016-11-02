using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.GameData;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureGameDataControllers(IServiceCollection services)
        {
            // Set EntityFramework's DBContext's connection string
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // TODO set category types for GameDataControllers used by other controllers
            services.AddScoped((_) => new GameData.AchievementController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString), new ActorController(connectionString),
                                        new RewardController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString))));
            services.AddScoped((_) => new GameData.SkillController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString), new ActorController(connectionString),
                                        new RewardController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString))));
            services.AddScoped((_) => new RewardController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString)));

            services.AddScoped((_) => new GameData.LeaderboardController(new GameDataController(connectionString), new GroupRelationshipController(connectionString),
                new UserRelationshipController(connectionString), new ActorController(connectionString), new GroupController(connectionString),
                new UserController(connectionString)));
        }
    }
}
