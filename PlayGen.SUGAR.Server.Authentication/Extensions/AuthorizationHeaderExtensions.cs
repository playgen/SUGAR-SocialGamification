using Microsoft.AspNetCore.Http;
using PlayGen.SUGAR.Common.Web;

namespace PlayGen.SUGAR.Server.Authentication.Extensions
{
	public static class AuthorizationHeaderExtensions
	{
		private const string ValuePrefix = "Bearer ";
		
		public static string GetAuthorization(this HttpRequest request)
		{
			string authorization = null;

			if (request.Headers.ContainsKey(HeaderKeys.Authorization))
			{
				authorization = request.Headers[HeaderKeys.Authorization];
			}

			return authorization;
		}

		public static string GetAuthorization(this HttpResponse response)
		{
			string authorization = null;

			if (response.Headers.ContainsKey(HeaderKeys.Authorization))
			{
				authorization = response.Headers[HeaderKeys.Authorization];
			}

			return authorization;
		}

		public static bool HasAuthorization(this HttpResponse response)
		{
			return response.Headers.ContainsKey(HeaderKeys.Authorization);
		}

		public static bool HasAuthorization(this HttpRequest request)
		{
			return request.Headers.ContainsKey(HeaderKeys.Authorization);
		}

		public static void SetAuthorization(this HttpResponse response, string authorization)
		{
			response.Headers[HeaderKeys.Authorization] = authorization;
		}

		public static void SetAuthorizationToken(this HttpResponse response, string token)
		{
			response.Headers[HeaderKeys.Authorization] = ValuePrefix + token;
		}

        public static string GetAuthorizationToken(this HttpResponse response)
        {
            string token = null;

            if (response.HasAuthorization())
            {
                token = response.Headers.GetAuthorizationToken();
            }

            return token;
        }

        public static string GetAuthorizationToken(this HttpRequest request)
		{
			string token = null;

			if (request.HasAuthorization())
			{
                token = request.Headers.GetAuthorizationToken();
			}

			return token;
		}

	    public static string GetAuthorizationToken(this IHeaderDictionary headers)
	    {
            string token = null;

            if (headers.ContainsKey(HeaderKeys.Authorization))
	        {
	            var authorization = (string) headers[HeaderKeys.Authorization];

	            if (authorization.StartsWith(ValuePrefix))
	            {
	                token = authorization.Substring(ValuePrefix.Length);
	            }
	        }

	        return token;
	    }
	}
}
