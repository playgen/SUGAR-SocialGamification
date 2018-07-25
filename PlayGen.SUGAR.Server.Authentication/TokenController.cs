using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using PlayGen.SUGAR.Server.Authentication.Extensions;
using PlayGen.SUGAR.Server.Core.Sessions;

namespace PlayGen.SUGAR.Server.Authentication
{
	public class TokenController
	{
		private readonly TokenAuthOptions _tokenOptions;

		public TokenController(TokenAuthOptions token)
		{
			_tokenOptions = token;
		}

		public void IssueSessionToken(HttpContext context, long sessionId, int gameId, int userId)
		{
			var token = CreateSessionToken(sessionId, gameId, userId);
			context.Response.SetAuthorizationToken(token);
		}

		public string IssueLoginToken(int gameId, int userId)
		{
			var expiry = DateTime.UtcNow.Add(_tokenOptions.LoginTokenValidityTimeout);
			return CreateLoginToken(gameId.ToString(), userId.ToString(), expiry);
		}

		public void IssueSessionToken(HttpContext context, Session session)
		{
			var token = CreateSessionToken(session.Id, session.GameId, session.ActorId);
			context.Response.SetAuthorizationToken(token);
		}

		public (int, int) ValidateToken(HttpContext context, string token)
		{
			if (VerifyToken(token))
			{
				var claims = ExtractClaims(token);
				var userId = Convert.ToInt16(claims.First(c => c.Type == ClaimConstants.UserId).Value);
				var gameId = Convert.ToInt16(claims.First(c => c.Type == ClaimConstants.GameId).Value);
				return (userId, gameId);
			}
			return (-1, -1);
		}

		public void RevokeToken(HttpContext context)
		{
			context.Response.SetAuthorizationToken(null);
		}
		
		private string CreateSessionToken(long sessionId, int gameId, int userId)
		{
			var expiry = DateTime.UtcNow.Add(_tokenOptions.SessionTokenValidityTimeout);
			var tok = CreateSessionToken(sessionId.ToString(), gameId.ToString(), userId.ToString(), expiry);
			return tok;
		}

		private string CreateSessionToken(string sessionId, string gameId, string userId, DateTime expires)
		{
			var handler = new JwtSecurityTokenHandler();

			var identity = new ClaimsIdentity(
				new GenericIdentity(userId, "TokenAuth"), 
				new[] 
				{
					new Claim(ClaimConstants.SessionId, sessionId, ClaimValueTypes.Integer),
					new Claim(ClaimConstants.GameId, gameId, ClaimValueTypes.Integer),
					new Claim(ClaimConstants.UserId, userId, ClaimValueTypes.Integer),
					new Claim(ClaimConstants.Expiry, expires.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.DateTime)
				});
			
			var securityToken = handler.CreateToken(new SecurityTokenDescriptor
			{
				Issuer = _tokenOptions.Issuer,
				Audience = _tokenOptions.Audience,
				SigningCredentials = _tokenOptions.SigningCredentials,
				Subject = identity,
				Expires = expires
			});

			return handler.WriteToken(securityToken);
		}

		private string CreateLoginToken(string gameId, string userId, DateTime expires) 
		{
			var identity = new ClaimsIdentity(
				new GenericIdentity(userId, "TokenAuth"),
				new[]
				{
					new Claim(ClaimConstants.GameId, gameId, ClaimValueTypes.Integer),
					new Claim(ClaimConstants.UserId, userId, ClaimValueTypes.Integer),
					new Claim(ClaimConstants.Expiry, expires.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.DateTime)
				});

			var handler = new JwtSecurityTokenHandler();
			var securityToken = handler.CreateToken(new SecurityTokenDescriptor
			{
				IssuedAt = DateTime.UtcNow,
				Issuer = _tokenOptions.Issuer,
				Audience = _tokenOptions.Audience,
				SigningCredentials = _tokenOptions.SigningCredentials,
				Subject = identity,
				Expires = expires
			});
			return handler.WriteToken(securityToken);
		}

		public bool VerifyToken(string token)
		{
			var validationParameters = new TokenValidationParameters()
			{
				IssuerSigningKey = _tokenOptions.SigningCredentials.Key,
				ValidIssuer = _tokenOptions.Issuer,
				ValidAudience = _tokenOptions.Audience,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken validatedToken = null;
			try
			{
				tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
			}
			catch (SecurityTokenException)
			{
				return false;
			}
			//... manual validations return false if anything untoward is discovered
			return validatedToken != null;
		}

		private List<Claim> ExtractClaims(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.ReadJwtToken(token).Claims.ToList();
		}
	}
}
