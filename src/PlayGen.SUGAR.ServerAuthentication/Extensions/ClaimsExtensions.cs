using Microsoft.AspNetCore.Http;

namespace PlayGen.SUGAR.ServerAuthentication.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetSessionId(this HttpRequest request)
        {
            return request.GetClaimInt(ClaimConstants.SessionId);
        }

        public static int GetGameId(this HttpRequest request)
        {
            return request.GetClaimInt(ClaimConstants.GameId);
        }

        public static int GetUserId(this HttpRequest request)
        {
            return request.GetClaimInt(ClaimConstants.UserId);
        }

        public static bool TryGetSessionId(this HttpRequest request, out int sessionId)
        {
            return request.TryGetClaim(ClaimConstants.SessionId, out sessionId);
        }

        public static bool TryGetGameId(this HttpRequest request, out int gameId)
        {
            return request.TryGetClaim(ClaimConstants.GameId, out gameId);
        }

        public static bool TryGetUserId(this HttpRequest request, out int userId)
        {
            return request.TryGetClaim(ClaimConstants.UserId, out userId);
        }
    }
}
