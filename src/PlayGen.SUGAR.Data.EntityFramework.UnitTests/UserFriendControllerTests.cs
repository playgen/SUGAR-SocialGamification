using System;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;
using Xunit;
using System.Linq;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class UserFriendControllerTests : IClassFixture<TestEnvironment>
	{
		#region Configuration
		private readonly UserRelationshipController _userRelationshipDbController;
		private readonly UserController _userDbController;

		public UserFriendControllerTests(TestEnvironment testEnvironment)
		{
			_userRelationshipDbController = testEnvironment.UserRelationshipController;
			_userDbController = testEnvironment.UserController;
		}
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUserFriendRequest()
		{
			string userFriendName = "CreateUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			var userRequests = _userRelationshipDbController.GetRequests(newFriend.AcceptorId);

			int matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateUserFriendWithNonExistingRequestor()
		{
			string userFriendName = "CreateUserFriendWithNonExistingRequestor";
			var acceptor = CreateUser(userFriendName);
			Assert.Throws<MissingRecordException>(() => CreateUserFriend(-1, acceptor.Id));
		}

		[Fact]
		public void CreateUserFriendWithNonExistingAcceptor()
		{
			string userFriendName = "CreateUserFriendWithNonExistingAcceptor";
			var requestor = CreateUser(userFriendName);
			Assert.Throws<MissingRecordException>(() => CreateUserFriend(requestor.Id, -1));
		}

		[Fact]
		public void CreateDuplicateUserFriend()
		{
			string userFriendName = "CreateDuplicateUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			CreateUserFriend(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateUserFriend(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedUserFriend()
		{
			string userFriendName = "CreateDuplicateReversedUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			CreateUserFriend(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateUserFriend(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void GetNonExistingUserFriendRequests()
		{
			var userFriends = _userRelationshipDbController.GetRequests(-1);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void GetUserSentFriendRequests()
		{
			string userFriendName = "GetUserSentFriendRequests";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			var userRequests = _userRelationshipDbController.GetSentRequests(newFriend.RequestorId);

			int matches = userRequests.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void GetNonExistingUserSentFriendRequests()
		{
			var userFriends = _userRelationshipDbController.GetSentRequests(-1);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void AcceptUserFriendRequest()
		{
			string userFriendName = "AcceptUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			_userRelationshipDbController.UpdateRequest(newFriend, true);

			var userRequests = _userRelationshipDbController.GetRequests(newFriend.AcceptorId);

			int matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 0);

			var userFriends = _userRelationshipDbController.GetFriends(newFriend.RequestorId);

			matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void RejectUserFriendRequest()
		{
			string userFriendName = "RejectUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			_userRelationshipDbController.UpdateRequest(newFriend, false);

			var userRequests = _userRelationshipDbController.GetRequests(newFriend.AcceptorId);

			int matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 0);

			var userFriends = _userRelationshipDbController.GetFriends(newFriend.RequestorId);

			matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 0);
		}

		[Fact]
		public void UpdateNonExistingUserFriendRequest()
		{
			string userFriendName = "UpdateNonExistingUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = new UserToUserRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<InvalidOperationException>(() => _userRelationshipDbController.UpdateRequest(newFriend, true));
		}

		[Fact]
		public void GetNonExistingUserFriends()
		{
			var userFriends = _userRelationshipDbController.GetFriends(-1);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void CreateDuplicateAcceptedUserFriend()
		{
			string userFriendName = "CreateDuplicateAcceptedUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			_userRelationshipDbController.UpdateRequest(newFriend, true);

			Assert.Throws<DuplicateRecordException>(() => CreateUserFriend(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedAcceptedUserFriend()
		{
			string userFriendName = "CreateDuplicateReversedAcceptedUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			_userRelationshipDbController.UpdateRequest(newFriend, true);

			Assert.Throws<DuplicateRecordException>(() => CreateUserFriend(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void UpdateUserFriend()
		{
			string userFriendName = "UpdateUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			_userRelationshipDbController.UpdateRequest(newFriend, true);

			_userRelationshipDbController.Update(newFriend);
			var friends = _userRelationshipDbController.GetFriends(acceptor.Id);

			Assert.Empty(friends);
		}

		[Fact]
		public void UpdateNonExistingUserFriend()
		{
			string userFriendName = "UpdateNonExistingUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = new UserToUserRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<InvalidOperationException>(() => _userRelationshipDbController.Update(newFriend));
		}
		#endregion

		#region Helpers
		private User CreateUser(string name)
		{
			var user = new User
			{
				Name = name,
			};

			_userDbController.Create(user);

			return user;
		}

		private UserToUserRelationship CreateUserFriend(int requestor, int acceptor)
		{
			var userFriend = new UserToUserRelationship
			{
				RequestorId = requestor,
				AcceptorId = acceptor
			};
			_userRelationshipDbController.Create(userFriend, false);

			return userFriend;
		}
		#endregion
	}
}
