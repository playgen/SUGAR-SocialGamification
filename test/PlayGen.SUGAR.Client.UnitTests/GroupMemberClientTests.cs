using System.Linq;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class GroupMemberClientTests : ClientTestsBase
	{
		[Test]
		public void CanCreateRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CanCreateRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CanCreateRequest");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.AreEqual(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.AreEqual(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.AreEqual(1, received.Count());
		}

		[Test]
		public void CanCreateAutoAcceptedRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CanCreateAutoAcceptedRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CanCreateAutoAcceptedRequest");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.AreEqual(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.AreEqual(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.AreEqual(2, received.Count());
		}

		[Test]
		public void CannotCreateDuplicateRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CannotCreateDuplicateRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CannotCreateDuplicateRequest");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Test]
		public void CannotCreateDuplicateRequestOfAccepted()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "DuplicateRequestOfAccepted");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "DuplicateRequestOfAccepted");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = false;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Test]
		public void CannotCreateDuplicateAutoAcceptedRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "DuplicateAutoAcceptedRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "DuplicateAutoAcceptedRequest");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Test]
		public void CannotCreateRequestWithNonExistingUser()
		{
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CCRWNonExistingUser");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = -1,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Test]
		public void CannotCreateRequestWithNonExistingGroup()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "RequestWithNonExistingGroup");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = -1
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Test]
		public void CanAcceptRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CanAcceptRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CanAcceptRequest");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.AreEqual(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate);

			sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.AreEqual(0, sent.Count());

			received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.AreEqual(0, received.Count());

			sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.AreEqual(2, received.Count());
		}

		[Test]
		public void CanRejectRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CanRejectRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CanRejectRequest");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.AreEqual(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = false
			};

			SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate);

			sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.AreEqual(0, sent.Count());

			received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.AreEqual(0, received.Count());

			sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.AreEqual(0, sent.Count());

			received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.AreEqual(1, received.Count());
		}

		[Test]
		public void CannotUpdateAlreadyAcceptedRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CannotUpdateAlreadyAcceptedRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CannotUpdateAlreadyAcceptedRequest");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate));
		}

		[Test]
		public void CannotUpdateNotExistingRequest()
		{
			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = -1,
				AcceptorId = -1,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate));
		}

		[Test]
		public void CanUpdateRelationship()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CanUpdateRelationship");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CanUpdateRelationship");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.AreEqual(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.AreEqual(2, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			SUGARClient.GroupMember.UpdateMember(relationshipStatusUpdate);

			sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.AreEqual(0, sent.Count());

			received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.AreEqual(1, received.Count());
		}

		[Test]
		public void CannotUpdateNotExistingRelationship()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CannotUpdateNotExistingRelationship");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CannotUpdateNotExistingRelationship");

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.UpdateMember(relationshipStatusUpdate));
		}

		[Test]
		public void CanGetMemberRequests()
		{
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CanGetMemberRequests");
			var requestorNames = new[]
			{
				"CanGetMemberRequests1",
				"CanGetMemberRequests2",
				"CanGetMemberRequests3",
				"CanGetMemberRequests4",
				"CanGetMemberRequests5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = Helpers.GetOrCreateUser(SUGARClient.User, name);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var requests = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.AreEqual(5, requests.Count());

			var requestCheck = requests.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.AreEqual(5, requestCheck.Count());
		}

		[Test]
		public void CanGetSentRequests()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CanGetSentRequests");
			var acceptorNames = new[]
			{
				"CanGetSentRequests1",
				"CanGetSentRequests2",
				"CanGetSentRequests3",
				"CanGetSentRequests4",
				"CanGetSentRequests5"
			};

			foreach (var name in acceptorNames)
			{
				var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, name);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var requests = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.AreEqual(5, requests.Count());

			var requestCheck = requests.Where(r => acceptorNames.Any(an => r.Name.Contains(an)));

			Assert.AreEqual(5, requestCheck.Count());
		}

		[Test]
		public void CanGetMembers()
		{
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, "CanGetMembers");
			var requestorNames = new[]
			{
				"CanGetMembers1",
				"CanGetMembers2",
				"CanGetMembers3",
				"CanGetMembers4",
				"CanGetMembers5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = Helpers.GetOrCreateUser(SUGARClient.User, name);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id,
					AutoAccept = true
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var members = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.AreEqual(6, members.Count());

			var memberCheck = members.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.AreEqual(5, memberCheck.Count());
		}

		[Test]
		public void CanGetUserGroups()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, "CanGetUserGroups");
			var acceptorNames = new[]
			{
				"CanGetUserGroups1",
				"CanGetUserGroups2",
				"CanGetUserGroups3",
				"CanGetUserGroups4",
				"CanGetUserGroups5"
			};

			foreach (var name in acceptorNames)
			{
				var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, name);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id,
					AutoAccept = true
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var userGroups = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.AreEqual(5, userGroups.Count());

			var groupCheck = userGroups.Where(r => acceptorNames.Any(an => r.Name.Contains(an)));

			Assert.AreEqual(5, groupCheck.Count());
		}
	}
}