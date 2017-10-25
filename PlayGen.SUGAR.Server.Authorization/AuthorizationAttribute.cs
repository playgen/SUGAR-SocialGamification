using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.Common.Authorization;

namespace PlayGen.SUGAR.Server.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        public ClaimScope ClaimScope { get; set; }

        public string Name { get; set; }

        public AuthorizationAttribute(ClaimScope claimScope, AuthorizationAction action, AuthorizationEntity entity)
        {
            ClaimScope = claimScope;
			Name = AuthorizationName.Generate(action, entity);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Items.Any())
            {
                return;
            }

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

			if (actionDescriptor?.MethodInfo.GetCustomAttributes(typeof(AuthorizationAttribute), false) is AuthorizationAttribute[] customAtt)
            {
                foreach (var att in customAtt)
                {
                    context.HttpContext.Items.Add(Key(att.ClaimScope), new AuthorizationRequirement(att.ClaimScope, att.Name));
                }
            }
        }

		public static string Key(ClaimScope scope)
		{
			return $"{scope}-Requirements";
		}
    }
}
