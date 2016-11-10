using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

public class AuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, int>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, int resource)
    {
        //todo: check against claims in db
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}