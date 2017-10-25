using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Common.Authorization;

namespace PlayGen.SUGAR.Server.Authorization
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public ClaimScope ClaimScope { get; set; }

        public string Name { get; set; }

        public AuthorizationRequirement(ClaimScope scope, string name)
        {
            ClaimScope = scope;
            Name = name;
        }
    }
}
