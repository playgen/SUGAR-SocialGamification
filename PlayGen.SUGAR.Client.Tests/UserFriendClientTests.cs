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
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanCreateRequestR");
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanCreateRequestA");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = SUGARClient.UserFriend.GetSentRequests(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.UserFriend.GetFriendRequests(acceptor.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CanCreateAutoAcceptedRequest()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanCreateAutoAcceptedRequestR");
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanCreateAutoAcceptedRequestA");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = SUGARClient.UserFriend.GetFriends(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.UserFriend.GetFriends(acceptor.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CannotCreateDuplicateRequest()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CannotCreateDuplicateRequestR");
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CannotCreateDuplicateRequestA");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateRequestOfAccepted()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_DuplicateRequestOfAcceptedR");
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_DuplicateRequestOfAcceptedA");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			relationshipRequest.AutoAccept = false;

			Assert.Throws<ClientHttpException>(() => SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateAutoAcceptedRequest()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_DuplicateAutoAcceptedRequestR");
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_DuplicateAutoAcceptedRequestA");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

			Assert.Throws<ClientHttpException>(() => SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingUser()
		{
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CannotCreateRequestWithNonExistingUser");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = -1,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingGroup()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_RequestWithNonExistingGroup");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = requestor.Id,
				AcceptorId = -1
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest));
		}

		[Fact]
		public void CanAcceptRequest()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanAcceptRequestR");
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanAcceptRequestA");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			var sent = SUGARClient.UserFriend.GetSentRequests(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.UserFriend.GetFriendRequests(acceptor.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
												{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			SUGARClient.UserFriend.UpdateFriendRequest(relationshipStatusUpdate);

			sent = SUGARClient.UserFriend.GetSentRequests(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.UserFriend.GetFriendRequests(acceptor.Id);

			Assert.Equal(0, received.Count());

			sent = SUGARClient.UserFriend.GetFriends(requestor.Id);

			Assert.Equal(1, sent.Count());

			received = SUGARClient.UserFriend.GetFriends(acceptor.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CanRejectRequest()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanRejectRequestR");
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanRejectRequestA");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			var sent = SUGARClient.UserFriend.GetSentRequests(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.UserFriend.GetFriendRequests(acceptor.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
												{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = false
			};

			SUGARClient.UserFriend.UpdateFriendRequest(relationshipStatusUpdate);

			sent = SUGARClient.UserFriend.GetSentRequests(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.UserFriend.GetFriendRequests(acceptor.Id);

			Assert.Equal(0, received.Count());

			sent = SUGARClient.UserFriend.GetFriends(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.UserFriend.GetFriends(acceptor.Id);

			Assert.Equal(0, received.Count());
		}

		[Fact]
		public void CannotUpdateAlreadyAcceptedRequest()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, "CannotUpdateAlreadyAcceptedRequestR");
			var acceptor = Helpers.GetUser(SUGARClient.User, "CannotUpdateAlreadyAcceptedRequestA");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
												{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.UserFriend.UpdateFriendRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CannotUpdateNotExistingRequest()
		{
			var relationshipStatusUpdate = new RelationshipStatusUpdate
												{
				RequestorId = -1,
				AcceptorId = -1,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.UserFriend.UpdateFriendRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CanUpdateRelationship()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanUpdateRelationshipR");
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanUpdateRelationshipA");

			var relationshipRequest = new RelationshipRequest
										{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);

			var sent = SUGARClient.UserFriend.GetFriends(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.UserFriend.GetFriends(acceptor.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
												{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			SUGARClient.UserFriend.UpdateFriend(relationshipStatusUpdate);

			sent = SUGARClient.UserFriend.GetFriends(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.UserFriend.GetFriends(acceptor.Id);

			Assert.Equal(0, received.Count());
		}

		[Fact]
		public void CannotUpdateNotExistingRelationship()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CannotUpdateNotExistingRelationshipR");
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CannotUpdateNotExistingRelationshipA");

			var relationshipStatusUpdate = new RelationshipStatusUpdate
												{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.UserFriend.UpdateFriend(relationshipStatusUpdate));
		}
		
		[Fact]
		public void CanGetFriendRequests()
		{
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanGetFriendRequestsA");
			var requestorNames = new[] {
				"CanGetFriendRequests1",
				"CanGetFriendRequests2",
				"CanGetFriendRequests3",
				"CanGetFriendRequests4",
				"CanGetFriendRequests5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_{name}");
				var relationshipRequest = new RelationshipRequest
											{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);
			}

			var requests = SUGARClient.UserFriend.GetFriendRequests(acceptor.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetSentRequests()
		{
			var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanGetSentRequestsR");
			var acceptorNames = new[] {
				"CanGetSentRequests1",
				"CanGetSentRequests2",
				"CanGetSentRequests3",
				"CanGetSentRequests4",
				"CanGetSentRequests5"
			};

			foreach (var name in acceptorNames)
			{
				var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_{name}");
				var relationshipRequest = new RelationshipRequest
											{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);
			}

			var requests = SUGARClient.UserFriend.GetSentRequests(requestor.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => acceptorNames.Any(an => r.Name.Contains(an)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetFriends()
		{
			var acceptor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_CanGetFriendsA");
			var requestorNames = new[] {
				"CanGetFriends1",
				"CanGetFriends2",
				"CanGetFriends3",
				"CanGetFriends4",
				"CanGetFriends5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = Helpers.GetUser(SUGARClient.User, $"{nameof(UserFriendClientTests)}_{name}");
				var relationshipRequest = new RelationshipRequest
											{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id,
					AutoAccept = true
				};
				SUGARClient.UserFriend.CreateFriendRequest(relationshipRequest);
			}

			var members = SUGARClient.UserFriend.GetFriends(acceptor.Id);

			Assert.Equal(5, members.Count());

			var memberCheck = members.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, memberCheck.Count());
		}
	}
}