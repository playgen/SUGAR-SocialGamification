using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Authorization;
using System.Linq;

namespace PlayGen.SUGAR.Core.Authorization
{
    public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement, int>
    {
        private readonly Data.EntityFramework.Controllers.ActorRoleController _actorRoleDbController;
        private readonly Data.EntityFramework.Controllers.ClaimController _claimDbController;
        private readonly Data.EntityFramework.Controllers.RoleClaimController _roleClaimDbController;

        public AuthorizationHandler(Data.EntityFramework.Controllers.ActorRoleController actorRoleDbController,
                    Data.EntityFramework.Controllers.ClaimController claimDbController,
                    Data.EntityFramework.Controllers.RoleClaimController roleClaimDbController)
        {
            _actorRoleDbController = actorRoleDbController;
            _claimDbController = claimDbController;
            _roleClaimDbController = roleClaimDbController;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement, int entityId)
        {
            var claim = _claimDbController.Get(requirement.ClaimScope, requirement.Name);
            if (claim != null)
            {
                var roles = _actorRoleDbController.GetActorRolesForEntity(int.Parse(context.User.Identity.Name), entityId);
                var claims = _roleClaimDbController.GetClaimsByRoles(roles.Select(r => r.Id));
                if (claims.Any(c => c.Id == claim.Id))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}