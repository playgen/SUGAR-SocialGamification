using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GroupMemberClientTests : ClientTestBase
	{
		[Fact]
		public void CanCreateRequest()
		{
			var key = "GroupMember_CanCreateRequest";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CanCreateAutoAcceptedRequest()
		{
			var key = "GroupMember_CanCreateAutoAcceptedRequest";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = true
			};

			var relationshipResponse = SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(2, received.Count());
		}

		[Fact]
		public void CannotCreateDuplicateRequest()
		{
			var key = "GroupMember_CannotCreateDuplicateRequest";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id
			};

			SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateRequestOfAccepted()
		{
			var key = "GroupMember_CannotCreateDuplicateRequestOfAccepted";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = true
			};

			SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = false;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateAutoAcceptedRequest()
		{
			var key = "GroupMember_CannotCreateDuplicateAutoAcceptedRequest";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id
			};

			SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingUser()
		{
			var key = "GroupMember_CannotCreateRequestWithNonExistingUser";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = -1,
				AcceptorId = group.Id
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingGroup()
		{
			var key = "GroupMember_CannotCreateRequestWithNonExistingGroup";
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = -1
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CanAcceptRequest()
		{
			var key = "GroupMember_CanAcceptRequest";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id
			};

			SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(1, received.Count());

			Helpers.Login(SUGARClient, "Global", key + "_Creator", out game, out var creatorAccount);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				Accepted = true
			};

			SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate);

			sent = SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(0, received.Count());

			sent = SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			received = SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(2, received.Count());
		}

		[Fact]
		public void CanRejectRequest()
		{
			var key = "GroupMember_CanRejectRequest";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id
			};

			SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(1, received.Count());

			Helpers.Login(SUGARClient, "Global", key + "_Creator", out game, out var creatorAccount);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				Accepted = false
			};

			SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate);

			sent = SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(0, received.Count());

			sent = SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CannotUpdateAlreadyAcceptedRequest()
		{
			var key = "GroupMember_CannotUpdateAlreadyAcceptedRequest";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = true
			};

			SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Helpers.Login(SUGARClient, "Global", key + "_Creator", out game, out var creatorAccount);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CannotUpdateNotExistingRequest()
		{
			var key = "GroupMember_CannotUpdateNotExistingRequest";
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
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
			var key = "GroupMember_CanUpdateRelationship";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = true
			};

			SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			var received = SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(2, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id
			};

			SUGARClient.GroupMember.UpdateMember(relationshipStatusUpdate);

			sent = SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			received = SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CannotUpdateNotExistingRelationship()
		{
			var key = "GroupMember_CannotUpdateNotExistingRelationship";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GroupMember.UpdateMember(relationshipStatusUpdate));
		}

		[Fact]
		public void CanGetMemberRequests()
		{
			var key = "GroupMember_CanGetMemberRequests";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var requestorNames = new[] {
				key + "1",
				key + "2",
				key + "3",
				key + "4",
				key + "5"
			};

			foreach (var name in requestorNames)
			{
				Helpers.Login(SUGARClient, "Global", name, out game, out var requestAccount);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = requestAccount.User.Id,
					AcceptorId = group.Id
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			Helpers.Login(SUGARClient, "Global", key + "_Creator", out game, out var creatorAccount);

			var requests = SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetSentRequests()
		{
			var key = "GroupMember_CanGetSentRequests";
			var acceptorNames = new[] {
				key + "_Group1",
				key + "_Group2",
				key + "_Group3",
				key + "_Group4",
				key + "_Group5"
			};
			foreach (var name in acceptorNames)
			{
				var group = CreateGroup(name);
				Helpers.Login(SUGARClient, "Global", key, out var gameLoop, out var loggedInAccountLoop);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = loggedInAccountLoop.User.Id,
					AcceptorId = group.Id
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var requests = SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => acceptorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetMembers()
		{
			var key = "GroupMember_CanGetMembers";
			var group = CreateGroup(key);
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var requestorNames = new[] {
				key + "1",
				key + "2",
				key + "3",
				key + "4",
				key + "5"
			};

			foreach (var name in requestorNames)
			{
				Helpers.Login(SUGARClient, "Global", name, out game, out var requestAccount);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = requestAccount.User.Id,
					AcceptorId = group.Id,
					AutoAccept = true
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var members = SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(6, members.Count());

			var memberCheck = members.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, memberCheck.Count());
		}

		[Fact]
		public void CanGetUserGroups()
		{
			var key = "GroupMember_CanGetUserGroups";
			var acceptorNames = new[] {
				key + "_Group1",
				key + "_Group2",
				key + "_Group3",
				key + "_Group4",
				key + "_Group5"
			};
			foreach (var name in acceptorNames)
			{
				var group = CreateGroup(name);
				Helpers.Login(SUGARClient, "Global", key, out var gameLoop, out var loggedInAccountLoop);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = loggedInAccountLoop.User.Id,
					AcceptorId = group.Id,
					AutoAccept = true
				};
				SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var userGroups = SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(5, userGroups.Count());

			var groupCheck = userGroups.Where(r => acceptorNames.Any(an => r.Name.Contains(an)));

			Assert.Equal(5, groupCheck.Count());
		}

		#region Helpers
		private GroupResponse CreateGroup(string key)
		{
			Helpers.Login(SUGARClient, "Global", key + "_Creator", out var game, out var creatorAccount);

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			return SUGARClient.Group.Create(groupRequest);
		}
		#endregion
	}
}
