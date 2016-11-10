using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;

namespace PlayGen.SUGAR.ServerAuthentication
{
    public class AuthRequirement : IAuthorizationRequirement
    {
        public ClaimScope ClaimScope { get; set; }

        public string Name { get; set; }

        public AuthRequirement(ClaimScope scope, string name)
        {
            ClaimScope = scope;
            Name = name;
        }
    }
}
