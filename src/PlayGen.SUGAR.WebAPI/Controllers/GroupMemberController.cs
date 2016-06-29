using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Contracts.Controllers;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;
using PlayGen.SUGAR.WebAPI.Exceptions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates User to Group relationship specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class GroupMemberController : Controller
	{
		private readonly Data.EntityFramework.Controllers.GroupRelationshipController _groupRelationshipController;

		public GroupMemberController(Data.EntityFramework.Controllers.GroupRelationshipController groupRelationshipController)
		{
			_groupRelationshipController = groupRelationshipController;
		}

		/// <summary>
		/// Get a list of all Users that have relationship requests for this <param name="groupId"/>.
		/// 
		/// Example Usage: GET api/groupmember/requests/1
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("requests/{groupId:int}")]
		[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult GetMemberRequests([FromRoute]int groupId)
		{
			var users = _groupRelationshipController.GetRequests(groupId);
			var actorContract = users.ToContractList();
			return Ok(actorContract);
		}

		/// <summary>
		/// Get a list of all Groups that have been sent relationship requests for this <param name="userId"/>.
		/// 
		/// Example Usage: GET api/groupmember/sentrequests/1
		/// </summary>
		/// <param name="userId">ID of the user.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("sentrequests/{userId:int}")]
		[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult GetSentRequests([FromRoute]int userId)
		{
			var requests = _groupRelationshipController.GetSentRequests(userId);
			var actorContract = requests.ToContractList();
			return Ok(actorContract);
		}

		/// <summary>
		/// Get a list of all Users that have relationships with this <param name="groupId"/>.
		/// 
		/// Example Usage: GET api/groupmember/members/1
		/// </summary>
		/// <param name="groupId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("members/{groupId:int}")]
		[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult GetMembers([FromRoute]int groupId)
		{
			var members = _groupRelationshipController.GetMembers(groupId);
			var actorContract = members.ToContractList();
			return Ok(actorContract);
		}

		/// <summary>
		/// Get a list of all Groups that have relationships with this <param name="userId"/>.
		/// 
		/// Example Usage: GET api/groupmember/usergroups/1
		/// </summary>
		/// <param name="userId">ID of the User.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("usergroups/{userId:int}")]
		[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult GetUserGroups([FromRoute]int userId)
		{
			var groups = _groupRelationshipController.GetUserGroups(userId);
			var actorContract = groups.ToContractList();
			return Ok(actorContract);
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
		[ResponseType(typeof(RelationshipResponse))]
		[ArgumentsNotNull]
		public IActionResult CreateMemberRequest([FromBody]RelationshipRequest relationship)
		{
			var request = relationship.ToGroupModel();
			_groupRelationshipController.Create(relationship.ToGroupModel(), relationship.AutoAccept);
			var relationshipContract = request.ToContract();
			return Ok(relationshipContract);
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
		public IActionResult UpdateMemberRequest([FromBody] RelationshipStatusUpdate relationship)
		{
			var relation = new RelationshipRequest {
				RequestorId = relationship.RequestorId,
				AcceptorId = relationship.AcceptorId
			};
			_groupRelationshipController.UpdateRequest(relation.ToGroupModel(), relationship.Accepted);
			return Ok();
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
		public IActionResult UpdateMember([FromBody] RelationshipStatusUpdate relationship)
		{
			var relation = new RelationshipRequest
			{
				RequestorId = relationship.RequestorId,
				AcceptorId = relationship.AcceptorId
			};
			_groupRelationshipController.Update(relation.ToGroupModel());
			return Ok();
		}
	}
}