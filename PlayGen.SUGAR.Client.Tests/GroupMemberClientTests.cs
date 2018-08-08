using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Authorization;
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
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = false
			};

			var relationshipResponse = Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = Fixture.SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Creator");

			var received = Fixture.SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CanCreateAutoAcceptedRequest()
		{
			var key = "GroupMember_CanCreateAutoAcceptedRequest";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = true
			};

			var relationshipResponse = Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = Fixture.SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			var received = Fixture.SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(2, received.Count());

			var receivedCount = Fixture.SUGARClient.GroupMember.GetMemberCount(group.Id);

			Assert.Equal(2, receivedCount);
		}

		[Fact]
		public void CannotCreateDuplicateRequest()
		{
			var key = "GroupMember_CannotCreateDuplicateRequest";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateRequestOfAccepted()
		{
			var key = "GroupMember_CannotCreateDuplicateRequestOfAccepted";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = true
			};

			Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = false;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateAutoAcceptedRequest()
		{
			var key = "GroupMember_CannotCreateDuplicateAutoAcceptedRequest";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateAutoAcceptedRequestAsync()
		{
			var key = "GroupMember_CannotCreateDuplicateAsync";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

            var complete = false;
			Exception requestException = null;

			Fixture.SUGARClient.GroupMember.CreateMemberRequestAsync(
				relationshipRequest, 
				response => complete = true,
				exception =>
				{
					requestException = exception;
                    complete = true;
				});

			while (!complete)
			{
				Fixture.SUGARClient.TryExecuteResponse();
			}

			Assert.IsType<ClientHttpException>(requestException);
		}

        [Fact]
		public void CannotCreateRequestWithNonExistingUser()
		{
			var key = "GroupMember_CannotCreateRequestWithNonExistingUser";
			var group = CreateGroup(key);
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = -1,
				AcceptorId = group.Id
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingGroup()
		{
			var key = "GroupMember_CannotCreateRequestWithNonExistingGroup";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = -1
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest));
		}

		[Fact]
		public void CanAcceptRequest()
		{
			var key = "GroupMember_CanAcceptRequest";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = Fixture.SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Creator");

			var received = Fixture.SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				Accepted = true
			};

			Fixture.SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate);

			received = Fixture.SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(0, received.Count());

			received = Fixture.SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(2, received.Count());

			var receivedCount = Fixture.SUGARClient.GroupMember.GetMemberCount(group.Id);

			Assert.Equal(2, receivedCount);

			loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			sent = Fixture.SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			sent = Fixture.SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());
		}

		[Fact]
		public void CanRejectRequest()
		{
			var key = "GroupMember_CanRejectRequest";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = Fixture.SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Creator");

			var received = Fixture.SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				Accepted = false
			};

			Fixture.SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate);

			received = Fixture.SUGARClient.GroupMember.GetMemberRequests(group.Id);

			Assert.Equal(0, received.Count());

			received = Fixture.SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(1, received.Count());

			var receivedCount = Fixture.SUGARClient.GroupMember.GetMemberCount(group.Id);

			Assert.Equal(1, receivedCount);

			loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			sent = Fixture.SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			sent = Fixture.SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());
		}

		[Fact]
		public void CannotUpdateAlreadyAcceptedRequest()
		{
			var key = "GroupMember_CannotUpdateAlreadyAcceptedRequest";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = true
			};

			Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Creator");

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CannotUpdateNotExistingRequest()
		{
			var key = "GroupMember_CannotUpdateNotExistingRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = -1,
				AcceptorId = -1,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GroupMember.UpdateMemberRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CanUpdateRelationship()
		{
			var key = "GroupMember_CanUpdateRelationship";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = true
			};

			Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			var sent = Fixture.SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(1, sent.Count());

			var received = Fixture.SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(2, received.Count());

			var receivedCount = Fixture.SUGARClient.GroupMember.GetMemberCount(group.Id);

			Assert.Equal(2, receivedCount);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				Accepted = false
			};

			Fixture.SUGARClient.GroupMember.UpdateMember(relationshipStatusUpdate);

			sent = Fixture.SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(0, sent.Count());

			received = Fixture.SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(1, received.Count());

			receivedCount = Fixture.SUGARClient.GroupMember.GetMemberCount(group.Id);

			Assert.Equal(1, receivedCount);
		}

		[Fact]
		public void CannotUpdateNotExistingRelationship()
		{
			var key = "GroupMember_CannotUpdateNotExistingRelationship";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GroupMember.UpdateMember(relationshipStatusUpdate));
		}

		[Fact]
		public void CanGetMemberRequests()
		{
			var key = "GroupMember_CanGetMemberRequests";
			var group = CreateGroup(key);
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
					AcceptorId = group.Id,
					AutoAccept = false
				};
				Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Creator");

			var requests = Fixture.SUGARClient.GroupMember.GetMemberRequests(group.Id);

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
				var loggedInAccountLoop = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = loggedInAccountLoop.User.Id,
					AcceptorId = group.Id,
					AutoAccept = false
				};
				Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var requests = Fixture.SUGARClient.GroupMember.GetSentRequests(loggedInAccount.User.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => acceptorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetMembers()
		{
			var key = "GroupMember_CanGetMembers";
			var group = CreateGroup(key);
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
					AcceptorId = group.Id,
					AutoAccept = true
				};
				Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var members = Fixture.SUGARClient.GroupMember.GetMembers(group.Id);

			Assert.Equal(6, members.Count());

			var memberCount = Fixture.SUGARClient.GroupMember.GetMemberCount(group.Id);

			Assert.Equal(6, memberCount);

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
				var loggedInAccountLoop = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = loggedInAccountLoop.User.Id,
					AcceptorId = group.Id,
					AutoAccept = true
				};
				Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);
			}

			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var userGroups = Fixture.SUGARClient.GroupMember.GetUserGroups(loggedInAccount.User.Id);

			Assert.Equal(5, userGroups.Count());

			var groupCheck = userGroups.Where(r => acceptorNames.Any(an => r.Name.Contains(an)));

			Assert.Equal(5, groupCheck.Count());
		}

		[Fact]
		public void CanJoinGroupAndTakeResources()
		{
			var key = "GroupMember_CanJoinGroupAndTakeResources";
			var group = CreateGroup(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = loggedInAccount.User.Id,
				AcceptorId = group.Id,
				AutoAccept = true
			};

			var relationshipResponse = Fixture.SUGARClient.GroupMember.CreateMemberRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var initialQunatity = 100;
			var transferQuantity = 20;

			var resourceResponse = GiveResource(Platform.GlobalId, group.Id, key, initialQunatity);
			Assert.Equal(resourceResponse.Quantity, initialQunatity);

			// Act
			var transferResponse = TakeResource(Platform.GlobalId, key, loggedInAccount.User.Id, group.Id, transferQuantity);

			Assert.Equal(initialQunatity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(transferQuantity, transferResponse.ToResource.Quantity);
		}

		#region Helpers
		private GroupResponse CreateGroup(string key)
		{
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Creator");

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			return Fixture.SUGARClient.Group.Create(groupRequest);
		}

		private ResourceResponse GiveResource(int gameId, int actorId, string key, int quantity)
		{
			var resourceRequest = new ResourceAddRequest
			{
				ActorId = actorId,
				GameId = gameId,
				Key = key,
				Quantity = quantity
			};

			return Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);
		}

		private ResourceTransferResponse TakeResource(int gameId, string key, int recipientId, int senderId, int quantity)
		{
			var TransferRequest = new ResourceTransferRequest()
			{
				GameId = gameId,
				Key = key,
				Quantity = quantity,
				RecipientActorId = recipientId,
				SenderActorId = senderId
			};

			try
			{
				return Fixture.SUGARClient.Resource.Transfer(TransferRequest);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			return null;
		}

		
		#endregion

		public GroupMemberClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}
