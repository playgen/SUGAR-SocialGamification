using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using PlayGen.SUGAR.Common.Authorization;

namespace PlayGen.SUGAR.Server.Authorization
{
	public static class AuthorizationExtensions
	{
		public static IAuthorizationRequirement ScopeItems(this HttpContext context, ClaimScope scope)
		{
			return (IAuthorizationRequirement)context.Items[scope.Key()];
		}

		public static string Key(this ClaimScope scope)
		{
			return $"{scope}-Requirements";
		}
	}
}
