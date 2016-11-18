using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
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

        [HttpGet("{gameId:int}/{userId:int}")]
        public string CreateToken([FromRoute]int gameId, [FromRoute]int userId)
		{
			var expiry = DateTime.UtcNow.AddHours(2);
			var tok = CreateToken(gameId.ToString(), userId.ToString(), expiry);
			return tok;
		}


		//[HttpGet]
		//[Authorize("Bearer")]
		//public string Get()
		//{
		//    string tok = null;

		//    var currentUser = HttpContext.User;
		//	if (currentUser != null)
		//	{
		//	    var authenticated = currentUser.Identity.IsAuthenticated;
		//	    if (authenticated)
		//		{
        //          // todo get game Id
		//			var user = currentUser.Identity.Name;
		//			var tokenExpires = DateTime.UtcNow.AddHours(2);
		//			tok = GetToken(user, tokenExpires);
		//		}
		//	}
		//	return tok;
		//}

		private string CreateToken(string gameId, string userId, DateTime expires)
		{
			var handler = new JwtSecurityTokenHandler();

			var identity = new ClaimsIdentity(
                new GenericIdentity(userId, "TokenAuth"), 
                new[] 
                {
                    new Claim("Expiry", expires.ToString(), ClaimValueTypes.DateTime),
                    new Claim("UserId", userId, ClaimValueTypes.Integer),
                    new Claim("GameId", gameId, ClaimValueTypes.Integer)
                });

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
