using System;
using System.Collections.Generic;
using JWT;
using Newtonsoft.Json;
using PlayGen.SUGAR.ServerAuthentication.Helpers;

namespace PlayGen.SUGAR.ServerAuthentication
{
	public class JsonWebTokenUtility
	{
		private readonly string _secretKey;
		private readonly JwtHashAlgorithm _hashAlgorithm;
		private readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public JsonWebTokenUtility(string secretKey, JwtHashAlgorithm hashAlgorithm = JwtHashAlgorithm.HS256)
		{
			JsonWebToken.JsonSerializer = new JWTSerializationAdapter();
			_secretKey = secretKey;
			_hashAlgorithm = hashAlgorithm;
		}

		public string CreateToken(Dictionary<string, object> claims)
		{
			var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - _unixEpoch).TotalSeconds);
			var issuedAt = Math.Round((DateTime.UtcNow - _unixEpoch).TotalSeconds);
			var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - _unixEpoch).TotalSeconds);

			var payload = new Dictionary<string, object>(claims)
			{
				{"notbefore", notBefore},
				{"issuedat", issuedAt},
				{"expiry", expiry},
			};

			return JsonWebToken.Encode(payload, _secretKey, _hashAlgorithm);
		}

		public bool IsTokenValid(string token)
		{
			return GetTokenValidity(token) == TokenValidity.Valid;
		}

		public TokenValidity GetTokenValidity(string token)
		{
			var payloadJson = JsonWebToken.Decode(token, _secretKey);
			var payloadData = JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson);

			object expiry;
			if (payloadData == null || payloadData.TryGetValue("expiry", out expiry))
			{
				return TokenValidity.Invalid;
			}

			long expiryTicks;
			if (!long.TryParse(expiry.ToString(), out expiryTicks))
			{
				return TokenValidity.Invalid;
			}

			var validUntil = _unixEpoch.AddSeconds(expiryTicks);
			return DateTime.Compare(validUntil, DateTime.UtcNow) <= 0 
				? TokenValidity.Expired
				: TokenValidity.Valid;
		}
	}
}
