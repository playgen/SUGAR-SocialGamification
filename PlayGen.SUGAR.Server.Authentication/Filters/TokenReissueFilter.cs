using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.Server.Authentication.Extensions;

namespace PlayGen.SUGAR.Server.Authentication.Filters
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

            // If there is no authorization added to the response already by a lower layer.
            if (!response.HasAuthorization()
                && request.Headers.TryGetClaim("SessionId", out long sessionId)
                && request.Headers.TryGetClaim("GameId", out int gameId)
                && request.Headers.TryGetClaim("UserId", out int userId))
            {
                _tokenController.IssueSessionToken(context.HttpContext, sessionId, gameId, userId);
            }
        }
    }
}
