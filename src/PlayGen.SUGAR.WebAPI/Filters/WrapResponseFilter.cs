using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Core.EvaluationEvents;

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

            // todo check if header has send events claim
            // todo need access to actor id and game id here - using session in header

            var gameId = 1;
            var actorId = 1;

            var wrappedResponse = new ResponseWrapper<object>
            {
                Response = result.Value,

                EvaluationsProgress = null //todo uncomment GetPendingEvents(gameId, actorId)
            };

            context.Result = new ObjectResult(wrappedResponse);
        }

        private List<EvaluationProgressResponse> GetPendingEvents(int gameId, int actorId)
        {
            var pendingNotifications = _evaluationTracker.GetPendingNotifications(gameId, actorId);

            // todo turn pending notifications into evaluation progress response list
            return new List<EvaluationProgressResponse>();
        }
    }
}