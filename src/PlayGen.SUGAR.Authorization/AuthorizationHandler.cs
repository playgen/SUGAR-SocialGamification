using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PlayGen.SUGAR.Authorization
{
    public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement, int resource)
        {
            //todo: check against claims in db
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}