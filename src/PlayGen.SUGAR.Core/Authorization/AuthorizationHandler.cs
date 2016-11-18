using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Authorization;
using System.Linq;

using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Core.Controllers;

namespace PlayGen.SUGAR.Core.Authorization
{
    public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement, int>
    {
        private readonly ActorRoleController _actorRoleDbController;
        private readonly ClaimController _claimDbController;
        private readonly RoleClaimController _roleClaimDbController;

        public AuthorizationHandler(ActorRoleController actorRoleDbController,
                    ClaimController claimDbController,
                    RoleClaimController roleClaimDbController)
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

    public class AuthorizationHandlerWithoutEntity : AuthorizationHandler<AuthorizationRequirement, ClaimScope>
    {
        private readonly ActorRoleController _actorRoleDbController;
        private readonly ClaimController _claimDbController;
        private readonly RoleClaimController _roleClaimDbController;

        public AuthorizationHandlerWithoutEntity(ActorRoleController actorRoleDbController,
                    ClaimController claimDbController,
                    RoleClaimController roleClaimDbController)
        {
            _actorRoleDbController = actorRoleDbController;
            _claimDbController = claimDbController;
            _roleClaimDbController = roleClaimDbController;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement, ClaimScope scope)
        {
            var claim = _claimDbController.Get(requirement.ClaimScope, requirement.Name);
            if (claim != null)
            {
                var actorRoles = _actorRoleDbController.GetActorRoles(int.Parse(context.User.Identity.Name)).ToList();
                actorRoles.AddRange(_actorRoleDbController.GetActorRoles(int.Parse(context.User.Identity.Name)).ToList());
                var claims = _roleClaimDbController.GetClaimsByRoles(actorRoles.Select(r => r.RoleId));
                if (claims.Any(c => c.Id == claim.Id))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}