using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.Core.Authorization;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates RoleClaim specific operations.
	/// </summary>
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class RoleClaimController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.RoleClaimController _roleClaimCoreController;
		private readonly ClaimController _claimController;
		private readonly Core.Controllers.RoleController _roleController;
		private readonly Core.Controllers.ActorClaimController _actorClaimController;

		public RoleClaimController(Core.Controllers.RoleClaimController roleClaimCoreController, ClaimController claimController, Core.Controllers.RoleController roleController, Core.Controllers.ActorClaimController actorClaimController, IAuthorizationService authorizationService)
		{
			_roleClaimCoreController = roleClaimCoreController;
			_claimController = claimController;
			_roleController = roleController;
			_actorClaimController = actorClaimController;
			_authorizationService = authorizationService;
		}
		/// <summary>
		/// Get a list of all Claims for this Role.
		/// 
		/// Example Usage: GET api/roleclaim/role/1
		/// </summary>
		/// <returns>A list of <see cref="ClaimResponse"/> that hold Claim details.</returns>
		[HttpGet("role/{id:int}")]
		[Authorization(ClaimScope.Role, AuthorizationAction.Get, AuthorizationEntity.RoleClaim)]
		public async Task<IActionResult> GetRoleClaims([FromRoute]int id)
		{
			if (await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.Role)))
			{
				var roles = _roleClaimCoreController.GetClaimsByRole(id);
				var roleContract = roles.ToContractList();
				return new ObjectResult(roleContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Create a new RoleClaim.
		/// 
		/// Example Usage: POST api/roleclaim
		/// </summary>
		/// <param name="newRoleClaim"><see cref="RoleClaimRequest"/> object that contains the details of the new RoleClaim.</param>
		/// <returns>A <see cref="RoleClaimResponse"/> containing the new RoleClaim details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Role, AuthorizationAction.Create, AuthorizationEntity.RoleClaim)]
		public async Task<IActionResult> Create([FromBody]RoleClaimRequest newRoleClaim)
		{
			if (await _authorizationService.AuthorizeAsync(User, newRoleClaim.RoleId, HttpContext.ScopeItems(ClaimScope.Role)))
			{
				var role = _roleController.GetById(newRoleClaim.RoleId.Value);
				if (!role.Default)
				{
					var claimScope = _claimController.Get(newRoleClaim.ClaimId.Value).ClaimScope;
					if (role.ClaimScope == claimScope)
					{
						var claims = _actorClaimController.GetActorClaims(int.Parse(User.Identity.Name)).Select(c => c.ClaimId);
						if (claims.Contains(newRoleClaim.ClaimId.Value))
						{
							var roleClaim = newRoleClaim.ToModel();
							_roleClaimCoreController.Create(roleClaim);
							var roleContract = roleClaim.ToContract();
							return new ObjectResult(roleContract);
						}
					}
				}
			}
			return Forbid();
		}

		/// <summary>
		/// Delete RoleClaim with the ID provided.
		/// 
		/// Example Usage: DELETE api/roleclaim/role/1/claim/1
		/// </summary>
		/// <param name="roleId">Role ID.</param>
		/// <param name="claimId">Claim ID.</param>
		[HttpDelete("role/{roleId:int}/claim/{claimId:int}")]
		[Authorization(ClaimScope.Role, AuthorizationAction.Delete, AuthorizationEntity.RoleClaim)]
		public async Task<IActionResult> Delete([FromRoute]int roleId, [FromRoute]int claimId)
		{
			if (await _authorizationService.AuthorizeAsync(User, roleId, HttpContext.ScopeItems(ClaimScope.Role)))
			{
				var role = _roleController.GetById(roleId);
				if (!role.Default)
				{
					//Todo: May need to check claims don't become inaccessible due to deletion
					_roleClaimCoreController.Delete(roleId, claimId);
					return Ok();
				}
			}
			return Forbid();
		}
	}
}
