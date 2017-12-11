using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.WebAPI.Exceptions;

namespace PlayGen.SUGAR.Server.WebAPI.Filters
{
	/// <summary>
	/// Wraps internal exceptions for external consumption.
	/// </summary>
	public class ExceptionFilter : ExceptionFilterAttribute
	{
        private readonly ILogger _logger;

		public ExceptionFilter(ILogger<ExceptionFilter> logger)
		{
			_logger = logger;
		}
        
        public override void OnException(ExceptionContext context)
		{
			var exception = context.Exception;

			switch (exception)
			{
				case IncompatibleAPIVersionException apiVersion:
					context.Result = new ObjectResult(apiVersion.Message);
					context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					break;

				case DuplicateRecordException duplicateRecord:
					context.Result = new ObjectResult("Invalid data provided.");
					context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
					break;

				case InvalidAccountDetailsException invalidAccount:
					context.Result = new ObjectResult(invalidAccount.Message);
					context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					break;

				case InvalidSessionException invalidSession:
					context.Result = new ObjectResult(invalidSession.Message);
					context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					break;

				default:
					context.Result = new ObjectResult(context.Exception.Message);
					context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					break;
			}
			
            _logger.LogError(exception.Message);

            context.Exception = null;
			base.OnException(context);
		}
	}
}