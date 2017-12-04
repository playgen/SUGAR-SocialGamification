using System;
using System.Linq;
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
	/// Web Controller that facilitates ActorRole specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class ActorRoleController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.ActorRoleController _actorRoleCoreController;
		private readonly Core.Controllers.ActorClaimController _actorClaimController;
		private readonly Core.Controllers.RoleClaimController _roleClaimController;
		private readonly Core.Controllers.RoleController _roleController;

		public ActorRoleController(
			Core.Controllers.ActorRoleController actorRoleCoreController, 
			Core.Controllers.ActorClaimController actorClaimController, 
			Core.Controllers.RoleClaimController roleClaimController, 
			Core.Controllers.RoleController roleController, 
			IAuthorizationService authorizationService)
		{
			_actorRoleCoreController = actorRoleCoreController;
			_actorClaimController = actorClaimController;
			_roleClaimController = roleClaimController;
			_roleController = roleController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Get a list of all Actors for this Role and Entity.
		/// 
		/// Example Usage: GET api/actorrole/role/1/entity/1
		/// </summary>
		/// <returns>A list of <see cref="ActorResponse"/> that hold Actor details.</returns>
		[HttpGet("role/{roleId:int}/entity/{entityId:int}")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Get, AuthorizationEntity.ActorRole)]
		[Authorization(ClaimScope.Group, AuthorizationAction.Get, AuthorizationEntity.ActorRole)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.ActorRole)]
		public async Task<IActionResult> GetRoleActors([FromRoute]int roleId, [FromRoute]int entityId)
		{
			var role = _roleController.GetById(roleId);

			if (role.ClaimScope == ClaimScope.Global)
			{
				entityId = Platform.EntityId;
			}

			if ((await _authorizationService.AuthorizeAsync(User, entityId, HttpContext.ScopeItems(role.ClaimScope))).Succeeded)
			{
				var actors = _actorRoleCoreController.GetRoleActors(roleId, entityId);
				var actorContract = actors.ToActorContractList();
				return new ObjectResult(actorContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Get a list of all Roles for this Actor, ClaimScope and Entity.
		/// 
		/// Example Usage: GET api/actorrole/actor/1/entity/1/claimscope/game
		/// </summary>
		/// <returns>A list of <see cref="RoleResponse"/> that hold Role details.</returns>
		[HttpGet("actor/{actorId:int}/entity/{entityId:int}/claimscope/{scopeName}")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Get, AuthorizationEntity.ActorRole)]
		[Authorization(ClaimScope.Group, AuthorizationAction.Get, AuthorizationEntity.ActorRole)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.ActorRole)]
		public async Task<IActionResult> GetActorEntityRoles([FromRoute]int actorId, [FromRoute]int entityId, [FromRoute]string scopeName)
		{
			if (Enum.TryParse(scopeName, true, out ClaimScope claimScope))
			{
				if ((await _authorizationService.AuthorizeAsync(User, entityId, HttpContext.ScopeItems(claimScope))).Succeeded)
				{
					var roles = _actorRoleCoreController.GetActorRolesForEntity(actorId, entityId, claimScope).Distinct().ToList();
					var roleContract = roles.ToContractList();
					return new ObjectResult(roleContract);
				}
				return Forbid();
			}
			return Forbid();
		}

		/// <summary>
		/// Get a list of all Roles this Actor has control over.
		/// 
		/// Example Usage: GET api/actorrole/controlled
		/// </summary>
		/// <returns>A list of <see cref="RoleResponse"/> that hold Role details.</returns>
		[HttpGet("controlled")]
		public IActionResult GetControlled()
		{
			var roles = _actorRoleCoreController.GetControlled(int.Parse(User.Identity.Name));
			var roleContract = roles.ToContractList();
			return new ObjectResult(roleContract);
		}

		/// <summary>
		/// Create a new ActorRole.
		/// 
		/// Example Usage: POST api/actorrole
		/// </summary>
		/// <param name="newRole"><see cref="ActorRoleRequest"/> object that contains the details of the new ActorRole.</param>
		/// <returns>A <see cref="ActorRoleResponse"/> containing the new ActorRole details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationAction.Create, AuthorizationEntity.ActorRole)]
		[Authorization(ClaimScope.Group, AuthorizationAction.Create, AuthorizationEntity.ActorRole)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Create, AuthorizationEntity.ActorRole)]
		public async Task<IActionResult> Create([FromBody]ActorRoleRequest newRole)
		{
			var newRoleInfo = _roleController.GetById(newRole.RoleId);
			if (newRoleInfo.ClaimScope == ClaimScope.Global)
			{
				newRole.EntityId = Platform.EntityId;
			}
			if ((await _authorizationService.AuthorizeAsync(User, newRole.EntityId, HttpContext.ScopeItems(newRoleInfo.ClaimScope))).Succeeded)
			{
				var claimScope = _roleController.GetById(newRole.RoleId).ClaimScope;
				var creatorClaims = _actorClaimController.GetActorClaimsForEntity(int.Parse(User.Identity.Name), newRole.EntityId, claimScope).Select(c => c.Id).ToList();
				var newClaims = _roleClaimController.GetClaimsByRole(newRole.RoleId).Select(c => c.Id);
				if (newClaims.All(nc => creatorClaims.Contains(nc)))
				{
					var role = newRole.ToModel();
					_actorRoleCoreController.Create(role);
					var roleContract = role.ToContract();
					return new ObjectResult(roleContract);
				}
			}
			return Forbid();
		}

		/// <summary>
		/// Delete ActorRole with the ID provided.
		/// 
		/// Example Usage: DELETE api/actorrole/1
		/// </summary>
		/// <param name="id">ActorRole ID.</param>
		[HttpDelete("{id:int}")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Delete, AuthorizationEntity.ActorRole)]
		[Authorization(ClaimScope.Group, AuthorizationAction.Delete, AuthorizationEntity.ActorRole)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Delete, AuthorizationEntity.ActorRole)]
		public async Task<IActionResult> Delete([FromRoute]int id)
		{
			var actorRole = _actorRoleCoreController.Get(id);
			var role = _roleController.GetById(actorRole.RoleId);
			if ((await _authorizationService.AuthorizeAsync(User, actorRole.EntityId, HttpContext.ScopeItems(role.ClaimScope))).Succeeded)
			{
				if (role.Default)
				{
					var roleCount = _actorRoleCoreController.GetRoleActors(actorRole.RoleId, actorRole.EntityId).Count;
					if (roleCount > 1)
					{
						_actorRoleCoreController.Delete(id);
						return Ok();
					}
				}
				else
				{
					_actorRoleCoreController.Delete(id);
					return Ok();
				}
			}
			return Forbid();
		}
	}  
}
