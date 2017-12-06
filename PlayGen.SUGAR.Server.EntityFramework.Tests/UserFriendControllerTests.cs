using System;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
	public class UserFriendControllerTests : EntityFrameworkTestBase
	{
		#region Configuration
		private readonly RelationshipController _relationshipController = ControllerLocator.RelationshipController;
		private readonly UserController _userController = ControllerLocator.UserController;
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUserFriendRequest()
		{
			var userFriendName = " CreateRelationshipRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationship(requestor.Id, acceptor.Id);

			var userRequests = _relationshipController.GetRequests(newFriend.AcceptorId, ActorType.User);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void  CreateRelationshipWithNonExistingRequestor()
		{
			var userFriendName = " CreateRelationshipWithNonExistingRequestor";
			var acceptor = CreateUser(userFriendName);
			Assert.Throws<MissingRecordException>(() =>  CreateRelationship(-1, acceptor.Id));
		}

		[Fact]
		public void  CreateRelationshipWithNonExistingAcceptor()
		{
			var userFriendName = " CreateRelationshipWithNonExistingAcceptor";
			var requestor = CreateUser(userFriendName);
			Assert.Throws<MissingRecordException>(() =>  CreateRelationship(requestor.Id, -1));
		}

		[Fact]
		public void CreateDuplicateUserFriend()
		{
			var userFriendName = "CreateDuplicateUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			 CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() =>  CreateRelationship(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedUserFriend()
		{
			var userFriendName = "CreateDuplicateReversedUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			 CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() =>  CreateRelationship(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void GetNonExistingUserFriendRequests()
		{
			var userFriends = _relationshipController.GetRequests(-1, ActorType.User);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void GetUserSentFriendRequests()
		{
			var userFriendName = "GetUserSentFriendRequests";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationship(requestor.Id, acceptor.Id);

			var userRequests = _relationshipController.GetSentRequests(newFriend.RequestorId, ActorType.User);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void GetNonExistingUserSentFriendRequests()
		{
			var userFriends = _relationshipController.GetSentRequests(-1, ActorType.User);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void AcceptUserFriendRequest()
		{
			var userFriendName = "AcceptUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationship(requestor.Id, acceptor.Id);

			_relationshipController.UpdateRequest(newFriend, true);

			var userRequests = _relationshipController.GetRequests(newFriend.AcceptorId, ActorType.User);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 0);

			var userFriends = _relationshipController.GetRelationships(newFriend.RequestorId, ActorType.User);

			matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void RejectUserFriendRequest()
		{
			var userFriendName = "RejectUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationship(requestor.Id, acceptor.Id);

			_relationshipController.UpdateRequest(newFriend, false);

			var userRequests = _relationshipController.GetRequests(newFriend.AcceptorId, ActorType.User);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 0);

			var userFriends = _relationshipController.GetRelationships(newFriend.RequestorId, ActorType.User);

			matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 0);
		}

		[Fact]
		public void UpdateNonExistingUserFriendRequest()
		{
			var userFriendName = "UpdateNonExistingUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateRelationship(requestor.Id,acceptor.Id);

			Assert.Throws<InvalidOperationException>(() => _relationshipController.UpdateRequest(newFriend, true));
		}

		[Fact]
		public void GetNonExistingUserFriends()
		{
			var userFriends = _relationshipController.GetRelationships(-1, ActorType.User);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void CreateDuplicateAcceptedUserFriend()
		{
			var userFriendName = "CreateDuplicateAcceptedUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationship(requestor.Id, acceptor.Id);

			_relationshipController.UpdateRequest(newFriend, true);

			Assert.Throws<DuplicateRecordException>(() =>  CreateRelationship(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedAcceptedUserFriend()
		{
			var userFriendName = "CreateDuplicateReversedAcceptedUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationship(requestor.Id, acceptor.Id);

			_relationshipController.UpdateRequest(newFriend, true);

			Assert.Throws<DuplicateRecordException>(() =>  CreateRelationship(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void UpdateUserFriend()
		{
			var userFriendName = "UpdateUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationship(requestor.Id, acceptor.Id);

			_relationshipController.UpdateRequest(newFriend, true);

			_relationshipController.Update(newFriend);
			var friends = _relationshipController.GetRelationships(acceptor.Id, ActorType.User);

			Assert.Empty(friends);
		}

		[Fact]
		public void UpdateNonExistingUserFriend()
		{
			var userFriendName = "UpdateNonExistingUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<InvalidOperationException>(() => _relationshipController.Update(newFriend));
		}
		#endregion

		#region Helpers
		private User CreateUser(string name)
		{
			var user = new User {
				Name = name,
			};

			_userController.Create(user);

			return user;
		}

		private ActorRelationship CreateRelationship(int requestor, int acceptor)
		{
			var actorRelationship = new ActorRelationship
			{
				RequestorId = requestor,
				AcceptorId = acceptor
			};
			_relationshipController.CreateRelationship(actorRelationship);

			return actorRelationship;
		}
		#endregion
	}
}
