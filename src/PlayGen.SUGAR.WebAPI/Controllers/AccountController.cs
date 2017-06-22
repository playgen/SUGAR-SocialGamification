using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.WebAPI.Attributes;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates account specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class AccountController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.AccountController _accountCoreController;

		public AccountController(Core.Controllers.AccountController accountCoreController,
			IAuthorizationService authorizationService)
		{
			_accountCoreController = accountCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Register a new account and creates an associated user.
		/// Requires the <see cref="AccountRequest.Name"/> to be unique.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// 
		/// Example Usage: POST api/account/create
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the details of the new Account.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the new Account details.</returns>
		[HttpPost("create")]
		[HttpPost("{gameId:int}/create")]
		[ArgumentsNotNull]
		public IActionResult Create([FromRoute]int? gameId, [FromBody] AccountRequest accountRequest)
		{
			var account = accountRequest.ToModel();
			account = _accountCoreController.Create(account, accountRequest.SourceToken);
			var response = account.ToContract();
			return new ObjectResult(response);
		}

		/// <summary>
		/// Delete Account with the ID provided.
		/// 
		/// Example Usage: DELETE api/account/1
		/// </summary>
		/// <param name="id">Account ID.</param>
		[HttpDelete("{id:int}")]
		[Authorize("Bearer")]
		[Authorization(ClaimScope.Account, AuthorizationOperation.Delete, AuthorizationOperation.Account)]
		[ValidateSession]
		public IActionResult Delete([FromRoute]int id)
		{
			if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
			{
				_accountCoreController.Delete(id);
				return Ok();
			}
			return Forbid();
		}
	}
}