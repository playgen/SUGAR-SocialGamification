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
	/// Web Controller that facilitates AccountSource specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	public class AccountSourceController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.AccountSourceController _accountSourceCoreController;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="accountSourceCoreController"></param>
		/// <param name="authorizationService"></param>
		public AccountSourceController(Core.Controllers.AccountSourceController accountSourceCoreController, IAuthorizationService authorizationService)
		{
			_accountSourceCoreController = accountSourceCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Get a list of all AccountSources.
		/// 
		/// Example Usage: GET api/accountSource/list
		/// </summary>
		/// <returns>A list of <see cref="AccountSourceResponse"/> that hold AccountSource details.</returns>
		[HttpGet("list")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Get, AuthorizationEntity.AccountSource)]
		public async Task<IActionResult> Get()
		{
			if ((await _authorizationService.AuthorizeAsync(User, Platform.AllId, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				var accountSources = _accountSourceCoreController.Get();
				var accountSourceContract = accountSources.ToContractList();
				return new ObjectResult(accountSourceContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Get AccountSource that matches the id provided.
		/// </summary>
		/// <param name="id">AccountSource id</param>
		/// <returns><see cref="AccountSourceResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}", Name = "GetByAccountSourceId")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Get, AuthorizationEntity.AccountSource)]
		public async Task<IActionResult> GetById([FromRoute]int id)
		{
			if ((await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				var accountSource = _accountSourceCoreController.Get(id);
				var accountSourceContract = accountSource.ToContract();
				return new ObjectResult(accountSourceContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Create a new AccountSource.
		/// Requires the <see cref="AccountSourceRequest.Token"/> to be unique.
		/// </summary>
		/// <param name="newAccountSource"><see cref="AccountSourceRequest"/> object that contains the details of the new AccountSource.</param>
		/// <returns>A <see cref="AccountSourceResponse"/> containing the new AccountSource details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationAction.Create, AuthorizationEntity.AccountSource)]
		public async Task<IActionResult> Create([FromBody]AccountSourceRequest newAccountSource)
		{
			if ((await _authorizationService.AuthorizeAsync(User, Platform.AllId, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				var accountSource = newAccountSource.ToModel();
				_accountSourceCoreController.Create(accountSource);
				var accountSourceContract = accountSource.ToContract();
				return new ObjectResult(accountSourceContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Update an existing AccountSource.
		/// </summary>
		/// <param name="id">Id of the existing AccountSource.</param>
		/// <param name="accountSource"><see cref="AccountSourceRequest"/> object that holds the details of the AccountSource.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationAction.Update, AuthorizationEntity.AccountSource)]
		// todo refactor accountSource request into AccountSourceUpdateRequest (which requires the Id) and AccountSourceCreateRequest (which has no required Id field) - and remove the Id param from the definition below
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] AccountSourceRequest accountSource)
		{
			if ((await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				var accountSourceModel = accountSource.ToModel();
				accountSourceModel.Id = id;
				_accountSourceCoreController.Update(accountSourceModel);
				return Ok();
			}
			return Forbid();
		}

		/// <summary>
		/// Delete AccountSource with the ID provided.
		/// </summary>
		/// <param name="id">AccountSource ID.</param>
		[HttpDelete("{id:int}")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Delete, AuthorizationEntity.AccountSource)]
		public async Task<IActionResult> Delete([FromRoute]int id)
		{
			if ((await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				_accountSourceCoreController.Delete(id);
				return Ok();
			}
			return Forbid();
		}
	}
}