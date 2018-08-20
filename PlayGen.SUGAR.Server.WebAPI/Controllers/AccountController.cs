using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates account specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class AccountController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.AccountController _accountCoreController;

		public AccountController(Core.Controllers.AccountController accountCoreController, IAuthorizationService authorizationService)
		{
			_accountCoreController = accountCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Register a new account and creates an associated user.
		/// Requires the <see cref="AccountRequest.Name"/> to be unique.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the details of the new Account.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the new Account details.</returns>
		[HttpPost("create")]
		[ArgumentsNotNull]
		[AllowWithoutSession]
		public IActionResult Create([FromBody] AccountRequest accountRequest)
		{
			var account = accountRequest.ToModel();
			account = _accountCoreController.Create(account, accountRequest.SourceToken);
			var response = account.ToContract();
			return new ObjectResult(response);
		}

		/// <summary>
		/// Delete Account with the ID provided.
		/// </summary>
		/// <param name="id">Account ID.</param>
		[HttpDelete("{id:int}")]
		[Authorize("Bearer")]
		[Authorization(ClaimScope.Account, AuthorizationAction.Delete, AuthorizationEntity.Account)]
		public async Task<IActionResult> Delete([FromRoute]int id)
		{
			if ((await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.Account))).Succeeded)
			{
				_accountCoreController.Delete(id);
				return Ok();
			}
			return Forbid();
		}
	}
}