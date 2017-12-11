using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Exceptions;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Filters
{
	/// <summary>
	/// Ensures API Versions match for incoming request and server
	/// </summary>
	public class APIVersionFilterFilter : IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (context.ActionDescriptor.GetCustomMethodAttribute<AllowAnyAPIVersion>() == null)
			{
				if (!context.HttpContext.Request.Headers.TryGetValue(APIVersion.Key, out var requestApiVersion)
					|| !APIVersion.IsCompatible(requestApiVersion))
				{
					throw new IncompatibleAPIVersionException("Server and Client API Major versions do not match." +
															" This is likley to cause unpredictable behaviour." +
															$" \nServer API Version: {requestApiVersion}" +
															$" \nClient API Version: {requestApiVersion}");
				}
			}
		}
	}
}
