using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates User to Group relationship specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorization]
	public class GroupMemberController : Controller
	{
		private readonly Core.Controllers.GroupMemberController _groupMemberCoreController;

		public GroupMemberController(Core.Controllers.GroupMemberController groupMemberCoreController)
		{
			_groupMemberCoreController = groupMemberCoreController;
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
		public IActionResult GetMemberRequests([FromRoute]int groupId)
		{
			var users = _groupMemberCoreController.GetMemberRequests(groupId);
			var actorContract = users.ToContractList();
			return new ObjectResult(actorContract);
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
		public IActionResult GetSentRequests([FromRoute]int userId)
		{
			var requests = _groupMemberCoreController.GetSentRequests(userId);
			var actorContract = requests.ToContractList();
			return new ObjectResult(actorContract);
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
		public IActionResult CreateMemberRequest([FromBody]RelationshipRequest relationship)
		{
			var request = relationship.ToGroupModel();
			_groupMemberCoreController.CreateMemberRequest(relationship.ToGroupModel(), relationship.AutoAccept);
			var relationshipContract = request.ToContract();
			return new ObjectResult(relationshipContract);
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
		public void UpdateMemberRequest([FromBody] RelationshipStatusUpdate relationship)
		{
			var relation = new RelationshipRequest {
				RequestorId = relationship.RequestorId,
				AcceptorId = relationship.AcceptorId
			};
			_groupMemberCoreController.UpdateMemberRequest(relation.ToGroupModel(), relationship.Accepted);
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
		public void UpdateMember([FromBody] RelationshipStatusUpdate relationship)
		{
			var relation = new RelationshipRequest
			{
				RequestorId = relationship.RequestorId,
				AcceptorId = relationship.AcceptorId
			};
			_groupMemberCoreController.UpdateMember(relation.ToGroupModel());
		}
	}
}