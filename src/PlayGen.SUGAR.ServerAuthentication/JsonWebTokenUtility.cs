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
			var expiry = (DateTime.UtcNow.AddHours(2) - _unixEpoch).Ticks;
			var issuedAt = (DateTime.UtcNow - _unixEpoch).Ticks;
			var notBefore = (DateTime.UtcNow - _unixEpoch).Ticks;

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

		public TokenValidity GetTokenValidity(string serializedToken)
		{
			Dictionary<string, object> payload;		

			if (!TryGetPayload(serializedToken, out payload))
			{
				return TokenValidity.Invalid;
			}

			object expiry, notBefore;
			if (!payload.TryGetValue("expiry", out expiry) || 
				!payload.TryGetValue("notbefore", out notBefore))
			{
				return TokenValidity.Invalid;	
			}

			long expiryTicks, notBeforeTicks;
			if (!long.TryParse(expiry.ToString(), out expiryTicks) ||
				!long.TryParse(notBefore.ToString(), out notBeforeTicks))
			{
				return TokenValidity.Invalid;
			}

			var validFrom = _unixEpoch.AddTicks(notBeforeTicks);
			if (DateTime.UtcNow < validFrom)
			{
				return TokenValidity.Invalid;
			}

			var validUntil = _unixEpoch.AddTicks(expiryTicks);
			return DateTime.Compare(validUntil, DateTime.UtcNow) <= 0
					? TokenValidity.Expired
					: TokenValidity.Valid;
		}

		private bool TryGetPayload(string serializedToken, out Dictionary<string, object> payload)
		{
			bool success = true;
			payload = null;

			try
			{
				var payloadJson = JsonWebToken.Decode(serializedToken, _secretKey);
				payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson);
			}
			catch
			{
				success = false;
			}

			return success;
		}
	}
}
