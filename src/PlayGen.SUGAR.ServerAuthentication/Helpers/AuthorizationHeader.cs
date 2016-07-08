using System;
using Microsoft.AspNetCore.Http;

namespace PlayGen.SUGAR.ServerAuthentication.Helpers
{
	public static class AuthorizationHeader
	{
		private const string Key = "Authorization";
		private const string ValuePrefix = "Bearer ";
		
		public static string GetAuthorization(IHeaderDictionary headers)
		{
			string authorization = null;

			if (headers.ContainsKey(Key))
			{
				authorization = headers[Key];
			}

			return authorization;
		}

		public static bool HasAuthorization(IHeaderDictionary headers)
		{
			return headers.ContainsKey(Key);
		}

		public static void SetAuthorization(IHeaderDictionary headers, string _authorization)
		{
			headers[Key] = _authorization;
		}

		public static void SetAuthorizationToken(IHeaderDictionary headers, string token)
		{
			headers[Key] = ValuePrefix + token;
		}

		public static string GetAuthorizationToken(IHeaderDictionary headers)
		{
			string token = null;

			if (HasAuthorization(headers))
			{
				var authorization = (string)headers[Key];

				if (authorization.StartsWith(ValuePrefix))
				{
					token = authorization.Substring(ValuePrefix.Length);
				}
			}

			return token;
		}
	}
}
