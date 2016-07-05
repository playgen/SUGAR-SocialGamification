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
	public class TokenHeaderFilter : IResourceFilter
	{
		private string _token;

		public void OnResourceExecuted(ResourceExecutedContext context)
		{
			_token = context.HttpContext.Request.Headers["Bearer"];
		}

		public void OnResourceExecuting(ResourceExecutingContext context)
		{
			context.HttpContext.Response.Headers["Bearer"] = _token;
		}
	}
}
