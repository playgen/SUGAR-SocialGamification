using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class UserFriendControllerTests : TestController
	{
		#region Configuration

		private readonly UserRelationshipController _userRelationshipDbController;

		public UserFriendControllerTests()
		{
			_userRelationshipDbController = new UserRelationshipController(NameOrConnectionString);
		}

		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUserFriendRequest()
		{
			const string userFriendName = "CreateUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			var userRequests = _userRelationshipDbController.GetRequests(newFriend.AcceptorId);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");
			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateUserFriendWithNonExistingRequestor()
		{
			const string userFriendName = "CreateUserFriendWithNonExistingRequestor";
			var acceptor = CreateUser(userFriendName);

			Assert.Throws<MissingRecordException>(() => CreateUserFriend(-1, acceptor.Id));
		}

		[Fact]
		public void CreateUserFriendWithNonExistingAcceptor()
		{
			const string userFriendName = "CreateUserFriendWithNonExistingAcceptor";
			var requestor = CreateUser(userFriendName);

			Assert.Throws<MissingRecordException>(() => CreateUserFriend(requestor.Id, -1));
		}

		[Fact]
		public void CreateDuplicateUserFriend()
		{
			const string userFriendName = "CreateDuplicateUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateUserFriend(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedUserFriend()
		{
			const string userFriendName = "CreateDuplicateReversedUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateUserFriend(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void GetNonExistingUserFriendRequests()
		{
			var userFriends = _userRelationshipDbController.GetRequests(-1);

			Assert.Empty(userFriends);
		}

		[Fact]
		public void AcceptUserFriendRequest()
		{
			const string userFriendName = "AcceptUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			_userRelationshipDbController.UpdateRequest(newFriend, true);

			var userRequests = _userRelationshipDbController.GetRequests(newFriend.AcceptorId);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

			Assert.Equal(matches, 0);

			var userFriends = _userRelationshipDbController.GetFriends(newFriend.RequestorId);

			matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void RejectUserFriendRequest()
		{
			const string userFriendName = "RejectUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");
			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);
			_userRelationshipDbController.UpdateRequest(newFriend, false);
			var userRequests = _userRelationshipDbController.GetRequests(newFriend.AcceptorId);

			var matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");
			Assert.Equal(matches, 0);

			var userFriends = _userRelationshipDbController.GetFriends(newFriend.RequestorId);
			matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");
			Assert.Equal(matches, 0);
		}

		[Fact]
		public void UpdateNonExistingUserFriendRequest()
		{
			const string userFriendName = "UpdateNonExistingUserFriendRequest";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = new UserToUserRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			//TODO: make exception type specific
			Assert.Throws<Exception>(() => _userRelationshipDbController.UpdateRequest(newFriend, true));
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
			const string userFriendName = "CreateDuplicateAcceptedUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			_userRelationshipDbController.UpdateRequest(newFriend, true);

			Assert.Throws<DuplicateRecordException>(() => CreateUserFriend(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedAcceptedUserFriend()
		{
			const string userFriendName = "CreateDuplicateReversedAcceptedUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

			_userRelationshipDbController.UpdateRequest(newFriend, true);

			Assert.Throws<DuplicateRecordException>(() => CreateUserFriend(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void UpdateUserFriend()
		{
			const string userFriendName = "UpdateUserFriend";

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
			const string userFriendName = "UpdateNonExistingUserFriend";

			var requestor = CreateUser(userFriendName + " Requestor");
			var acceptor = CreateUser(userFriendName + " Acceptor");

			var newFriend = new UserToUserRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			//TODO: make exception type specific
			Assert.Throws<Exception>(() => _userRelationshipDbController.Update(newFriend));
		}
		#endregion

		#region Helpers
		private User CreateUser(string name)
		{
			var userDbController = new UserController(NameOrConnectionString);
			var user = new User
			{
				Name = name,
			};

			userDbController.Create(user);

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