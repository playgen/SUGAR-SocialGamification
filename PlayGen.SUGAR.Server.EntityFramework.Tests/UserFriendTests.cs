using System;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
	public class UserFriendTests : EntityFrameworkTestBase
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

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationshipRequest(requestor.Id, acceptor.Id);

			var userRequests = _relationshipController.GetRelationRequestorActors(newFriend.AcceptorId, ActorType.User);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void  CreateRelationshipWithNonExistingRequestor()
		{
			var userFriendName = " CreateRelationshipWithNonExistingRequestor";
			var acceptor = Helpers.CreateUser(userFriendName);
			Assert.Throws<InvalidRelationshipException>(() =>  CreateRelationship(-1, acceptor.Id));
		}

		[Fact]
		public void  CreateRelationshipWithNonExistingAcceptor()
		{
			var userFriendName = " CreateRelationshipWithNonExistingAcceptor";
			var requestor = Helpers.CreateUser(userFriendName);
			Assert.Throws<InvalidRelationshipException>(() =>  CreateRelationship(requestor.Id, -1));
		}

		[Fact]
		public void CreateDuplicateUserFriend()
		{
			var userFriendName = "CreateDuplicateUserFriend";

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			 CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRelationshipException>(() =>  CreateRelationship(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedUserFriend()
		{
			var userFriendName = "CreateDuplicateReversedUserFriend";

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			 CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRelationshipException>(() =>  CreateRelationship(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void GetNonExistingUserFriendRequests()
		{
			var userFriends = _relationshipController.GetRelationRequestorActors(-1, ActorType.User);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void GetUserSentFriendRequests()
		{
			var userFriendName = "GetUserSentFriendRequests";

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationshipRequest(requestor.Id, acceptor.Id);

			var userRequests = _relationshipController.GetRelationAcceptorActors(newFriend.RequestorId, ActorType.User);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void GetNonExistingUserSentFriendRequests()
		{
			var userFriends = _relationshipController.GetRelationAcceptorActors(-1, ActorType.User);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void AcceptUserFriendRequest()
		{
			var userFriendName = "AcceptUserFriendRequest";

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationship(requestor.Id, acceptor.Id);

			var userRequests = _relationshipController.GetRelationRequestorActors(newFriend.AcceptorId, ActorType.User);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 0);

			var userFriends = _relationshipController.GetRelatedActors(newFriend.RequestorId, ActorType.User);

			matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void RejectUserFriendRequest()
		{
			var userFriendName = "RejectUserFriendRequest";

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationshipRequest(requestor.Id, acceptor.Id);

			_relationshipController.UpdateRequest(newFriend, false);

			var userRequests = _relationshipController.GetRelationRequestorActors(newFriend.AcceptorId, ActorType.User);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 0);

			var userFriends = _relationshipController.GetRelatedActors(newFriend.RequestorId, ActorType.User);

			matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 0);
		}

		[Fact]
		public void UpdateNonExistingUserFriendRequest()
		{
			var userFriendName = "UpdateNonExistingUserFriendRequest";

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateRelationship(requestor.Id,acceptor.Id);

			Assert.Throws<InvalidOperationException>(() => _relationshipController.UpdateRequest(newFriend, true));
		}

		[Fact]
		public void GetNonExistingUserFriends()
		{
			var userFriends = _relationshipController.GetRelatedActors(-1, ActorType.User);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void CreateDuplicateAcceptedUserFriend()
		{
			var userFriendName = "CreateDuplicateAcceptedUserFriend";

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRelationshipException>(() =>  CreateRelationship(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedAcceptedUserFriend()
		{
			var userFriendName = "CreateDuplicateReversedAcceptedUserFriend";

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRelationshipException>(() =>  CreateRelationship(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void UpdateUserFriend()
		{
			var userFriendName = "UpdateUserFriend";

			var requestor = Helpers.CreateUser(userFriendName + " Requestor");
			var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

			var newFriend =  CreateRelationship(requestor.Id, acceptor.Id);

			_relationshipController.Delete(newFriend);
			var friends = _relationshipController.GetRelatedActors(acceptor.Id, ActorType.User);

			Assert.Empty(friends);
		}

        [Fact]
        public void UpdateNonExistingUserFriend()
        {
            var userFriendName = "UpdateNonExistingUserFriend";

            var requestor = Helpers.CreateUser(userFriendName + " Requestor");
            var acceptor = Helpers.CreateUser(userFriendName + " Acceptor");

            var newFriend = new ActorRelationship
            {
                RequestorId = requestor.Id,
                AcceptorId = acceptor.Id
            };

            Assert.Throws<InvalidOperationException>(() => _relationshipController.Delete(newFriend));
        }

		[Fact]
		public void DoesDeleteRelationshipWhenUserDeleted()
		{
            // Arrange
			var requestor = Helpers.CreateUser($"{nameof(DoesDeleteRelationshipWhenUserDeleted)} Requestor");
			var acceptor = Helpers.CreateUser($"{nameof(DoesDeleteRelationshipWhenUserDeleted)} Acceptor");

			var relationship = new ActorRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			_relationshipController.CreateRelationship(relationship);

            // Act
			_userController.Delete(requestor.Id);

            // Assert
			var relationships = _relationshipController.GetRelatedActors(requestor.Id, ActorType.User);
            Assert.Empty(relationships);
		}

		[Fact]
		public void DoesDeleteRelationshipRequestWhenUserDeleted()
		{
			// Arrange
			var requestor = Helpers.CreateUser($"{nameof(DoesDeleteRelationshipRequestWhenUserDeleted)} Requestor");
			var acceptor = Helpers.CreateUser($"{nameof(DoesDeleteRelationshipRequestWhenUserDeleted)} Acceptor");

			var relationship = new ActorRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			_relationshipController.CreateRelationshipRequest(relationship);

			// Act
			_userController.Delete(requestor.Id);

			// Assert
			var relationships = _relationshipController.GetRelationAcceptorActors(requestor.Id, ActorType.User);
			Assert.Empty(relationships);
		}
        #endregion

        #region Helpers
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

		private ActorRelationship CreateRelationshipRequest(int requestor, int acceptor)
		{
			var actorRelationship = new ActorRelationship
			{
				RequestorId = requestor,
				AcceptorId = acceptor
			};
			_relationshipController.CreateRelationshipRequest(actorRelationship);

			return actorRelationship;
		}
		#endregion
	}
}
