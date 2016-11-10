using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace PlayGen.SUGAR.ServerAuthentication
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly TokenAuthOptions _tokenOptions;

        public TokenController(TokenAuthOptions token)
        {
            _tokenOptions = token;
        }

        [HttpPost]
        public dynamic CreateToken([FromBody] int claimerId)
		{
            var expiry = DateTime.UtcNow.AddHours(2);
            var tok = GetToken(claimerId.ToString(), expiry);
            return new { authenticated = true, token = tok, tokenExpires = expiry };
		}

		[HttpGet]
        [Authorize("Bearer")]
        public dynamic IsTokenValid()
        {
            bool authenticated = false;
            string user = null;
            string tok = null;
            DateTime? tokenExpires = DateTime.UtcNow.AddHours(2);

            var currentUser = HttpContext.User;
            if (currentUser != null)
            {
                authenticated = currentUser.Identity.IsAuthenticated;
                if (authenticated)
                {
                    user = currentUser.Identity.Name;
                    tokenExpires = DateTime.UtcNow.AddMinutes(2);
                    tok = GetToken(user, tokenExpires);
                }
            }
            return new { authenticated = authenticated, user = user, token = tok, tokenExpires = tokenExpires };
        }

        private string GetToken(string userId, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(userId, "TokenAuth"));

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "SUGAR",
                Audience = "Players",
                SigningCredentials = _tokenOptions.SigningCredentials,
                Subject = identity,
                Expires = expires,
            });
            return handler.WriteToken(securityToken);
        }
	}
}
