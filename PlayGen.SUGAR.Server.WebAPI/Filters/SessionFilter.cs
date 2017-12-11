using System;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.Server.Authentication.Extensions;
using PlayGen.SUGAR.Server.Core.Sessions;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Exceptions;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Filters
{
    public class SessionFilter : IActionFilter
    {
        private readonly SessionTracker _sessionTracker;

        public SessionFilter(SessionTracker sessionTracker)
        {
            _sessionTracker = sessionTracker;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!HasValidateSessionAttribute(context.ActionDescriptor)) return;

			if (!context.HttpContext.Request.Headers.TryGetSessionId(out var sessionId))
            {
                throw new InvalidSessionException("No \"SessionId\" set in the token's claims.");
            }

            if (!_sessionTracker.IsActive(sessionId))
            {
                throw new InvalidSessionException($"Session with id \"{sessionId}\" is not active.");
            }

            _sessionTracker.SetLastActive(sessionId, DateTime.UtcNow);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
        
        private bool HasValidateSessionAttribute(ActionDescriptor actionDescriptor)
        {
            // todo possibly cache these values - do a test first to see the performance benefit with caching vs non caching
            if (actionDescriptor.GetCustomClassAttribute<ValidateSessionAttribute>() != null)
            {
                return true;
            }

            return actionDescriptor.GetCustomMethodAttribute<ValidateSessionAttribute>() != null;
        }
    }
}
