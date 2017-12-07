using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PlayGen.SUGAR.Server.Authentication.Exceptions;

namespace PlayGen.SUGAR.Server.Authentication.Extensions
{
    public static class AuthorizationTokenExtensions
    {
        public static long GetClaimLong(this IHeaderDictionary headers, string type)
        {
            if (TryGetClaim(headers, type, out long value))
            {
                return value;
            }

            throw new ClaimNotFoundException($"Couldn't find Claim: {type} of type {value.GetType()}");
        }

        public static int GetClaimInt(this IHeaderDictionary headers, string type)
        {
            if (TryGetClaim(headers, type, out int value))
            {
                return value;
            }

            throw new ClaimNotFoundException($"Couldn't find Claim: {type} of type {value.GetType()}");
        }

        public static DateTime GetClaimDateTime(this IHeaderDictionary headers, string type)
        {
            if (TryGetClaim(headers, type, out DateTime value))
            {
                return value;
            }

            throw new ClaimNotFoundException($"Couldn't find Claim: {type} of type {value.GetType()}");
        }

        public static bool TryGetClaim(this IHeaderDictionary headers, string type, out long value)
        {
            value = default(long);

            if (TryGetClaim(headers, type, ClaimValueTypes.Integer, out var claimValue))
            {
                value = long.Parse(claimValue);
                return true;
            }

            return false;
        }

        public static bool TryGetClaim(this IHeaderDictionary headers, string type, out int value)
        {
            value = default(int);

            if (TryGetClaim(headers, type, ClaimValueTypes.Integer, out var claimValue))
            {
                value = int.Parse(claimValue);
                return true;
            }

            return false;
        }

        public static bool TryGetClaim(this IHeaderDictionary headers, string type, out DateTime value)
        {
            value = default(DateTime);

            if(TryGetClaim(headers, type, ClaimValueTypes.DateTime, out var claimValue))
            {
                value = DateTime.Parse(claimValue);
                return true;
            }

            return false;
        }

        private static bool TryGetClaim(this IHeaderDictionary headers, string type, string valueType, out string value)
        {
            value = null;
            var didGetClaim = false;

            var serializedToken = headers.GetAuthorizationToken();

            if (string.IsNullOrWhiteSpace(serializedToken)) return false;

            var handler = new JwtSecurityTokenHandler();
            
            var token = handler.ReadJwtToken(serializedToken);

            foreach (var claim in token.Claims)
            {
                if (claim.Type == type && claim.ValueType == valueType)
                {
                    value = claim.Value;
                    didGetClaim = true;
                    break;
                }
            }

            return didGetClaim;
        }
    }
}
