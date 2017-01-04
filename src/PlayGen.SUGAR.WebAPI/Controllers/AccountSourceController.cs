using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.WebAPI.Attributes;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates AccountSource specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class AccountSourceController : AuthorizedController
	{
		private readonly Core.Controllers.IAccountSourceController _accountSourceCoreController;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="authorizationService"></param>
		/// <param name="accountSourceController"></param>
		public AccountSourceController(IAuthorizationService authorizationService, Core.Controllers.IAccountSourceController accountSourceController) 
			: base(authorizationService)
		{
			_accountSourceCoreController = accountSourceController;
		}

		/// <summary>
		/// Get a list of all AccountSources.
		/// 
		/// Example Usage: GET api/accountSource/list
		/// </summary>
		/// <returns>A list of <see cref="AccountSourceResponse"/> that hold AccountSource details.</returns>
		[HttpGet("list")]
		[Authorization(ClaimScope.Global, AuthorizationOperation.Get, AuthorizationOperation.AccountSource)]
		public IActionResult List()
		{
			if (AuthorizedGlobal())
			{
				var accountSources = _accountSourceCoreController.Get();
				var accountSourceContract = accountSources.ToContractList();
				return new ObjectResult(accountSourceContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Get AccountSource that matches <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/accountSource/findbyid/1
		/// </summary>
		/// <param name="id">AccountSource id</param>
		/// <returns><see cref="AccountSourceResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}", Name = "GetByAccountSourceId")]
		[Authorization(ClaimScope.Global, AuthorizationOperation.Get, AuthorizationOperation.AccountSource)]
		public IActionResult GetById([FromRoute]int id)
		{
			if (AuthorizedGlobal())
			{
				AccountSource accountSource;

				if (_accountSourceCoreController.TryGet(id, out accountSource))
				{
					var accountSourceContract = accountSource.ToContract();
					return new ObjectResult(accountSourceContract);
				}
				return NoContent();
			}
			return Forbid();
		}

		/// <summary>
		/// Create a new AccountSource.
		/// Requires the <see cref="AccountSourceRequest.Name"/> to be unique.
		/// 
		/// Example Usage: POST api/accountSource
		/// </summary>
		/// <param name="newAccountSource"><see cref="AccountSourceRequest"/> object that contains the details of the new AccountSource.</param>
		/// <returns>A <see cref="AccountSourceResponse"/> containing the new AccountSource details.</returns>
		[HttpPost]
		//[ResponseType(typeof(AccountSourceResponse))]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationOperation.Create, AuthorizationOperation.AccountSource)]
		public IActionResult Create([FromBody]AccountSourceRequest newAccountSource)
		{
			if (AuthorizedGlobal())
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
		/// 
		/// Example Usage: PUT api/accountSource/update/1
		/// </summary>
		/// <param name="id">Id of the existing AccountSource.</param>
		/// <param name="accountSource"><see cref="AccountSourceRequest"/> object that holds the details of the AccountSource.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationOperation.Update, AuthorizationOperation.AccountSource)]
		// todo refactor accountSource request into AccountSourceUpdateRequest (which requires the Id) and AccountSourceCreateRequest (which has no required Id field) - and remove the Id param from the definition below
		public IActionResult Update([FromRoute] int id, [FromBody] AccountSourceRequest accountSource)
		{
			if (AuthorizedGlobal())
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
		/// 
		/// Example Usage: DELETE api/accountSource/1
		/// </summary>
		/// <param name="id">AccountSource ID.</param>
		[HttpDelete("{id:int}")]
		[Authorization(ClaimScope.Global, AuthorizationOperation.Delete, AuthorizationOperation.AccountSource)]
		public IActionResult Delete([FromRoute]int id)
		{
			if (AuthorizedGlobal())
			{
				_accountSourceCoreController.Delete(id);
				return Ok();
			}
			return Forbid();
		}
	}
}