using System;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.Core.Sessions;
using PlayGen.SUGAR.ServerAuthentication.Extensions;

namespace PlayGen.SUGAR.WebAPI.Filters
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
            int sessionId;

            if (context.HttpContext.Request.TryGetClaim("SessionId", out sessionId))
            {
                _sessionTracker.SetLastActive(sessionId, DateTime.UtcNow);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
