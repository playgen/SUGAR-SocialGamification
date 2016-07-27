using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.WebAPI.Exceptions;

namespace PlayGen.SUGAR.WebAPI.Controllers.Filters
{
	/// <summary>
	/// 
	/// </summary>
	public class ExceptionFilter : ExceptionFilterAttribute
	{
		/// <inheritdoc />
		public override void OnException(ExceptionContext context)
		{
			var exception = context.Exception;
			var handled = false;

			if (exception is DuplicateRecordException)
			{
				context.Result = new ObjectResult("Invalid data provided.");
				context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Conflict;
				handled = true;
			}

			if (exception is InvalidAccountDetailsException)
			{
				context.Result = new ObjectResult(exception.Message);
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				handled = true;
			}

			if (!handled)
			{ 
				context.Result = new ObjectResult(context.Exception.Message);
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}
			
			context.Exception = null;
			base.OnException(context);
		}
	}
}