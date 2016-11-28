using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.ServerAuthentication.Extensions;

namespace PlayGen.SUGAR.ServerAuthentication.Filters
{
    public class TokenReissueFilter : IActionFilter
    {
        private readonly TokenController _tokenController;

        public TokenReissueFilter(TokenController tokenController)
        {
            _tokenController = tokenController;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            int sessionId, gameId, userId;

            if (request.TryGetClaim("SessionId", out sessionId)
                && request.TryGetClaim("GameId", out gameId)
                && request.TryGetClaim("UserId", out userId))
            {
                _tokenController.IssueToken(context.HttpContext, sessionId, gameId, userId);
            }
        }
    }
}
