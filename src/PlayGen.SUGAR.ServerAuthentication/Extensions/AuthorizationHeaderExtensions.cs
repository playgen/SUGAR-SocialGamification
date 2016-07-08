using Microsoft.AspNetCore.Http;

namespace PlayGen.SUGAR.ServerAuthentication.Extensions
{
	public static class AuthorizationHeaderExtensions
	{
		private const string Key = "Authorization";
		private const string ValuePrefix = "Bearer ";
		
		public static string GetAuthorization(this HttpRequest request)
		{
			string authorization = null;

			if (request.Headers.ContainsKey(Key))
			{
				authorization = request.Headers[Key];
			}

			return authorization;
		}

		public static bool HasAuthorization(this HttpResponse response)
		{
			return response.Headers.ContainsKey(Key);
		}

		public static bool HasAuthorization(this HttpRequest request)
		{
			return request.Headers.ContainsKey(Key);
		}

		public static void SetAuthorization(this HttpResponse response, string authorization)
		{
			response.Headers[Key] = authorization;
		}

		public static void SetAuthorizationToken(this HttpResponse response, string token)
		{
			response.Headers[Key] = ValuePrefix + token;
		}

		public static string GetAuthorizationToken(this HttpRequest request)
		{
			string token = null;

			if (request.HasAuthorization())
			{
				var authorization = (string)request.Headers[Key];

				if (authorization.StartsWith(ValuePrefix))
				{
					token = authorization.Substring(ValuePrefix.Length);
				}
			}

			return token;
		}
	}
}
