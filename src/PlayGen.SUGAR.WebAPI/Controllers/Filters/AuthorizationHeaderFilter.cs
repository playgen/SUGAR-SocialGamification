using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.ServerAuthentication.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers.Filters
{
	/// <summary>
	/// Intercepts token attached to the incomming request and re-attatches
	/// it to the outgoing response.
	/// </summary>
	public class AuthorizationHeaderFilter : IActionFilter
	{
		private string _authorization;

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (!context.HttpContext.Response.HasAuthorization())
			{
				context.HttpContext.Response.SetAuthorization(_authorization);
			}
			else
			{
				// Todo re-issue if timed out.
			}
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			_authorization = context.HttpContext.Request.GetAuthorization();
		}
	}
}
