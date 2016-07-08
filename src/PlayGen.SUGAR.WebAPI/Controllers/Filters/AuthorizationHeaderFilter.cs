using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.ServerAuthentication;
using PlayGen.SUGAR.ServerAuthentication.Helpers;

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
			if (!AuthorizationHeader.HasAuthorization(context.HttpContext.Response.Headers))
			{
				AuthorizationHeader.SetAuthorization(context.HttpContext.Response.Headers, _authorization);
			}
			else
			{
				// Todo re-issue if timed out.
			}
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			_authorization = AuthorizationHeader.GetAuthorization(context.HttpContext.Request.Headers);
		}
	}
}
