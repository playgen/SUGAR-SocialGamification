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
				context.Result = new BadRequestObjectResult(context.ModelState);
			}
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
		}
	}
}