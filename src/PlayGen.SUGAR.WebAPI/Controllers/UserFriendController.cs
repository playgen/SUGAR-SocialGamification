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
	/// Web Controller that facilitates User to User relationship specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class UserFriendController : Controller
	{
		private readonly Data.EntityFramework.Controllers.UserRelationshipController _userRelationshipController;

		public UserFriendController(Data.EntityFramework.Controllers.UserRelationshipController userRelationshipController)
		{
			_userRelationshipController = userRelationshipController;
		}

		/// <summary>
		/// Get a list of all Users that have relationship requests for this <param name="userId"/>.
		/// 
		/// Example Usage: GET api/userfriend/requests/1
		/// </summary>
		/// <param name="userId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("requests/{userId:int}")]
		[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult GetFriendRequests([FromRoute]int userId)
		{
			var actor = _userRelationshipController.GetRequests(userId);
			var actorContract = actor.ToContractList();
			return Ok(actorContract);
		}

		/// <summary>
		/// Get a list of all Users that have been sent relationship requests for this <param name="userId"/>.
		/// 
		/// Example Usage: GET api/userfriend/sentrequests/1
		/// </summary>
		/// <param name="userId">ID of the user.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("sentrequests/{userId:int}")]
		[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult GetSentRequests([FromRoute]int userId)
		{
			var actor = _userRelationshipController.GetSentRequests(userId);
			var actorContract = actor.ToContractList();
			return Ok(actorContract);
		}

		/// <summary>
		/// Get a list of all Users that have relationships with this <param name="userId"/>.
		/// 
		/// Example Usage: GET api/userfriend/friends/1
		/// </summary>
		/// <param name="userId">ID of the group.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("friends/{userId:int}")]
		[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult GetFriends([FromRoute]int userId)
		{
			var actor = _userRelationshipController.GetFriends(userId);
			var actorContract = actor.ToContractList();
			return Ok(actorContract);
		}

		/// <summary>
		/// Create a new relationship request between two Users.
		/// Requires a relationship between the two to not already exist.
		/// 
		/// Example Usage: POST api/userfriend
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
		/// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
		[HttpPost]
		[ResponseType(typeof(RelationshipResponse))]
		[ArgumentsNotNull]
		public IActionResult CreateFriendRequest([FromBody]RelationshipRequest relationship)
		{
			var request = relationship.ToGroupModel();
			_userRelationshipController.Create(relationship.ToUserModel(), relationship.AutoAccept);
			var relationshipContract = request.ToContract();
			return Ok(relationshipContract);
		}

		/// <summary>
		/// Update an existing relationship request between <param name="relationship.RequestorId"/> and <param name="relationship.AcceptorId"/>.
		/// Requires the relationship request to already exist between the two Users.
		/// 
		/// Example Usage: PUT api/userfriend/request
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		[HttpPut("request")]
		[ArgumentsNotNull]
		public IActionResult UpdateFriendRequest([FromBody] RelationshipStatusUpdate relationship)
		{
			var relation = new RelationshipRequest
			{
				RequestorId = relationship.RequestorId,
				AcceptorId = relationship.AcceptorId
			};
			_userRelationshipController.UpdateRequest(relation.ToUserModel(), relationship.Accepted);
			return Ok();
		}

		/// <summary>
		/// Update an existing relationship between <param name="relationship.RequestorId"/> and <param name="relationship.AcceptorId"/>.
		/// Requires the relationship to already exist between the two Users.
		/// 
		/// Example Usage: PUT api/userfriend
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		[HttpPut]
		[ArgumentsNotNull]
		public IActionResult UpdateFriend([FromBody] RelationshipStatusUpdate relationship)
		{
			var relation = new RelationshipRequest
			{
				RequestorId = relationship.RequestorId,
				AcceptorId = relationship.AcceptorId
			};
			_userRelationshipController.Update(relation.ToUserModel());
			return Ok();
		}
	}
}