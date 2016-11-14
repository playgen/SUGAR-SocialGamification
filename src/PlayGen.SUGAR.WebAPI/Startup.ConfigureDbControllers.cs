using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.ServerAuthentication;

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
            services.AddScoped<EvaluationController>();
            services.AddScoped<LeaderboardController>();
            services.AddScoped<GroupRelationshipController>();
            services.AddScoped<UserRelationshipController>();
            services.AddScoped<ActorDataController>();
            services.AddScoped<ActorClaimController>();
            services.AddScoped<TokenController>();
            services.AddScoped<ClaimController>();
        }
    }
}
