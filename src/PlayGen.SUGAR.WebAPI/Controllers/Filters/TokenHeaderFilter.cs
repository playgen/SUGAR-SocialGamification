using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers.Filters
{
	/// <summary>
	/// Intercepts token attached to the incomming request and re-attatches
	/// it to the outgoing response.
	/// </summary>
	public class TokenHeaderFilter : IActionFilter
	{
		private string _token;

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (!context.HttpContext.Response.Headers.ContainsKey("Bearer"))
			{
				context.HttpContext.Response.Headers["Bearer"] = _token;
			}
			else
			{
				// Todo re-issue if timed out.
			}
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			_token = context.HttpContext.Request.Headers["Bearer"];
		}
	}
}
