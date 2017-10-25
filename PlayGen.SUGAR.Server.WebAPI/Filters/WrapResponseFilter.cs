using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.Common.Web;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authentication.Extensions;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Filters
{
    public class WrapResponseFilter : IActionFilter
    {
        private readonly EvaluationTracker _evaluationTracker;

        public WrapResponseFilter(EvaluationTracker evaluationTracker)
        {
            _evaluationTracker = evaluationTracker;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var objectResult = context.Result as ObjectResult;

            if (objectResult == null) return;

            var wrappedResponse = new ResponseWrapper<object>()
            {
                Response = objectResult.Value,
                EvaluationsProgress = GetPendingEvents(context.HttpContext.Request)
            };

            context.Result = new ObjectResult(wrappedResponse);
        }

        private List<EvaluationProgressResponse> GetPendingEvents(HttpRequest request)
        {
            List<EvaluationProgressResponse> pendingEvents = null;

            if (request.Headers.ContainsKey(HeaderKeys.EvaluationNotifications))
            {
                int gameId;
                int userId;

                if (request.Headers.TryGetGameId(out gameId)
                    && request.Headers.TryGetUserId(out userId))
                {
                    pendingEvents = GetPendingEvents(gameId, userId);
                }
            }

            return pendingEvents;
        }

        private List<EvaluationProgressResponse> GetPendingEvents(int gameId, int actorId)
        {
            var pendingNotifications = _evaluationTracker.GetPendingNotifications(gameId, actorId);
            var progressResponses = pendingNotifications.ToContractList();

            return progressResponses;
        }
    }
}