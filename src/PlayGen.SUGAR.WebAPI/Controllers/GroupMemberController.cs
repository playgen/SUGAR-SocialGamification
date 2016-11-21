using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates User to Group relationship specific operations.
	/// </summary>
	[Route("api/[controller]")]
    [Authorize("Bearer")]
    public class GroupMemberController : Controller
	{
        private readonly IAuthorizationService _authorizationService;
        private readonly Core.Controllers.GroupMemberController _groupMemberCoreController;

		public GroupMemberController(Core.Controllers.GroupMemberController groupMemberCoreController,
                    IAuthorizationService authorizationService)
		{
			_groupMemberCoreController = groupMemberCoreController;
            _authorizationService = authorizationService;
        }

		/// <summary>
		/// Get a list of all Users that have relationship requests for this <param name="groupId"/>.
		/// 
		/// Example Usage: GET api/groupmember/requests/1
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("requests/{groupId:int}")]
        //[ResponseType(typeof(IEnumerable<ActorResponse>))]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Get, AuthorizationOperation.GroupMemberRequest)]
        public IActionResult GetMemberRequests([FromRoute]int groupId)
		{
            if (_authorizationService.AuthorizeAsync(User, groupId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var users = _groupMemberCoreController.GetMemberRequests(groupId);
                var actorContract = users.ToContractList();
                return new ObjectResult(actorContract);
            }
            return Forbid();
        }

		/// <summary>
		/// Get a list of all Groups that have been sent relationship requests for this <param name="userId"/>.
		/// 
		/// Example Usage: GET api/groupmember/sentrequests/1
		/// </summary>
		/// <param name="userId">ID of the user.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("sentrequests/{userId:int}")]
        //[ResponseType(typeof(IEnumerable<ActorResponse>))]
        [Authorization(ClaimScope.User, AuthorizationOperation.Get, AuthorizationOperation.GroupMemberRequest)]
        public IActionResult GetSentRequests([FromRoute]int userId)
		{
            if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var requests = _groupMemberCoreController.GetSentRequests(userId);
                var actorContract = requests.ToContractList();
                return new ObjectResult(actorContract);
            }
            return Forbid();
        }

		/// <summary>
		/// Get a list of all Users that have relationships with this <param name="groupId"/>.
		/// 
		/// Example Usage: GET api/groupmember/members/1
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("members/{groupId:int}")]
		//[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult GetMembers([FromRoute]int groupId)
		{
			var members = _groupMemberCoreController.GetMembers(groupId);
			var actorContract = members.ToContractList();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Get a list of all Groups that have relationships with this <param name="userId"/>.
		/// 
		/// Example Usage: GET api/groupmember/usergroups/1
		/// </summary>
		/// <param name="userId">ID of the User.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("usergroups/{userId:int}")]
		//[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult GetUserGroups([FromRoute]int userId)
		{
			var groups = _groupMemberCoreController.GetUserGroups(userId);
			var actorContract = groups.ToContractList();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Create a new relationship request between the User and Group.
		/// Requires a relationship between the User and Group to not already exist.
		/// 
		/// Example Usage: POST api/groupmember
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
		/// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
		[HttpPost]
		//[ResponseType(typeof(RelationshipResponse))]
		[ArgumentsNotNull]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.GroupMemberRequest)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Create, AuthorizationOperation.GroupMemberRequest)]
        public IActionResult CreateMemberRequest([FromBody]RelationshipRequest relationship)
		{
            if (_authorizationService.AuthorizeAsync(User, relationship.RequestorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, relationship.AcceptorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result)
            {
                var request = relationship.ToGroupModel();
                _groupMemberCoreController.CreateMemberRequest(relationship.ToGroupModel(), relationship.AutoAccept);
                var relationshipContract = request.ToContract();
                return new ObjectResult(relationshipContract);
            }
            return Forbid();
        }

		/// <summary>
		/// Update an existing relationship request between <param name="relationship.UserId"/> and <param name="relationship.GroupId"/>.
		/// Requires the relationship request to already exist between the User and Group.
		/// 
		/// Example Usage: PUT api/groupmember/request
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		[HttpPut("request")]
		[ArgumentsNotNull]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Update, AuthorizationOperation.GroupMemberRequest)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Update, AuthorizationOperation.GroupMemberRequest)]
        public IActionResult UpdateMemberRequest([FromBody] RelationshipStatusUpdate relationship)
		{
            if (_authorizationService.AuthorizeAsync(User, relationship.RequestorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, relationship.AcceptorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result)
            {
                var relation = new RelationshipRequest
                {
                    RequestorId = relationship.RequestorId,
                    AcceptorId = relationship.AcceptorId
                };
                _groupMemberCoreController.UpdateMemberRequest(relation.ToGroupModel(), relationship.Accepted);
                return Ok();
            }
            return Forbid();
		}

		/// <summary>
		/// Update an existing relationship between <param name="relationship.UserId"/> and <param name="relationship.GroupId"/>.
		/// Requires the relationship to already exist between the User and Group.
		/// 
		/// Example Usage: PUT api/groupmember
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		[HttpPut]
		[ArgumentsNotNull]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Delete, AuthorizationOperation.GroupMemberRequest)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Delete, AuthorizationOperation.GroupMemberRequest)]
        public IActionResult UpdateMember([FromBody] RelationshipStatusUpdate relationship)
		{
            if (_authorizationService.AuthorizeAsync(User, relationship.RequestorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, relationship.AcceptorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result)
            {
                var relation = new RelationshipRequest
                {
                    RequestorId = relationship.RequestorId,
                    AcceptorId = relationship.AcceptorId
                };
                _groupMemberCoreController.UpdateMember(relation.ToGroupModel());
                return Ok();
            }
            return Forbid();
        }
	}
}