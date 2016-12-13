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
            services.AddScoped<AccountSourceController>();
			services.AddScoped<ActorDataController>();
			services.AddScoped<GameController>();
            services.AddScoped<GroupController>();
            services.AddScoped<UserController>();
            services.AddScoped<ActorController>();
            services.AddScoped<SaveDataController>();
            services.AddScoped<EvaluationController>();
            services.AddScoped<LeaderboardController>();
            services.AddScoped<GroupRelationshipController>();
            services.AddScoped<UserRelationshipController>();
            services.AddScoped<TokenController>();
            services.AddScoped<ClaimController>();
            services.AddScoped<RoleController>();
            services.AddScoped<ActorRoleController>();
			services.AddScoped<ActorClaimController>();
			services.AddScoped<RoleClaimController>();
            services.AddScoped<MatchController>();
        }
    }
}
