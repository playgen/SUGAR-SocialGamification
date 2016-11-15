using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Filters;
using PlayGen.SUGAR.Common.Shared.Permissions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates Role specific operations.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class RoleController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly Core.Controllers.RoleController _roleCoreController;

        public RoleController(Core.Controllers.RoleController roleCoreController,
                    IAuthorizationService authorizationService)
        {
            _roleCoreController = roleCoreController;
            _authorizationService = authorizationService;
        }
        /// <summary>
		/// Get a list of all Roles.
		/// 
		/// Example Usage: GET api/role/list
		/// </summary>
		/// <returns>A list of <see cref="RoleResponse"/> that hold Role details.</returns>
		[HttpGet("list")]
        //[ResponseType(typeof(IEnumerable<RoleResponse>))]
        [Authorization(ClaimScope.Global, AuthorizationOperation.Get, AuthorizationOperation.Role)]
        public IActionResult Get()
        {
            if (_authorizationService.AuthorizeAsync(User, 0, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var roles = _roleCoreController.Get();
                var roleContract = roles.ToContractList();
                return new ObjectResult(roleContract);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Create a new Role.
        /// Requires the <see cref="RoleRequest.Name"/> to be unique.
        /// 
        /// Example Usage: POST api/role
        /// </summary>
        /// <param name="newRole"><see cref="RoleRequest"/> object that contains the details of the new Role.</param>
        /// <returns>A <see cref="RoleResponse"/> containing the new Role details.</returns>
        [HttpPost]
        //[ResponseType(typeof(RoleResponse))]
        [ArgumentsNotNull]
        [Authorization(ClaimScope.Global, AuthorizationOperation.Create, AuthorizationOperation.Role)]
        public IActionResult Create([FromBody]RoleRequest newRole)
        {
            if (_authorizationService.AuthorizeAsync(User, 0, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var role = newRole.ToModel();
                _roleCoreController.Create(role);
                var roleContract = role.ToContract();
                return new ObjectResult(roleContract);
            }
            return Unauthorized();
        }

        /// <summary>
		/// Delete Role with the ID provided.
		/// 
		/// Example Usage: DELETE api/role/1
		/// </summary>
		/// <param name="id">Role ID.</param>
		[HttpDelete("{id:int}")]
        [Authorization(ClaimScope.Actor, AuthorizationOperation.Delete, AuthorizationOperation.Role)]
        public void Delete([FromRoute]int id)
        {
            if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                _roleCoreController.Delete(id);
            }
        }
    }
}