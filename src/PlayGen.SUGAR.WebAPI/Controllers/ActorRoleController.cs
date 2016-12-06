using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.WebAPI.Filters;
using System.Linq;
using PlayGen.SUGAR.WebAPI.Attributes;

namespace PlayGen.SUGAR.WebAPI.Controllers
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

		public ActorRoleController(Core.Controllers.ActorRoleController actorRoleCoreController,
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
        /// <returns>A list of <see cref="ActorResponse"/> that hold ActorRole details.</returns>
        [HttpGet("role/{roleId:int}/entity/{entityId:int}")]
        //[ResponseType(typeof(IEnumerable<ActorRoleResponse>))]
        [Authorization(ClaimScope.Global, AuthorizationOperation.Get, AuthorizationOperation.ActorRole)]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Get, AuthorizationOperation.ActorRole)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.ActorRole)]
        public IActionResult GetRoleActors([FromRoute]int roleId, [FromRoute]int entityId)
        {
            if (_authorizationService.AuthorizeAsync(User, -1, (AuthorizationRequirement)HttpContext.Items["GlobalRequirements"]).Result ||
               _authorizationService.AuthorizeAsync(User, entityId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, entityId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
            {
                var actors = _actorRoleCoreController.GetRoleActors(roleId, entityId);
                var actorContract = actors.ToActorContractList();
                return new ObjectResult(actorContract);
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
        //[ResponseType(typeof(IEnumerable<RoleResponse>))]
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
        //[ResponseType(typeof(ActorRoleResponse))]
        [ArgumentsNotNull]
        [Authorization(ClaimScope.Global, AuthorizationOperation.Create, AuthorizationOperation.ActorRole)]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.ActorRole)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.ActorRole)]
        public IActionResult Create([FromBody]ActorRoleRequest newRole)
        {
            if (_authorizationService.AuthorizeAsync(User, -1, (AuthorizationRequirement)HttpContext.Items["GlobalRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, newRole.EntityId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, newRole.EntityId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
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
        [Authorization(ClaimScope.Global, AuthorizationOperation.Delete, AuthorizationOperation.ActorRole)]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Delete, AuthorizationOperation.ActorRole)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Delete, AuthorizationOperation.ActorRole)]
        public IActionResult Delete([FromRoute]int id)
        {
            var actorRole = _actorRoleCoreController.Get(id);
            if (_authorizationService.AuthorizeAsync(User, -1, (AuthorizationRequirement)HttpContext.Items["GlobalRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, actorRole.EntityId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, actorRole.EntityId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
            {
				var role = _roleController.GetById(actorRole.RoleId);
				if (role.Default)
				{
					var roleCount = _actorRoleCoreController.GetRoleActors(actorRole.RoleId, actorRole.EntityId.Value).Count();
					if (roleCount > 1)
					{
						_actorRoleCoreController.Delete(id);
						return Ok();
					}
				} else
				{
					_actorRoleCoreController.Delete(id);
					return Ok();
				}
            }
            return Forbid();
        }
    }  
}
