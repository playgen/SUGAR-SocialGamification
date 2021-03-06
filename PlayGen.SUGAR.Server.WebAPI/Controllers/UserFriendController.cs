﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates User to User relationship specific operations.
	/// </summary>
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	public class UserFriendController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.RelationshipController _relationshipCoreController;

		public UserFriendController(Core.Controllers.RelationshipController relationshipCoreController, IAuthorizationService authorizationService)
		{
			_relationshipCoreController = relationshipCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Get a list of all Users that have sent relationship requests to this userId.
		/// </summary>
		/// <param name="userId">ID of the user.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("requests/{userId:int}")]
		[Authorization(ClaimScope.User, AuthorizationAction.Get, AuthorizationEntity.UserFriendRequest)]
		public async Task<IActionResult> GetFriendRequests([FromRoute]int userId)
		{
			if ((await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
			{
				var actor = _relationshipCoreController.GetRequests(userId, ActorType.User);
				var actorContract = actor.ToActorContractList();
				return new ObjectResult(actorContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Get a list of all Users that have been sent relationship requests from this userId.
		/// </summary>
		/// <param name="userId">ID of the user.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("sentrequests/{userId:int}")]
		[Authorization(ClaimScope.User, AuthorizationAction.Get, AuthorizationEntity.UserFriendRequest)]
		public async Task<IActionResult> GetSentRequests([FromRoute]int userId)
		{
			if ((await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
			{
				var actor = _relationshipCoreController.GetSentRequests(userId, ActorType.User);
				var actorContract = actor.ToActorContractList();
				return new ObjectResult(actorContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Get a list of all Users that have relationships with this userId.
		/// </summary>
		/// <param name="userId">ID of the user.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("friends/{userId:int}")]
		public IActionResult GetFriends([FromRoute]int userId)
		{
			var actor = _relationshipCoreController.GetRelatedActors(userId, ActorType.User);
			var actorContract = actor.ToActorContractList();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Create a new relationship request between two Users.
		/// Requires a relationship between the two to not already exist.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
		/// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.User, AuthorizationAction.Create, AuthorizationEntity.UserFriendRequest)]
		public async Task<IActionResult> CreateFriendRequest([FromBody]RelationshipRequest relationship)
		{
			if ((await _authorizationService.AuthorizeAsync(User, relationship.RequestorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, relationship.AcceptorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
			{
				var request = relationship.ToRelationshipModel();
				_relationshipCoreController.CreateRequest(relationship.ToRelationshipModel(), relationship.AutoAccept);
				var relationshipContract = request.ToContract();
				return new ObjectResult(relationshipContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Update an existing relationship request between two Users.
		/// Requires the relationship request to already exist between the two Users.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		[HttpPut("request")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.User, AuthorizationAction.Update, AuthorizationEntity.UserFriendRequest)]
		public async Task<IActionResult> UpdateFriendRequest([FromBody] RelationshipStatusUpdate relationship)
		{
			if ((await _authorizationService.AuthorizeAsync(User, relationship.RequestorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, relationship.AcceptorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
			{
				var relation = new RelationshipRequest
				{
					RequestorId = relationship.RequestorId,
					AcceptorId = relationship.AcceptorId
				};
				_relationshipCoreController.UpdateRequest(relation.ToRelationshipModel(), relationship.Accepted);
				return Ok();
			}
			return Forbid();
		}

		/// <summary>
		/// Update an existing relationship between two Users.
		/// Requires the relationship to already exist between the two Users.
		/// </summary>
		/// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
		[HttpPut] // todo change to a remove that takes both members of the relationship
        [ArgumentsNotNull]
		[Authorization(ClaimScope.User, AuthorizationAction.Delete, AuthorizationEntity.UserFriend)]
		public async Task<IActionResult> RemoveFriend([FromBody] RelationshipStatusUpdate relationship)
		{
			if ((await _authorizationService.AuthorizeAsync(User, relationship.RequestorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, relationship.AcceptorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
			{
				var relation = new RelationshipRequest
				{
					RequestorId = relationship.RequestorId,
					AcceptorId = relationship.AcceptorId
				};
				_relationshipCoreController.Delete(relation.ToRelationshipModel());
				return Ok();
			}
			return Forbid();
		}
	}
}