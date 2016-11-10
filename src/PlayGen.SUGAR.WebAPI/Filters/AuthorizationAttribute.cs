using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.ServerAuthentication;
using PlayGen.SUGAR.ServerAuthentication.Extensions;

namespace PlayGen.SUGAR.WebAPI.Filters
{
	public class AuthorizationAttribute : TypeFilterAttribute
	{
		public AuthorizationAttribute() : base(typeof(AuthorizationAttributeImpl))
		{
		}

		private class AuthorizationAttributeImpl : AuthorizeAttribute, IAuthorizationFilter
		{
			private readonly JsonWebTokenUtility _jsonWebTokenUtility;

			public AuthorizationAttributeImpl(JsonWebTokenUtility jsonWebTokenUtility)
			{
				_jsonWebTokenUtility = jsonWebTokenUtility;
			}

			// See account controller for token header info
			public void OnAuthorization(AuthorizationFilterContext context)
			{
				string token = null;
				var authorized = false;

				token = context.HttpContext.Request.GetAuthorizationToken();

				if (token != null)
				{
					var validity = _jsonWebTokenUtility.GetTokenValidity(token);

					switch (validity)
					{
						case TokenValidity.Valid:
							authorized = true;
							break;

						case TokenValidity.Expired:
							// TODO this is the point to re-issue token if expired
							break;
					}
				}

				if (!authorized)
				{
					context.Result = new StatusCodeResult((int) HttpStatusCode.Unauthorized);
				}
			}
		}
	}
}