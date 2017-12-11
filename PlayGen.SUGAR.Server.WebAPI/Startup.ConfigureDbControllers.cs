using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Server.Authentication;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;

namespace PlayGen.SUGAR.Server.WebAPI
{
    public partial class Startup
    {
        private void ConfigureDbControllers(IServiceCollection services)
        {
            // Set EntityFramework's DBContext's connection string
            services.AddSingleton<AccountController>();
            services.AddSingleton<AccountSourceController>();
			services.AddSingleton<ActorDataController>();
			services.AddSingleton<GameController>();
            services.AddSingleton<GroupController>();
            services.AddSingleton<UserController>();
            services.AddSingleton<ActorController>();
            services.AddSingleton<EvaluationDataController>();
            services.AddSingleton<EvaluationController>();
            services.AddSingleton<LeaderboardController>();
            services.AddSingleton<RelationshipController>();
            services.AddSingleton<TokenController>();
            services.AddSingleton<ClaimController>();
            services.AddSingleton<RoleController>();
            services.AddSingleton<ActorRoleController>();
			services.AddSingleton<ActorClaimController>();
			services.AddSingleton<RoleClaimController>();
            services.AddSingleton<MatchController>();
        }
    }
}
