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
        [Authorization(ClaimScope.Actor, AuthorizationOperation.Get, AuthorizationOperation.ActorRole)]
        public IActionResult GetActorRoles([FromRoute]int id)
        {
            if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
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
        public IActionResult GetRoleActors([FromRoute]int roleId, [FromRoute]int entityId)
        {
            if (_authorizationService.AuthorizeAsync(User, 0, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var actors = _actorRoleCoreController.GetRoleActors(roleId, entityId);
                var actorContract = actors.ToContractList();
                return new ObjectResult(actorContract);
            }
            return Unauthorized();
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
        public IActionResult Create([FromBody]ActorRoleRequest newRole)
        {
            if (_authorizationService.AuthorizeAsync(User, 0, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var role = newRole.ToModel();
                _actorRoleCoreController.Create(role);
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
        public IActionResult Delete([FromRoute]int id)
        {
            if (_authorizationService.AuthorizeAsync(User, 0, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                _actorRoleCoreController.Delete(id);
                return Ok();
            }
            return Unauthorized();
        }
    }  
}
