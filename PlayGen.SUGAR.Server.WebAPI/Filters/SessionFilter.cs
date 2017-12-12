using System;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayGen.SUGAR.Server.Authentication.Extensions;
using PlayGen.SUGAR.Server.Core.Sessions;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Exceptions;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Filters
{
	/// <summary>
	/// Ensures the requestor has a valid session.
	/// </summary>
	public class SessionFilter : IAuthorizationFilter
	{
		private readonly SessionTracker _sessionTracker;

		public SessionFilter(SessionTracker sessionTracker)
		{
			_sessionTracker = sessionTracker;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (context.ActionDescriptor.GetCustomMethodAttribute<AllowWithoutSession>() == null)
			{
				if (!context.HttpContext.Request.Headers.TryGetSessionId(out var sessionId))
				{
					throw new InvalidSessionException("No \"SessionId\" set in the token's claims.");
				}
				if (!_sessionTracker.IsActive(sessionId))
				{
					throw new InvalidSessionException($"Session with id \"{sessionId}\" is not active.");
				}
				_sessionTracker.SetLastActive(sessionId, DateTime.UtcNow);
			}
		}
	}
}
