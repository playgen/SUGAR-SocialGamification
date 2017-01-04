using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	public abstract class AuthorizedController : Controller
	{
		protected readonly IAuthorizationService _authorizationService;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="authorizationService"></param>
		protected AuthorizedController(IAuthorizationService authorizationService)
		{
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Test if the current user has the required claim for a specific game
		/// </summary>
		/// <param name="gameId">Id of the game to test</param>
		/// <returns></returns>
		protected bool AuthorizedGame(int gameId)
		{
			return _authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement) HttpContext.Items[ClaimScope.Game]).Result;
		}

		/// <summary>
		/// Test if the current user has the required claim for a specific group
		/// </summary>
		/// <param name="groupId">Id of the group to test</param>
		/// <returns></returns>
		protected bool AuthorizedGroup(int groupId)
		{
			return _authorizationService.AuthorizeAsync(User, groupId, (AuthorizationRequirement)HttpContext.Items[ClaimScope.Group]).Result;
		}

		/// <summary>
		/// Test if the current user has the required claim for a specific user
		/// </summary>
		/// <param name="userId">Id of the user to test</param>
		/// <returns></returns>
		protected bool AuthorizedUser(int userId)
		{
			return _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items[ClaimScope.User]).Result;
		}

		/// <summary>
		/// Test if the current user has the required claim for a specific account
		/// </summary>
		/// <param name="accountId">Id of the account to test</param>
		/// <returns></returns>
		protected bool AuthorizedAccount(int accountId)
		{
			return _authorizationService.AuthorizeAsync(User, accountId, (AuthorizationRequirement) HttpContext.Items[ClaimScope.Account]).Result;
		}

		/// <summary>
		/// Test if the current user has the required global claim
		/// </summary>
		/// <returns></returns>
		protected bool AuthorizedGlobal()
		{
			return _authorizationService.AuthorizeAsync(User, -1, (AuthorizationRequirement)HttpContext.Items[ClaimScope.Global]).Result;
		}
	}
}
