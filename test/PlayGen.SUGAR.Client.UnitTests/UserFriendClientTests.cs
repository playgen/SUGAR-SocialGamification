using System;
using System.Linq;
using System.Net;
using PlayGen.SUGAR.Contracts;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;

namespace PlayGen.SUGAR.Client.IntegrationTests
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
			var requestor = GetOrCreateUser("CanCreateRequestR");
			var acceptor = GetOrCreateUser("CanCreateRequestA");

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
			var requestor = GetOrCreateUser("CanCreateAutoAcceptedRequestR");
			var acceptor = GetOrCreateUser("CanCreateAutoAcceptedRequestA");

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
			var requestor = GetOrCreateUser("CannotCreateDuplicateRequestR");
			var acceptor = GetOrCreateUser("CannotCreateDuplicateRequestA");

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
			var requestor = GetOrCreateUser("DuplicateRequestOfAcceptedR");
			var acceptor = GetOrCreateUser("DuplicateRequestOfAcceptedA");

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
			var requestor = GetOrCreateUser("DuplicateAutoAcceptedRequestR");
			var acceptor = GetOrCreateUser("DuplicateAutoAcceptedRequestA");

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
			var acceptor = GetOrCreateUser("CannotCreateRequestWithNonExistingUser");

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
			var requestor = GetOrCreateUser("RequestWithNonExistingGroup");

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
			var requestor = GetOrCreateUser("CanAcceptRequestR");
			var acceptor = GetOrCreateUser("CanAcceptRequestA");

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
			var requestor = GetOrCreateUser("CanRejectRequestR");
			var acceptor = GetOrCreateUser("CanRejectRequestA");

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
			var requestor = GetOrCreateUser("CannotUpdateAlreadyAcceptedRequestR");
			var acceptor = GetOrCreateUser("CannotUpdateAlreadyAcceptedRequestA");

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
			var requestor = GetOrCreateUser("CanUpdateRelationshipR");
			var acceptor = GetOrCreateUser("CanUpdateRelationshipA");

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
			var requestor = GetOrCreateUser("CannotUpdateNotExistingRelationshipR");
			var acceptor = GetOrCreateUser("CannotUpdateNotExistingRelationshipA");

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			Assert.Throws<ClientException>(() => _userFriendClient.UpdateFriend(relationshipStatusUpdate));
		}
		#endregion

		#region Helpers
		private ActorResponse GetOrCreateUser(string suffix)
		{
			string name = "UserFriendControllerTests" + suffix ?? $"_{suffix}";
			var users = _userClient.Get(name, true);
			ActorResponse user;

			if (users.Any())
			{
				user = users.Single();
			}
			else
			{
				user = _userClient.Create(new ActorRequest
				{
					Name = name
				});
			}

			return user;
		}

		[Test]
		public void CanGetFriendRequests()
		{
			var acceptor = GetOrCreateUser("CanGetFriendRequestsA");
			var requestorNames = new string[] {
				"CanGetFriendRequests1",
				"CanGetFriendRequests2",
				"CanGetFriendRequests3",
				"CanGetFriendRequests4",
				"CanGetFriendRequests5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = GetOrCreateUser(name);
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
			var requestor = GetOrCreateUser("CanGetSentRequestsR");
			var acceptorNames = new string[] {
				"CanGetSentRequests1",
				"CanGetSentRequests2",
				"CanGetSentRequests3",
				"CanGetSentRequests4",
				"CanGetSentRequests5"
			};

			foreach (var name in acceptorNames)
			{
				var acceptor = GetOrCreateUser(name);
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
			var acceptor = GetOrCreateUser("CanGetFriendsA");
			var requestorNames = new string[] {
				"CanGetFriends1",
				"CanGetFriends2",
				"CanGetFriends3",
				"CanGetFriends4",
				"CanGetFriends5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = GetOrCreateUser(name);
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