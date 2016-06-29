using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlayGen.SUGAR.WebAPI.Filters
{
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
