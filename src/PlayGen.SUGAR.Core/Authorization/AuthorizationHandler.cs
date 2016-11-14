using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PlayGen.SUGAR.Authorization
{
    public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement, int>
    {
        private readonly Data.EntityFramework.Controllers.ActorClaimController _actorClaimDbController;
        private readonly Data.EntityFramework.Controllers.ClaimController _claimDbController;

        public AuthorizationHandler(Data.EntityFramework.Controllers.ActorClaimController actorClaimDbController,
                    Data.EntityFramework.Controllers.ClaimController claimDbController)
        {
            _actorClaimDbController = actorClaimDbController;
            _claimDbController = claimDbController;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement, int entityId)
        {
            var claim = _claimDbController.Get(requirement.ClaimScope, requirement.Name);
            if (claim != null)
            {
                if (_actorClaimDbController.Check(int.Parse(context.User.Identity.Name), entityId, claim.Id))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}