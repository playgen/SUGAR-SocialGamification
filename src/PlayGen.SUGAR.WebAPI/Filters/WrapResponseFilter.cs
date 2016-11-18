using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.ServerAuthentication.Extensions;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Filters
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
            var result = context.Result as ObjectResult;

            if (result == null)
            {
                return;
            }

            int gameId;
            int userId;

            if (!context.HttpContext.Request.TryGetClaim("GameId", out gameId)
                || !context.HttpContext.Request.TryGetClaim("UserId", out userId))
            {
                return;
            }

            var wrappedResponse = new ResponseWrapper<object>
            {
                Response = result.Value,
                EvaluationsProgress = GetPendingEvents(gameId, userId)
            };

            context.Result = new ObjectResult(wrappedResponse);
        }

        private List<EvaluationProgressResponse> GetPendingEvents(int gameId, int actorId)
        {
            var pendingNotifications = _evaluationTracker.GetPendingNotifications(gameId, actorId);
            var progressResponses = pendingNotifications.ToContractList();
            return progressResponses;
        }
    }
}