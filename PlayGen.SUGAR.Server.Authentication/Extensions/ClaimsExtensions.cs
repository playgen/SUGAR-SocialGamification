using Microsoft.AspNetCore.Http;

namespace PlayGen.SUGAR.ServerAuthentication.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetSessionId(this IHeaderDictionary headers)
        {
            return headers.GetClaimInt(ClaimConstants.SessionId);
        }

        public static int GetGameId(this IHeaderDictionary headers)
        {
            return headers.GetClaimInt(ClaimConstants.GameId);
        }

        public static int GetUserId(this IHeaderDictionary headers)
        {
            return headers.GetClaimInt(ClaimConstants.UserId);
        }

        public static bool TryGetSessionId(this IHeaderDictionary headers, out int sessionId)
        {
            return headers.TryGetClaim(ClaimConstants.SessionId, out sessionId);
        }

        public static bool TryGetGameId(this IHeaderDictionary headers, out int gameId)
        {
            return headers.TryGetClaim(ClaimConstants.GameId, out gameId);
        }

        public static bool TryGetUserId(this IHeaderDictionary headers, out int userId)
        {
            return headers.TryGetClaim(ClaimConstants.UserId, out userId);
        }
    }
}
