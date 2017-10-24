using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GroupMemberTests : ClientTestBase
    {
		[Fact]
		public void CanCreateRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CanCreateRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CanCreateRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CanCreateAutoAcceptedRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CanCreateAutoAcceptedRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CanCreateAutoAcceptedRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true,
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.Equal(2, received.Count());
		}

		[Fact]
		public void CannotCreateDuplicateRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CannotCreateDuplicateRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CannotCreateDuplicateRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateRequestOfAccepted()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_DuplicateRequestOfAccepted");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_DuplicateRequestOfAccepted");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = false;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateAutoAcceptedRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_DuplicateAutoAcceptedRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_DuplicateAutoAcceptedRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingUser()
		{
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CCRWNonExistingUser");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = -1,
				AcceptorId = acceptor.Id,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingGroup()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_RequestWithNonExistingGroup");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = -1,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CanAcceptRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CanAcceptRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CanAcceptRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate);

			sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.Equal(0, received.Count());

			sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.Equal(1, sent.Count());

			received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.Equal(2, received.Count());
		}

		[Fact]
		public void CanRejectRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CanRejectRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CanRejectRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = false
			};

			SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate);

			sent = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.Equal(0, received.Count());

			sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CannotUpdateAlreadyAcceptedRequest()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CannotUpdateAlreadyAcceptedRequest");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CannotUpdateAlreadyAcceptedRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CannotUpdateNotExistingRequest()
		{
			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = -1,
				AcceptorId = -1,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CanUpdateRelationship()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CanUpdateRelationship");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CanUpdateRelationship");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true,
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.Equal(2, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			SUGARClient.GroupMember.UpdateMember(relationshipStatusUpdate);

			sent = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CannotUpdateNotExistingRelationship()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CannotUpdateNotExistingRelationship");
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CannotUpdateNotExistingRelationship");

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.UpdateMember(relationshipStatusUpdate));
		}

		[Fact]
		public void CanGetMemberRequests()
		{
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CanGetMemberRequests");
			var requestorNames = new string[] {
				"CanGetMemberRequests1",
				"CanGetMemberRequests2",
				"CanGetMemberRequests3",
				"CanGetMemberRequests4",
				"CanGetMemberRequests5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_{name}");
				var relationshipRequest = new RelationshipRequest() {
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var requests = SUGARClient.GroupMember.GetMemberRequests(acceptor.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetSentRequests()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CanGetSentRequests");
			var acceptorNames = new string[] {
				"CanGetSentRequests1",
				"CanGetSentRequests2",
				"CanGetSentRequests3",
				"CanGetSentRequests4",
				"CanGetSentRequests5"
			};

			foreach (var name in acceptorNames)
			{
				var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_{name}");
				var relationshipRequest = new RelationshipRequest()
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var requests = SUGARClient.GroupMember.GetSentRequests(requestor.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => acceptorNames.Any(an => r.Name.Contains(an)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetMembers()
		{
			var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_CanGetMembers");
			var requestorNames = new string[] {
				"CanGetMembers1",
				"CanGetMembers2",
				"CanGetMembers3",
				"CanGetMembers4",
				"CanGetMembers5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_{name}");
				var relationshipRequest = new RelationshipRequest()
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id,
					AutoAccept = true
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var members = SUGARClient.GroupMember.GetMembers(acceptor.Id);

			Assert.Equal(6, members.Count());

			var memberCheck = members.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, memberCheck.Count());
		}

		[Fact]
		public void CanGetUserGroups()
		{
			var requestor = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GroupMemberTests)}_CanGetUserGroups");
			var acceptorNames = new string[] {
				"CanGetUserGroups1",
				"CanGetUserGroups2",
				"CanGetUserGroups3",
				"CanGetUserGroups4",
				"CanGetUserGroups5"
			};

			foreach (var name in acceptorNames)
			{
				var acceptor = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(GroupMemberTests)}_{name}");
				var relationshipRequest = new RelationshipRequest()
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id,
					AutoAccept = true
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var userGroups = SUGARClient.GroupMember.GetUserGroups(requestor.Id);

			Assert.Equal(5, userGroups.Count());

			var groupCheck = userGroups.Where(r => acceptorNames.Any(an => r.Name.Contains(an)));

			Assert.Equal(5, groupCheck.Count());
		}
	}
}
