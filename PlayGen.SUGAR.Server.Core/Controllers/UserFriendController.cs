using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class UserFriendController
	{
		private readonly ILogger _logger;
		private readonly UserRelationshipController _userRelationshipDbController;

		public UserFriendController(
			ILogger<UserFriendController> logger,
			UserRelationshipController userRelationshipDbController)
		{
			_logger = logger;
			_userRelationshipDbController = userRelationshipDbController;
		}

		public List<User> GetFriendRequests(int userId)
		{
			var requestors = _userRelationshipDbController.GetRequests(userId);

			_logger.LogInformation($"{requestors.Count} Requestors for UserId: {userId}");

			return requestors;
		}

		public List<User> GetSentRequests(int userId)
		{
			var requests = _userRelationshipDbController.GetSentRequests(userId);

			_logger.LogInformation($"{requests.Count} Requests for UserId: {userId}");

			return requests;
		}

		public List<User> GetFriends(int userId)
		{
			var friends = _userRelationshipDbController.GetFriends(userId);

			_logger.LogInformation($"{friends.Count} Friends for UserId: {userId}");

			return friends;
		}

		public void CreateFriendRequest(UserToUserRelationship newRelationship, bool autoAccept)
		{
			_userRelationshipDbController.Create(newRelationship, autoAccept);

			_logger.LogInformation($"{newRelationship.RequestorId} -> {newRelationship.AcceptorId}, AutoAccept: {autoAccept}");
		}

		public void UpdateFriendRequest(UserToUserRelationship relationship, bool accepted)
		{
			_userRelationshipDbController.UpdateRequest(relationship, accepted);

			_logger.LogInformation($"{relationship.RequestorId} -> {relationship.AcceptorId}, Accepted: {accepted}");
		}

		public void UpdateFriend(UserToUserRelationship relationship)
		{
			_userRelationshipDbController.Update(relationship);

			_logger.LogInformation($"{relationship.RequestorId} -> {relationship.AcceptorId}");
		}
	}
}