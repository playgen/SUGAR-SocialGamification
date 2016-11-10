using Microsoft.AspNetCore.Authorization.Infrastructure;
using PlayGen.SUGAR.Common.Shared.Permissions;

namespace PlayGen.SUGAR.ServerAuthentication
{
    public class AuthOperations
    {
        public static AuthRequirement CreateGame = new AuthRequirement(ClaimScope.Game, "Create");
        public static AuthRequirement ReadGame = new AuthRequirement(ClaimScope.Game, "Read");
        public static AuthRequirement UpdateGame = new AuthRequirement(ClaimScope.Game, "Update");
        public static AuthRequirement DeleteGame = new AuthRequirement(ClaimScope.Game, "Delete");
    }
}
