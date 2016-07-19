using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace PlayGen.SUGAR.WebAPI.Controllers.Filters
{
	public class ExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			string exceptionName = context.Exception.GetType().ToString();
			switch (exceptionName)
			{
				case "PlayGen.SUGAR.Data.EntityFramework.Exceptions.DuplicateRecordException":
					context.Result = new ObjectResult("Invalid data provided.");
					context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
					break;
				default:
					context.Result = new ObjectResult(context.Exception.Message);
					context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					break;
			}
			
			context.Exception = null;
			base.OnException(context);
		}
	}
}