using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts.Shared;
using Xunit;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class GroupMemberClientTests
	{
		#region Configuration

		private readonly GroupMemberClient _groupMemberClient;
		private readonly GroupClient _groupClient;
		private readonly UserClient _userClient;

		public GroupMemberClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_groupMemberClient = testSugarClient.GroupMember;
			_groupClient = testSugarClient.Group;
			_userClient = testSugarClient.User;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "GroupMemberClientTests",
				Password = "GroupMemberClientTestsPassword",
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

		[Fact]
		public void CanCreateRequest()
		{
			var requestor = GetOrCreateUser("CanCreateRequest");
			var acceptor = GetOrCreateGroup("CanCreateRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = _groupMemberClient.GetSentRequests(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = _groupMemberClient.GetMemberRequests(acceptor.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CanCreateAutoAcceptedRequest()
		{
			var requestor = GetOrCreateUser("CanCreateAutoAcceptedRequest");
			var acceptor = GetOrCreateGroup("CanCreateAutoAcceptedRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true,
			};

			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = _groupMemberClient.GetUserGroups(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = _groupMemberClient.GetMembers(acceptor.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CannotCreateDuplicateRequest()
		{
			var requestor = GetOrCreateUser("CannotCreateDuplicateRequest");
			var acceptor = GetOrCreateGroup("CannotCreateDuplicateRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			Assert.Throws<ClientException>(() => _groupMemberClient.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateRequestOfAccepted()
		{
			var requestor = GetOrCreateUser("DuplicateRequestOfAccepted");
			var acceptor = GetOrCreateGroup("DuplicateRequestOfAccepted");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = false;

			Assert.Throws<ClientException>(() => _groupMemberClient.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateAutoAcceptedRequest()
		{
			var requestor = GetOrCreateUser("DuplicateAutoAcceptedRequest");
			var acceptor = GetOrCreateGroup("DuplicateAutoAcceptedRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

			Assert.Throws<ClientException>(() => _groupMemberClient.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingUser()
		{
			var acceptor = GetOrCreateGroup("CannotCreateRequestWithNonExistingUser");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = -1,
				AcceptorId = acceptor.Id,
			};

			Assert.Throws<ClientException>(() => _groupMemberClient.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingGroup()
		{
			var requestor = GetOrCreateUser("RequestWithNonExistingGroup");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = -1,
			};

			Assert.Throws<ClientException>(() => _groupMemberClient.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CanAcceptRequest()
		{
			var requestor = GetOrCreateUser("CanAcceptRequest");
			var acceptor = GetOrCreateGroup("CanAcceptRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			var sent = _groupMemberClient.GetSentRequests(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = _groupMemberClient.GetMemberRequests(acceptor.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			_groupMemberClient.UpdateMemberRequest(relationshipStatusUpdate);

			sent = _groupMemberClient.GetSentRequests(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = _groupMemberClient.GetMemberRequests(acceptor.Id);

			Assert.Equal(0, received.Count());

			sent = _groupMemberClient.GetUserGroups(requestor.Id);

			Assert.Equal(1, sent.Count());

			received = _groupMemberClient.GetMembers(acceptor.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CanRejectRequest()
		{
			var requestor = GetOrCreateUser("CanRejectRequest");
			var acceptor = GetOrCreateGroup("CanRejectRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			var sent = _groupMemberClient.GetSentRequests(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = _groupMemberClient.GetMemberRequests(acceptor.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = false
			};

			_groupMemberClient.UpdateMemberRequest(relationshipStatusUpdate);

			sent = _groupMemberClient.GetSentRequests(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = _groupMemberClient.GetMemberRequests(acceptor.Id);

			Assert.Equal(0, received.Count());

			sent = _groupMemberClient.GetUserGroups(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = _groupMemberClient.GetMembers(acceptor.Id);

			Assert.Equal(0, received.Count());
		}

		[Fact]
		public void CannotUpdateAlreadyAcceptedRequest()
		{
			var requestor = GetOrCreateUser("CannotUpdateAlreadyAcceptedRequest");
			var acceptor = GetOrCreateGroup("CannotUpdateAlreadyAcceptedRequest");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true
			};

			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				Accepted = true
			};

			Assert.Throws<ClientException>(() => _groupMemberClient.UpdateMemberRequest(relationshipStatusUpdate));
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

			Assert.Throws<ClientException>(() => _groupMemberClient.UpdateMemberRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CanUpdateRelationship()
		{
			var requestor = GetOrCreateUser("CanUpdateRelationship");
			var acceptor = GetOrCreateGroup("CanUpdateRelationship");

			var relationshipRequest = new RelationshipRequest()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
				AutoAccept = true,
			};

			var relationshipResponse = _groupMemberClient.CreateMemberRequest(relationshipRequest);

			var sent = _groupMemberClient.GetUserGroups(requestor.Id);

			Assert.Equal(1, sent.Count());

			var received = _groupMemberClient.GetMembers(acceptor.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			_groupMemberClient.UpdateMember(relationshipStatusUpdate);

			sent = _groupMemberClient.GetUserGroups(requestor.Id);

			Assert.Equal(0, sent.Count());

			received = _groupMemberClient.GetMembers(acceptor.Id);

			Assert.Equal(0, received.Count());
		}

		[Fact]
		public void CannotUpdateNotExistingRelationship()
		{
			var requestor = GetOrCreateUser("CannotUpdateNotExistingRelationship");
			var acceptor = GetOrCreateGroup("CannotUpdateNotExistingRelationship");

			var relationshipStatusUpdate = new RelationshipStatusUpdate()
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id,
			};

			Assert.Throws<ClientException>(() => _groupMemberClient.UpdateMember(relationshipStatusUpdate));
		}

		[Fact]
		public void CanGetMemberRequests()
		{
			var acceptor = GetOrCreateGroup("CanGetMemberRequests");
			var requestorNames = new string[] {
				"CanGetMemberRequests1",
				"CanGetMemberRequests2",
				"CanGetMemberRequests3",
				"CanGetMemberRequests4",
				"CanGetMemberRequests5"
			};

			foreach (var name in requestorNames)
			{
				var requestor = GetOrCreateUser(name);
				var relationshipRequest = new RelationshipRequest() {
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				_groupMemberClient.CreateMemberRequest(relationshipRequest);
			}

			var requests = _groupMemberClient.GetMemberRequests(acceptor.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Select(r => requestorNames.Contains(r.Name));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetSentRequests()
		{
			var requestor = GetOrCreateUser("CanGetSentRequests");
			var acceptorNames = new string[] {
				"CanGetSentRequests1",
				"CanGetSentRequests2",
				"CanGetSentRequests3",
				"CanGetSentRequests4",
				"CanGetSentRequests5"
			};

			foreach (var name in acceptorNames)
			{
				var acceptor = GetOrCreateGroup(name);
				var relationshipRequest = new RelationshipRequest()
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id
				};
				_groupMemberClient.CreateMemberRequest(relationshipRequest);
			}

			var requests = _groupMemberClient.GetSentRequests(requestor.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Select(r => acceptorNames.Contains(r.Name));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetMembers()
		{
			var acceptor = GetOrCreateGroup("CanGetMembers");
			var requestorNames = new string[] {
				"CanGetMembers1",
				"CanGetMembers2",
				"CanGetMembers3",
				"CanGetMembers4",
				"CanGetMembers5"
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
				_groupMemberClient.CreateMemberRequest(relationshipRequest);
			}

			var members = _groupMemberClient.GetMembers(acceptor.Id);

			Assert.Equal(5, members.Count());

			var memberCheck = members.Select(r => requestorNames.Contains(r.Name));

			Assert.Equal(5, memberCheck.Count());
		}

		[Fact]
		public void CanGetUserGroups()
		{
			var requestor = GetOrCreateUser("CanGetUserGroups");
			var acceptorNames = new string[] {
				"CanGetUserGroups1",
				"CanGetUserGroups2",
				"CanGetUserGroups3",
				"CanGetUserGroups4",
				"CanGetUserGroups5"
			};

			foreach (var name in acceptorNames)
			{
				var acceptor = GetOrCreateGroup(name);
				var relationshipRequest = new RelationshipRequest()
				{
					RequestorId = requestor.Id,
					AcceptorId = acceptor.Id,
					AutoAccept = true
				};
				_groupMemberClient.CreateMemberRequest(relationshipRequest);
			}

			var userGroups = _groupMemberClient.GetUserGroups(requestor.Id);

			Assert.Equal(5, userGroups.Count());

			var groupCheck = userGroups.Select(r => acceptorNames.Contains(r.Name));

			Assert.Equal(5, groupCheck.Count());
		}

		#endregion

		#region Helpers
		private ActorResponse GetOrCreateUser(string suffix)
		{
			string name = "GroupMemberControllerTests" + suffix ?? $"_{suffix}";
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

		private ActorResponse GetOrCreateGroup(string suffix)
		{
			string name = "GroupMemberControllerTests" + suffix ?? $"_{suffix}";
			var groups = _groupClient.Get(name);
			ActorResponse group;

			if (groups.Any())
			{
				group = groups.Single();
			}
			else
			{
				group = _groupClient.Create(new ActorRequest
				{
					Name = name
				});
			}

			return group;
		}

		#endregion

	}
}
