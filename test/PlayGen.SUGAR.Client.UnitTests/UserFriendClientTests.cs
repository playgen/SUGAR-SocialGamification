using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class UserFriendClientTests
	{
		#region Configuration
		private readonly UserFriendClient _userFriendClient;
		private readonly UserClient _userClient;

		public UserFriendClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_userFriendClient = testSugarClient.UserFriend;
			_userClient = testSugarClient.User;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "UserFriendClientTests",
				Password = "UserFriendClientTestsPassword",
				AutoLogin = true,
			};

			try
			{
				client.Login(accountRequest);
			}
			catch
			{
				client.Register(accountRequest);
			}
		}
		#endregion

		#region Tests
		[Test]
		public void CanCreateRequest()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "CanCreateRequestR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CanCreateRequestA");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			Assert.AreEqual(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.AreEqual(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = _userFriendClient.GetSentRequests(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = _userFriendClient.GetFriendRequests(acceptor.Id);

			Assert.AreEqual(1, received.Count());
		}

		[Test]
		public void CanCreateAutoAcceptedRequest()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "CanCreateAutoAcceptedRequestR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CanCreateAutoAcceptedRequestA");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true,
			};

			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			Assert.AreEqual(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.AreEqual(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = _userFriendClient.GetFriends(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = _userFriendClient.GetFriends(acceptor.Id);

			Assert.AreEqual(1, received.Count());
		}

		[Test]
		public void CannotCreateDuplicateRequest()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "CannotCreateDuplicateRequestR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CannotCreateDuplicateRequestA");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			Assert.Throws<ClientException>(() => _userFriendClient.CreateFriendRequest(relationshipRequest));
		}

		[Test]
		public void CannotCreateDuplicateRequestOfAccepted()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "DuplicateRequestOfAcceptedR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "DuplicateRequestOfAcceptedA");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			relationshipRequest.AutoAccept = false;

			Assert.Throws<ClientException>(() => _userFriendClient.CreateFriendRequest(relationshipRequest));
		}

		[Test]
		public void CannotCreateDuplicateAutoAcceptedRequest()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "DuplicateAutoAcceptedRequestR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "DuplicateAutoAcceptedRequestA");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

			Assert.Throws<ClientException>(() => _userFriendClient.CreateFriendRequest(relationshipRequest));
		}

		[Test]
		public void CannotCreateRequestWithNonExistingUser()
		{
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CannotCreateRequestWithNonExistingUser");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = -1,
				AcceptorId = acceptor.Id,
			};

			Assert.Throws<ClientException>(() => _userFriendClient.CreateFriendRequest(relationshipRequest));
		}

		[Test]
		public void CannotCreateRequestWithNonExistingGroup()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "RequestWithNonExistingGroup");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = -1,
			};

			Assert.Throws<ClientException>(() => _userFriendClient.CreateFriendRequest(relationshipRequest));
		}

		[Test]
		public void CanAcceptRequest()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "CanAcceptRequestR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CanAcceptRequestA");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			var sent = _userFriendClient.GetSentRequests(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = _userFriendClient.GetFriendRequests(acceptor.Id);

			Assert.AreEqual(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			_userFriendClient.UpdateFriendRequest(relationshipStatusUpdate);

			sent = _userFriendClient.GetSentRequests(requestor.Id);

			Assert.AreEqual(0, sent.Count());

			received = _userFriendClient.GetFriendRequests(acceptor.Id);

			Assert.AreEqual(0, received.Count());

			sent = _userFriendClient.GetFriends(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			received = _userFriendClient.GetFriends(acceptor.Id);

			Assert.AreEqual(1, received.Count());
		}

		[Test]
		public void CanRejectRequest()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "CanRejectRequestR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CanRejectRequestA");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			var sent = _userFriendClient.GetSentRequests(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = _userFriendClient.GetFriendRequests(acceptor.Id);

			Assert.AreEqual(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = false
			};

			_userFriendClient.UpdateFriendRequest(relationshipStatusUpdate);

			sent = _userFriendClient.GetSentRequests(requestor.Id);

			Assert.AreEqual(0, sent.Count());

			received = _userFriendClient.GetFriendRequests(acceptor.Id);

			Assert.AreEqual(0, received.Count());

			sent = _userFriendClient.GetFriends(requestor.Id);

			Assert.AreEqual(0, sent.Count());

			received = _userFriendClient.GetFriends(acceptor.Id);

			Assert.AreEqual(0, received.Count());
		}

		[Test]
		public void CannotUpdateAlreadyAcceptedRequest()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "CannotUpdateAlreadyAcceptedRequestR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CannotUpdateAlreadyAcceptedRequestA");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			Assert.Throws<ClientException>(() => _userFriendClient.UpdateFriendRequest(relationshipStatusUpdate));
		}

		[Test]
		public void CannotUpdateNotExistingRequest()
		{
			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = -1,
				AcceptorId = -1,
				Accepted = true
			};

			Assert.Throws<ClientException>(() => _userFriendClient.UpdateFriendRequest(relationshipStatusUpdate));
		}

		[Test]
		public void CanUpdateRelationship()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "CanUpdateRelationshipR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CanUpdateRelationshipA");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true,
			};

			var relationshipResponse = _userFriendClient.CreateFriendRequest(relationshipRequest);

			var sent = _userFriendClient.GetFriends(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = _userFriendClient.GetFriends(acceptor.Id);

			Assert.AreEqual(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			_userFriendClient.UpdateFriend(relationshipStatusUpdate);

			sent = _userFriendClient.GetFriends(requestor.Id);

			Assert.AreEqual(0, sent.Count());

			received = _userFriendClient.GetFriends(acceptor.Id);

			Assert.AreEqual(0, received.Count());
		}

		[Test]
		public void CannotUpdateNotExistingRelationship()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "CannotUpdateNotExistingRelationshipR");
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CannotUpdateNotExistingRelationshipA");

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			Assert.Throws<ClientException>(() => _userFriendClient.UpdateFriend(relationshipStatusUpdate));
		}
		
		[Test]
		public void CanGetFriendRequests()
		{
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CanGetFriendRequestsA");
			var requestorNames = new string[] {
				"CanGetFriendRequests1",
				"CanGetFriendRequests2",
				"CanGetFriendRequests3",
				"CanGetFriendRequests4",
				"CanGetFriendRequests5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = Helpers.GetOrCreateUser(_userClient, name);
				var relationshipRequest = new RelationshipRequest()
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				_userFriendClient.CreateFriendRequest(relationshipRequest);
			}

			var requests = _userFriendClient.GetFriendRequests(acceptor.Id);

			Assert.AreEqual(5, requests.Count());

			var requestCheck = requests.Select(r => requestorNames.Contains(r.Name));

			Assert.AreEqual(5, requestCheck.Count());
		}

		[Test]
		public void CanGetSentRequests()
		{
			var requestor = Helpers.GetOrCreateUser(_userClient, "CanGetSentRequestsR");
			var acceptorNames = new string[] {
				"CanGetSentRequests1",
				"CanGetSentRequests2",
				"CanGetSentRequests3",
				"CanGetSentRequests4",
				"CanGetSentRequests5"
			};

			foreach (var name in acceptorNames)
			{
				var acceptor = Helpers.GetOrCreateUser(_userClient, name);
				var relationshipRequest = new RelationshipRequest()
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				_userFriendClient.CreateFriendRequest(relationshipRequest);
			}

			var requests = _userFriendClient.GetSentRequests(requestor.Id);

			Assert.AreEqual(5, requests.Count());

			var requestCheck = requests.Select(r => acceptorNames.Contains(r.Name));

			Assert.AreEqual(5, requestCheck.Count());
		}

		[Test]
		public void CanGetFriends()
		{
			var acceptor = Helpers.GetOrCreateUser(_userClient, "CanGetFriendsA");
			var requestorNames = new string[] {
				"CanGetFriends1",
				"CanGetFriends2",
				"CanGetFriends3",
				"CanGetFriends4",
				"CanGetFriends5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = Helpers.GetOrCreateUser(_userClient, name);
				var relationshipRequest = new RelationshipRequest()
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id,
					AutoAccept = true
				};
				_userFriendClient.CreateFriendRequest(relationshipRequest);
			}

			var members = _userFriendClient.GetFriends(acceptor.Id);

			Assert.AreEqual(5, members.Count());

			var memberCheck = members.Select(r => requestorNames.Contains(r.Name));

			Assert.AreEqual(5, memberCheck.Count());
		}
		#endregion

	}
}