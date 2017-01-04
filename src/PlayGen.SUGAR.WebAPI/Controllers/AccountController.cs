using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.WebAPI.Attributes;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates account specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class AccountController : AuthorizedController
	{
		private readonly Core.Controllers.IAccountController _accountCoreController;

		/// <summary>
		/// Web API Controlelr for account management
		/// </summary>
		/// <param name="authorizationService"></param>
		/// <param name="accountCoreController"></param>
		public AccountController(IAuthorizationService authorizationService, Core.Controllers.IAccountController accountCoreController) 
			: base(authorizationService)
		{
			_accountCoreController = accountCoreController;
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
		[Authorize("Bearer")]
		[ArgumentsNotNull]
		public IActionResult Create([FromBody] AccountRequest accountRequest)
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
			if (AuthorizedAccount(id)) //TODO: OR authorized global?
			{
				_accountCoreController.Delete(id);
				return Ok();
			}
			return Forbid();
		}
	}
}