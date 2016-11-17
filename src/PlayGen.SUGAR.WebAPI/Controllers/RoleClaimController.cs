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
    /// Web Controller that facilitates RoleClaim specific operations.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class RoleClaimController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly Core.Controllers.RoleClaimController _roleClaimCoreController;

        public RoleClaimController(Core.Controllers.RoleClaimController roleClaimCoreController,
                    IAuthorizationService authorizationService)
        {
            _roleClaimCoreController = roleClaimCoreController;
            _authorizationService = authorizationService;
        }
        /// <summary>
        /// Get a list of all Claims for this Role.
        /// 
        /// Example Usage: GET api/roleclaim/role/1
        /// </summary>
        /// <returns>A list of <see cref="ClaimResponse"/> that hold Claim details.</returns>
        [HttpGet("role/{id:int}")]
        //[ResponseType(typeof(IEnumerable<ClaimResponse>))]
        [Authorization(ClaimScope.Global, AuthorizationOperation.Get, AuthorizationOperation.RoleClaim)]
        public IActionResult GetRoleClaims([FromRoute]int id)
        {
            if (_authorizationService.AuthorizeAsync(User, 0, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var roles = _roleClaimCoreController.GetClaimsByRole(id);
                var roleContract = roles.ToContractList();
                return new ObjectResult(roleContract);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Create a new RoleClaim.
        /// 
        /// Example Usage: POST api/roleclaim
        /// </summary>
        /// <param name="newRoleClaim"><see cref="RoleClaimRequest"/> object that contains the details of the new RoleClaim.</param>
        /// <returns>A <see cref="RoleClaimResponse"/> containing the new RoleClaim details.</returns>
        [HttpPost]
        //[ResponseType(typeof(RoleClaimResponse))]
        [ArgumentsNotNull]
        [Authorization(ClaimScope.Role, AuthorizationOperation.Create, AuthorizationOperation.RoleClaim)]
        public IActionResult Create([FromBody]RoleClaimRequest newRoleClaim)
        {
            if (_authorizationService.AuthorizeAsync(User, newRoleClaim.RoleId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var roleClaim = newRoleClaim.ToModel();
                _roleClaimCoreController.Create(roleClaim, int.Parse(User.Identity.Name));
                var roleContract = roleClaim.ToContract();
                return new ObjectResult(roleContract);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Delete RoleClaim with the ID provided.
        /// 
        /// Example Usage: DELETE api/roleclaim/role/1/claim/1
        /// </summary>
        /// <param name="roleId">Role ID.</param>
        /// <param name="claimId">Claim ID.</param>
        [HttpDelete("role/{roleId:int}/claim/{claimId:int}")]
        [Authorization(ClaimScope.Role, AuthorizationOperation.Delete, AuthorizationOperation.RoleClaim)]
        public IActionResult Delete([FromRoute]int roleId, [FromRoute]int claimId)
        {
            if (_authorizationService.AuthorizeAsync(User, roleId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                _roleClaimCoreController.Delete(roleId, claimId);
                return Ok();
            }
            return Unauthorized();
        }
    }
}
