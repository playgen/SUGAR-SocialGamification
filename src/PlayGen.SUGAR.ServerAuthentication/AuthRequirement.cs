using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;

namespace PlayGen.SUGAR.ServerAuthentication
{
    public class AuthRequirement : IAuthorizationRequirement
    {
        public ClaimScope ClaimScope { get; set; }

        public string Name { get; set; }

        public AuthRequirement(AuthOperation operation)
        {
            ClaimScope = operation.ClaimScope;
            Name = operation.Name;
        }
    }
}
