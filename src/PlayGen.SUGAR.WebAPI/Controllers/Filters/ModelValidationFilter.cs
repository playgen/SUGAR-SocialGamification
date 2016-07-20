using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers.Filters
{
	/// <summary>
	/// Checks if models are valid.
	/// If a model is not valid, a Bad Request is returned as the result with details
	/// on why the model state is invalid.
	/// </summary>
	public class ModelValidationFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				var errorString = "";
				foreach (var value in context.ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						if (!string.IsNullOrEmpty(error.ErrorMessage))
						{
							errorString += error.ErrorMessage;
						} else
						{
							errorString += error.Exception.Message;
						}
						errorString += "\n";
					}
				}
				context.Result = new BadRequestObjectResult(errorString);
			}
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
		}
	}
}