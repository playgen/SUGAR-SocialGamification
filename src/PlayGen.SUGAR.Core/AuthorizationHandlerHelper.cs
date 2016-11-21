using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Core.Controllers;
using System.Linq;

namespace PlayGen.SUGAR.Core.Authorization
{
    internal class AuthorizationHandlerHelper
    {
        internal static Task HandleRequirements(ClaimController _claimDbController, ActorRoleController _actorRoleDbController, RoleClaimController _roleClaimDbController, AuthorizationHandlerContext context, AuthorizationRequirement requirement, int entityId = 0)
        {
            var claim = _claimDbController.Get(requirement.ClaimScope, requirement.Name);
            if (claim != null)
            {
                var roles = _actorRoleDbController.GetActorRolesForEntity(int.Parse(context.User.Identity.Name), entityId).ToList();
                roles.AddRange(_actorRoleDbController.GetActorRolesForEntity(int.Parse(context.User.Identity.Name), -1).ToList());
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