using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authentication;
using PlayGen.SUGAR.Server.Core.Sessions;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates session specific operations.
	/// </summary>
	[Route("api")]
	public class SessionController : Controller
	{
		private readonly TokenController _tokenController;
		private readonly Core.Controllers.AccountController _accountCoreController;
		private readonly Core.Controllers.AccountSourceController _accountSourceController;
		private readonly SessionTracker _sessionTracker;

		public SessionController(
			Core.Controllers.AccountController accountCoreController,
			Core.Controllers.AccountSourceController accountSourceController,
			TokenController tokenController,
			SessionTracker sessionTracker)
		{
			_accountCoreController = accountCoreController;
			_accountSourceController = accountSourceController;
			_sessionTracker = sessionTracker;
			_tokenController = tokenController;
		}

		/// <summary>
		/// Logs in an account based on the name and password combination.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// 
		/// Example Usage: POST api/loginplatform
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		[HttpPost("loginplatform")]
		[ArgumentsNotNull]
		[AllowWithoutSession]
		public IActionResult Login([FromBody]AccountRequest accountRequest)
		{
			var account = accountRequest.ToModel();

			account = _accountCoreController.Authenticate(account, accountRequest.SourceToken);

			var session = _sessionTracker.StartSession(Platform.GlobalId, account.User.Id); // todo should this be moved to the login core controller where we can evaluate if the user is allowed to login to the specific game?
			_tokenController.IssueToken(HttpContext, session);

			var response = account.ToContract();
			return new ObjectResult(response);
		}

		/// <summary>
		/// Logs in an account based on the name and password combination.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// 
		/// Example Usage: POST api/1/logingame
		/// </summary>
		/// <param name="gameId">Optional Id of the game the account is logging in for.</param>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		[HttpPost("{gameId:int}/logingame")]
		[ArgumentsNotNull]
		[AllowWithoutSession]
		public IActionResult Login([FromRoute]int gameId, [FromBody]AccountRequest accountRequest)
		{
			// todo check if has permission to login for specified game
			var account = accountRequest.ToModel();

			account = _accountCoreController.Authenticate(account, accountRequest.SourceToken);

			var session = _sessionTracker.StartSession(gameId, account.User.Id); // todo should this be moved to the login core controller where we can evaluate if the user is allowed to login to the specific game?
			_tokenController.IssueToken(HttpContext, session);

			var response = account.ToContract();
			return new ObjectResult(response);
		}

		// HACK TEST
		public class Token
		{
			public string TokenString;
		}

		/// <summary>
		/// Login to the game using a provided token
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		[HttpPost("logintoken")]
		[ArgumentsNotNull]
		[AllowWithoutSession]
		public IActionResult LoginToken([FromBody]Token token)
		{
			// todo check if has permission to login for specified game
			var userId = _tokenController.ValidateToken(HttpContext, token.TokenString);

			var account = _accountCoreController.GetByUser(userId);
			var accountSourceToken = _accountSourceController.Get(account.AccountSourceId).Token;
			
			account = _accountCoreController.AuthenticateToken(account, accountSourceToken);

			var response = account.ToContract();
			return new ObjectResult(response);
		}


		/// <summary>
		/// Creates a new account and login that account.
		/// 
		/// Example Usage: POST api/createandloginplatform
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		[HttpPost("createandloginplatform")]
		[ArgumentsNotNull]
		[AllowWithoutSession]
		public IActionResult CreateAndLogin([FromBody] AccountRequest accountRequest)
		{
			var account = accountRequest.ToModel();
			_accountCoreController.Create(account, accountRequest.SourceToken);

			var result = Login(Platform.GlobalId, accountRequest);
			return result;
		}

		/// <summary>
		/// Creates a new account and login that account.
		/// 
		/// Example Usage: POST api/1/createandlogingame
		/// </summary>
		/// <param name="gameId">Optional Id of the game the account is logging in for.</param>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		[HttpPost("{gameId:int}/createandlogingame")]
		[ArgumentsNotNull]
		[AllowWithoutSession]
		public IActionResult CreateAndLogin([FromRoute]int gameId, [FromBody] AccountRequest accountRequest)
		{
			var account = accountRequest.ToModel();
			_accountCoreController.Create(account, accountRequest.SourceToken);

			var result = Login(gameId, accountRequest);
			return result;
		}

		/// <summary>
		/// Heartbeat method to keep the specific session alive.
		/// Calling it within a server defined interval will notify the server that the
		/// session is still active.
		/// </summary>
		[HttpGet("heartbeat")]
		[Authorize("Bearer")]
		public IActionResult Heartbeat()
		{
			return new ObjectResult(null);
		}

		/// <summary>
		/// Logs out the currently logged in account, ending it's session and removing the 
		/// authorization token.
		/// </summary>
		/// <returns></returns>
		[HttpGet("logout")]
		[Authorize("Bearer")]
		public IActionResult Logout()
		{
			var sessionId = HttpContext.GetSessionId();
			_sessionTracker.EndSession(sessionId);
			_tokenController.RevokeToken(HttpContext);

			return Ok();
		}
	}
}
