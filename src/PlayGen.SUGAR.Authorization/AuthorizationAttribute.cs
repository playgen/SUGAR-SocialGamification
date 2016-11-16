using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using PlayGen.SUGAR.Common.Shared.Permissions;
using System.Reflection;

namespace PlayGen.SUGAR.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
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
            if (context.HttpContext.Items.Count > 0)
            {
                return;
            }
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            var customAtt = actionDescriptor?.MethodInfo.GetCustomAttributes(typeof(AuthorizationAttribute), false) as AuthorizationAttribute[];
            if (customAtt != null && customAtt.Length > 0)
            {
                if (customAtt.Length == 1)
                {
                    context.HttpContext.Items.Add("Requirements", new AuthorizationRequirement(customAtt[0].ClaimScope, customAtt[0].Name));
                }
                else
                {
                    foreach (var att in customAtt)
                    {
                        context.HttpContext.Items.Add(att.ClaimScope + "Requirements", new AuthorizationRequirement(att.ClaimScope, att.Name));
                    }
                }
            }
        }
    }
}
