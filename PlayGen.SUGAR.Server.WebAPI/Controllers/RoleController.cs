using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.WebAPI.Attributes;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates Role specific operations.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    [ValidateSession]
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
            if (_authorizationService.AuthorizeAsync(User, -1, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var roles = _roleCoreController.Get();
                var roleContract = roles.ToContractList();
                return new ObjectResult(roleContract);
            }
            return Forbid();
        }

		/// <summary>
		/// Get a list of all Roles for the scope with this name.
		/// 
		/// Example Usage: GET api/role/scope/game
		/// </summary>
		/// <returns>A list of <see cref="RoleResponse"/> that hold Role details.</returns>
		[HttpGet("scope/{name}")]
		//[ResponseType(typeof(IEnumerable<RoleResponse>))]
		[Authorization(ClaimScope.Global, AuthorizationOperation.Get, AuthorizationOperation.Role)]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Get, AuthorizationOperation.Role)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.Role)]
		public IActionResult GetByScope([FromRoute]string name)
		{
			ClaimScope claimScope;
			if (Enum.TryParse(name, true, out claimScope))
			{
				if (_authorizationService.AuthorizeAsync(User, claimScope, (AuthorizationRequirement)HttpContext.Items[claimScope + "Requirements"]).Result)
				{
					var roles = _roleCoreController.GetByScope(claimScope);
					var roleContract = roles.ToContractList();
					return new ObjectResult(roleContract);
				}
				return Forbid();
			}
			return Forbid();
		}

		/// <summary>
		/// Get default Role for the scope with this name.
		/// 
		/// Example Usage: GET api/role/scopedefault/game
		/// </summary>
		/// <returns>A <see cref="RoleResponse"/> that holds Role details.</returns>
		[HttpGet("scopedefault/{name}")]
		//[ResponseType(typeof(RoleRespons>))]
		[Authorization(ClaimScope.Global, AuthorizationOperation.Get, AuthorizationOperation.Role)]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Get, AuthorizationOperation.Role)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.Role)]
		public IActionResult GetDefaultForScope([FromRoute]string name)
		{
			ClaimScope claimScope;
			if (Enum.TryParse(name, true, out claimScope))
			{
				if (_authorizationService.AuthorizeAsync(User, claimScope, (AuthorizationRequirement)HttpContext.Items[claimScope + "Requirements"]).Result)
				{
					var role = _roleCoreController.GetDefaultForScope(claimScope);
					var roleContract = role.ToContract();
					return new ObjectResult(roleContract);
				}
				return Forbid();
			}
			return Forbid();
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
        [Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.Role)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.Role)]
        public IActionResult Create([FromBody]RoleRequest newRole)
        {
            if (_authorizationService.AuthorizeAsync(User, newRole.ClaimScope, (AuthorizationRequirement)HttpContext.Items[newRole.ClaimScope + "Requirements"]).Result)
            {
                var role = newRole.ToModel();
                _roleCoreController.Create(role, int.Parse(User.Identity.Name));
                var roleContract = role.ToContract();
                return new ObjectResult(roleContract);
            }
            return Forbid();
        }

        /// <summary>
		/// Delete Role with the ID provided.
		/// 
		/// Example Usage: DELETE api/role/1
		/// </summary>
		/// <param name="id">Role ID.</param>
		[HttpDelete("{id:int}")]
        [Authorization(ClaimScope.Role, AuthorizationOperation.Delete, AuthorizationOperation.Role)]
        public IActionResult Delete([FromRoute]int id)
        {
            if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
				
				var role = _roleCoreController.GetById(id);
				if (!role.Default)
				{
					//Todo: May need to check claims don't become inaccessible due to deletion
					_roleCoreController.Delete(id);
					return Ok();
				}
            }
            return Forbid();
        }
    }
}