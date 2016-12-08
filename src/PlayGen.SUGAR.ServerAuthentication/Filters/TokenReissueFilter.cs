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
            var response = context.HttpContext.Response;
            long sessionId;
            int gameId, userId;

            // If there is no authorization added to the response already by a lower layer.
            if (!response.HasAuthorization()
                && request.Headers.TryGetClaim("SessionId", out sessionId)
                && request.Headers.TryGetClaim("GameId", out gameId)
                && request.Headers.TryGetClaim("UserId", out userId))
            {
                _tokenController.IssueToken(context.HttpContext, sessionId, gameId, userId);
            }
        }
    }
}
