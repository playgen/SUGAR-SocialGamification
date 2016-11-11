using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace PlayGen.SUGAR.ServerAuthentication
{
    public class AuthorizationHandler : AuthorizationHandler<AuthRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthRequirement requirement, int resource)
        {
            //todo: check against claims in db
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}