using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Core.Controllers;
using System.Linq;

namespace PlayGen.SUGAR.Core.Authorization
{
    internal class AuthorizationHandlerHelper
    {
        internal static Task HandleRequirements(ClaimController _claimDbController, ActorClaimController _actorClaimDbController, AuthorizationHandlerContext context, AuthorizationRequirement requirement, int entityId = 0)
        {
            var claim = _claimDbController.Get(requirement.ClaimScope, requirement.Name);
            if (claim != null)
            {
                var claims = _actorClaimDbController.GetActorClaimsForEntity(int.Parse(context.User.Identity.Name), entityId, requirement.ClaimScope).ToList();
                if (claims.Any(c => c.Id == claim.Id))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}