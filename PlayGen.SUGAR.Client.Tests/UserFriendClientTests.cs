using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class UserFriendClientTests : ClientTestBase
	{
		[Fact]
		public void CanCreateRequest()
		{
			var key = "UserFriend_CanCreateRequest";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				AutoAccept = false
			};

			var relationshipResponse = Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = Fixture.SUGARClient.UserFriend.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Friend");

			var received = Fixture.SUGARClient.UserFriend.GetFriendRequests(friend.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CanCreateAutoAcceptedRequest()
		{
			var key = "UserFriend_CanCreateAutoAcceptedRequest";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				AutoAccept = true
			};

			var relationshipResponse = Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = Fixture.SUGARClient.UserFriend.GetFriends(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			var received = Fixture.SUGARClient.UserFriend.GetFriends(friend.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CannotCreateDuplicateRequest()
		{
			var key = "UserFriend_CannotCreateDuplicateRequest";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateRequestOfAccepted()
		{
			var key = "UserFriend_CannotCreateDuplicateRequestOfAccepted";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				AutoAccept = true
			};

			Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			relationshipRequest.AutoAccept = false;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateAutoAcceptedRequest()
		{
			var key = "UserFriend_CannotCreateDuplicateAutoAcceptedRequest";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingUser()
		{
			var key = "UserFriend_CannotCreateRequestWithNonExistingUser";
			var friend = CreateUser(key);
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = -1,
				AcceptorId = friend.Id
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingFriend()
		{
			var key = "UserFriend_CannotCreateRequestWithNonExistingFriend";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = -1
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CanAcceptRequest()
		{
			var key = "UserFriend_CanAcceptRequest";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			var sent = Fixture.SUGARClient.UserFriend.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Friend");

			var received = Fixture.SUGARClient.UserFriend.GetFriendRequests(friend.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				Accepted = true
			};

			Fixture.SUGARClient.UserFriend.UpdateFriendRequest(relationshipStatusUpdate);

			received = Fixture.SUGARClient.UserFriend.GetFriendRequests(friend.Id);

			Assert.Equal(0, received.Count());

			received = Fixture.SUGARClient.UserFriend.GetFriends(friend.Id);

			Assert.Equal(1, received.Count());

			loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			sent = Fixture.SUGARClient.UserFriend.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			sent = Fixture.SUGARClient.UserFriend.GetFriends(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());
		}

		[Fact]
		public void CanRejectRequest()
		{
			var key = "UserFriend_CanRejectRequest";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			var sent = Fixture.SUGARClient.UserFriend.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Friend");

			var received = Fixture.SUGARClient.UserFriend.GetFriendRequests(friend.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				Accepted = false
			};

			Fixture.SUGARClient.UserFriend.UpdateFriendRequest(relationshipStatusUpdate);

			received = Fixture.SUGARClient.UserFriend.GetFriendRequests(friend.Id);

			Assert.Equal(0, received.Count());

			received = Fixture.SUGARClient.UserFriend.GetFriends(friend.Id);

			Assert.Equal(0, received.Count());

			loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			sent = Fixture.SUGARClient.UserFriend.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			sent = Fixture.SUGARClient.UserFriend.GetFriends(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());
		}

		[Fact]
		public void CannotUpdateAlreadyAcceptedRequest()
		{
			var key = "UserFriend_CannotUpdateAlreadyAcceptedRequest";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				AutoAccept = true
			};

			Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Friend");

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.UserFriend.UpdateFriendRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CannotUpdateNotExistingRequest()
		{
			var key = "UserFriend_CannotUpdateNotExistingRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = -1,
				AcceptorId = -1,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.UserFriend.UpdateFriendRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CanUpdateRelationship()
		{
			var key = "UserFriend_CanUpdateRelationship";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				AutoAccept = true
			};

			Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			var sent = Fixture.SUGARClient.UserFriend.GetFriends(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			var received = Fixture.SUGARClient.UserFriend.GetFriends(friend.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id,
				Accepted = false
			};

			Fixture.SUGARClient.UserFriend.UpdateFriend(relationshipStatusUpdate);

			sent = Fixture.SUGARClient.UserFriend.GetFriends(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			received = Fixture.SUGARClient.UserFriend.GetFriends(friend.Id);

			Assert.Equal(0, received.Count());
		}

		[Fact]
		public void CannotUpdateNotExistingRelationship()
		{
			var key = "UserFriend_CannotUpdateNotExistingRelationship";
			var friend = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = friend.Id
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.UserFriend.UpdateFriend(relationshipStatusUpdate));
		}

		[Fact]
		public void CanGetFriendRequests()
		{
			var key = "UserFriend_CanGetFriendRequests";
			var friend = CreateUser(key);
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var requestorNames = new[] {
				key + "1",
				key + "2",
				key + "3",
				key + "4",
				key + "5"
			};

			foreach (var name in requestorNames)
			{
				var requestAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, name);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = requestAccount.User.Id,
					AcceptorId = friend.Id,
					AutoAccept = false
				};
				Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);
			}

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Friend");

			var requests = Fixture.SUGARClient.UserFriend.GetFriendRequests(friend.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetSentRequests()
		{
			var key = "UserFriend_CanGetSentRequests";
			var acceptorNames = new[] {
				key + "1",
				key + "2",
				key + "3",
				key + "4",
				key + "5"
			};
			foreach (var name in acceptorNames)
			{
				var friend = CreateUser(name);
				var loggedInAccountLoop = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = loggedInAccountLoop.User.Id,
					AcceptorId = friend.Id,
					AutoAccept = false
				};
				Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);
			}

			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var requests = Fixture.SUGARClient.UserFriend.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => acceptorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetFriends()
		{
			var key = "UserFriend_CanGetFriends";
			var friend = CreateUser(key);
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var requestorNames = new[] {
				key + "1",
				key + "2",
				key + "3",
				key + "4",
				key + "5"
			};

			foreach (var name in requestorNames)
			{
				var requestAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, name);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = requestAccount.User.Id,
					AcceptorId = friend.Id,
					AutoAccept = true
				};
				Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);
			}

			var Friends = Fixture.SUGARClient.UserFriend.GetFriends(friend.Id);

			Assert.Equal(5, Friends.Count());

			var FriendCheck = Friends.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, FriendCheck.Count());
		}

		[Fact]
		public void CanGetUserfriends()
		{
			var key = "UserFriend_CanGetUserfriends";
			var acceptorNames = new[] {
				key + "1",
				key + "2",
				key + "3",
				key + "4",
				key + "5"
			};
			foreach (var name in acceptorNames)
			{
				var friend = CreateUser(name);
				var loggedInAccountLoop = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = loggedInAccountLoop.User.Id,
					AcceptorId = friend.Id,
					AutoAccept = true
				};
				Fixture.SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);
			}

			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var userfriends = Fixture.SUGARClient.UserFriend.GetFriends(loggedInAccount.User.Id);

			Assert.Equal(5, userfriends.Count());

			var friendCheck = userfriends.Where(r => acceptorNames.Any(an => r.Name.Contains(an)));

			Assert.Equal(5, friendCheck.Count());
		}

		#region Helpers
		private UserResponse CreateUser(string key)
		{
			var friendAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Friend");
			return friendAccount.User;
		}
		#endregion

		public UserFriendClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}