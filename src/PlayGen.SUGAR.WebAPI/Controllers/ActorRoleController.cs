using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates ActorRole specific operations.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class ActorRoleController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly Core.Controllers.ActorRoleController _actorRoleCoreController;

        public ActorRoleController(Core.Controllers.ActorRoleController actorRoleCoreController,
                    IAuthorizationService authorizationService)
        {
            _actorRoleCoreController = actorRoleCoreController;
            _authorizationService = authorizationService;
        }
        /// <summary>
        /// Get a list of all Roles for this Actor.
        /// 
        /// Example Usage: GET api/actorrole/actor/1
        /// </summary>
        /// <returns>A list of <see cref="ActorRoleResponse"/> that hold ActorRole details.</returns>
        [HttpGet("actor/{id:int}")]
        //[ResponseType(typeof(IEnumerable<ActorRoleResponse>))]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Get, AuthorizationOperation.ActorRole)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Get, AuthorizationOperation.ActorRole)]
        public IActionResult GetActorRoles([FromRoute]int id)
        {
            if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result)
            {
                var roles = _actorRoleCoreController.GetActorRoles(id);
                var roleContract = roles.ToContractList();
                return new ObjectResult(roleContract);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Get a list of all Actors for this Role and Entity.
        /// 
        /// Example Usage: GET api/actorrole/role/1/entity/1
        /// </summary>
        /// <returns>A list of <see cref="ActorRoleResponse"/> that hold ActorRole details.</returns>
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
                var actorContract = actors.ToContractList();
                return new ObjectResult(actorContract);
            }
            return Unauthorized();
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
                var role = newRole.ToModel();
                _actorRoleCoreController.Create(role, int.Parse(User.Identity.Name));
                var roleContract = role.ToContract();
                return new ObjectResult(roleContract);
            }
            return Unauthorized();
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
                _actorRoleCoreController.Delete(id, int.Parse(User.Identity.Name));
                return Ok();
            }
            return Unauthorized();
        }
    }  
}
