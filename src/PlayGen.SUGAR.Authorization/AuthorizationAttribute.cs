using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using PlayGen.SUGAR.Common.Shared.Permissions;
using System.Reflection;
using System.Linq;

namespace PlayGen.SUGAR.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        public ClaimScope ClaimScope { get; set; }

        public string Name { get; set; }

        public AuthorizationAttribute(ClaimScope scope, string action, string type)
        {
            ClaimScope = scope;
            Name = string.Concat(action, type);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            var customAtt = actionDescriptor?.MethodInfo.GetCustomAttributes(typeof(AuthorizationAttribute), false).SingleOrDefault() as AuthorizationAttribute;
            if (customAtt != null)
            {
                context.HttpContext.Items.Add("Requirements", new AuthorizationRequirement(customAtt.ClaimScope, customAtt.Name));
            }
        }
    }
}
