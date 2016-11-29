using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Core.Sessions;
using PlayGen.SUGAR.ServerAuthentication;
using PlayGen.SUGAR.WebAPI.Attributes;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates session specific operations.
	/// </summary>
	[Route("api")]
	public class SessionController : Controller
	{
        private readonly TokenController _tokenController;
        private readonly Core.Controllers.AccountController _accountCoreController;
        private readonly SessionTracker _sessionTracker;

        public SessionController(Core.Controllers.AccountController accountCoreController,
            TokenController tokenController,
            SessionTracker sessionTracker)
        {
            _accountCoreController = accountCoreController;
            _sessionTracker = sessionTracker;
            _tokenController = tokenController;
        }
        
		/// <summary>
		/// Logs in an account based on the name and password combination.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// 
		/// Example Usage: POST api/account/login
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		[HttpPost("login")]
		[HttpPost("{gameId:int}/login")]
		[ArgumentsNotNull]
		public IActionResult Login([FromRoute]int? gameId, [FromBody]AccountRequest accountRequest)
		{
			// todo check if has permission to login for specified game
			var account = accountRequest.ToModel();

			account = _accountCoreController.Authenticate(account, accountRequest.SourceToken);

			var session = _sessionTracker.StartSession(gameId, account.User.Id); // todo should this be moved to the login core controller where we can evaluate if the user is allowed to login to the specific game?
			_tokenController.IssueToken(HttpContext, session);

			var response = account.ToContract();
			return new ObjectResult(response);
		}

        // todo comment
        [HttpPost("createandlogin")]
        [HttpPost("{gameId:int}/createandlogin")]
        [ArgumentsNotNull]
        public IActionResult CreateAndLogin([FromRoute]int? gameId, [FromBody] AccountRequest accountRequest)
        {
            var account = accountRequest.ToModel();
            _accountCoreController.Create(account, accountRequest.SourceToken);

            var result = Login(gameId, accountRequest);
            return result;
        }

        [HttpGet("heartbeat")]
        [Authorize("Bearer")]
        [ValidateSession]
        public IActionResult Heartbeat()
		{
			return new ObjectResult(null);
		}

		// todo comment
		[HttpGet("logout")]
		[Authorize("Bearer")]
        [ValidateSession]
        public IActionResult Logout()
		{
			var sessionId = HttpContext.GetSessionId();
			_sessionTracker.EndSession(sessionId);
			_tokenController.RevokeToken(HttpContext);

			return Ok();
		}
	}
}
