using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Core.Controllers;

namespace PlayGen.SUGAR.WebAPI
{
	public partial class Startup
	{
		private void ConfigureCoreControllers(IServiceCollection services)
		{
			services.AddScoped<AccountController>();
			services.AddScoped<AccountSourceController>();
			services.AddScoped<ActorClaimController>();
			services.AddScoped<ActorDataController>();
			services.AddScoped<ActorController>();
			services.AddScoped<ActorRoleController>();
			services.AddScoped<EvaluationController>();
			services.AddScoped<GameController>();
			services.AddScoped<GameDataController>();
			services.AddScoped<GroupController>();
			services.AddScoped<GroupMemberController>();
			services.AddScoped<LeaderboardController>();
			services.AddScoped<ResourceController>();
			services.AddScoped<RewardController>();
			services.AddScoped<RoleController>();
			services.AddScoped<RoleClaimController>();
			services.AddScoped<UserController>();
			services.AddScoped<UserFriendController>();
			services.AddScoped<MatchController>();
		}
	}
}