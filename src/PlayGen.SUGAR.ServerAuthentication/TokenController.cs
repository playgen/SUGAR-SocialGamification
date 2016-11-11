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
		public string CreateToken([FromBody] int claimerId)
		{
			var expiry = DateTime.UtcNow.AddHours(2);
			var tok = GetToken(claimerId.ToString(), expiry);
			return tok;
		}

		[HttpGet]
		[Authorize("Bearer")]
		public string Get()
		{
			bool authenticated;
			string user;
			string tok = null;
			DateTime tokenExpires = default(DateTime);

			var currentUser = HttpContext.User;
			if (currentUser != null)
			{
				authenticated = currentUser.Identity.IsAuthenticated;
				if (authenticated)
				{
					user = currentUser.Identity.Name;
					tokenExpires = DateTime.UtcNow.AddHours(2);
					tok = GetToken(user, tokenExpires);
				}
			}
			return tok;
		}

		private string GetToken(string userId, DateTime expires)
		{
			var handler = new JwtSecurityTokenHandler();
			ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(userId, "TokenAuth"), new[] { new Claim("Expiry", expires.ToString(), ClaimValueTypes.DateTime), new Claim("UserID", userId, ClaimValueTypes.Integer) });

			var securityToken = handler.CreateToken(new SecurityTokenDescriptor
			{
                Issuer = _tokenOptions.Issuer,
                Audience = _tokenOptions.Audience,
                SigningCredentials = _tokenOptions.SigningCredentials,
				Subject = identity,
				Expires = expires,
			});
			return handler.WriteToken(securityToken);
		}
	}
}
