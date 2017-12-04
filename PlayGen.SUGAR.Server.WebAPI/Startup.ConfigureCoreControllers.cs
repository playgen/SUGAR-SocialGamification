using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Server.Core.Controllers;

namespace PlayGen.SUGAR.Server.WebAPI
{
	public partial class Startup
	{
		private void ConfigureCoreControllers(IServiceCollection services)
		{
			services.AddSingleton<AccountController>();
			services.AddSingleton<AccountSourceController>();
			services.AddSingleton<ActorClaimController>();
			services.AddSingleton<ActorDataController>();
		    services.AddSingleton<ActorController>();
			services.AddSingleton<ActorRoleController>();
			services.AddSingleton<EvaluationController>();
			services.AddSingleton<GameController>();
            services.AddSingleton<GameDataController>();
            services.AddSingleton<GroupController>();
			services.AddSingleton<GroupMemberController>();
			services.AddSingleton<LeaderboardController>();
			services.AddSingleton<ResourceController>();
			services.AddSingleton<RewardController>();
			services.AddSingleton<RoleController>();
			services.AddSingleton<RoleClaimController>();
			services.AddSingleton<UserController>();
			services.AddSingleton<UserFriendController>();
		    services.AddSingleton<MatchController>();
		}
	}
}
