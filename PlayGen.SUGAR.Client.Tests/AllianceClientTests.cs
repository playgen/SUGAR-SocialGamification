using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class AllianceClientTests : ClientTestBase
	{
		[Fact]
		public void CanCreateRequest()
		{
			var key = "Alliance_CanCreateRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				AutoAccept = false
			};

			var relationshipResponse = Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = Fixture.SUGARClient.AllianceClient.GetSentRequests(group1.Id);

			Assert.Equal(1, sent.Count());

			var received = Fixture.SUGARClient.AllianceClient.GetAllianceRequests(group2.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CanCreateAutoAcceptedRequest()
		{
			var key = "Alliance_CanCreateAutoAcceptedRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				AutoAccept = true
			};

			var relationshipResponse = Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);

			Assert.Equal(relationshipRequest.RequestorId, relationshipResponse.RequestorId);
			Assert.Equal(relationshipRequest.AcceptorId, relationshipResponse.AcceptorId);

			var sent = Fixture.SUGARClient.AllianceClient.GetAlliances(group1.Id);

			Assert.Equal(1, sent.Count());

			var received = Fixture.SUGARClient.AllianceClient.GetAlliances(group2.Id);

			Assert.Equal(1, received.Count());
		}

		[Fact]
		public void CannotCreateDuplicateRequest()
		{
			var key = "Alliance_CannotCreateDuplicateRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateRequestOfAccepted()
		{
			var key = "Alliance_CannotCreateDuplicateRequestOfAccepted";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				AutoAccept = true
			};

			Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);

			relationshipRequest.AutoAccept = false;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateDuplicateAutoAcceptedRequest()
		{
			var key = "Alliance_CannotCreateDuplicateAutoAcceptedRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);

			relationshipRequest.AutoAccept = true;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest));
		}

		[Fact]
		public void CannotCreateRequestWithNonExistingGroup()
		{
			var key = "Alliance_CannotCreateRequestWithNonExistingGroup";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = -1,
				AcceptorId = group2.Id
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest));
		}

		[Fact]
		public void CanAcceptRequest()
		{
			var key = "Alliance_CanAcceptRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);

			var sent = Fixture.SUGARClient.AllianceClient.GetSentRequests(group1.Id);

			Assert.Equal(1, sent.Count());

			var received = Fixture.SUGARClient.AllianceClient.GetAllianceRequests(group2.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				Accepted = true
			};

			Fixture.SUGARClient.AllianceClient.UpdateAllianceRequest(relationshipStatusUpdate);

			received = Fixture.SUGARClient.AllianceClient.GetAllianceRequests(group2.Id);

			Assert.Equal(0, received.Count());

			received = Fixture.SUGARClient.AllianceClient.GetAlliances(group2.Id);

			Assert.Equal(1, received.Count());

			sent = Fixture.SUGARClient.AllianceClient.GetSentRequests(group1.Id);

			Assert.Equal(0, sent.Count());

			sent = Fixture.SUGARClient.AllianceClient.GetAlliances(group1.Id);

			Assert.Equal(1, sent.Count());
		}

		[Fact]
		public void CanRejectRequest()
		{
			var key = "Alliance_CanRejectRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				AutoAccept = false
			};

			Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);

			var sent = Fixture.SUGARClient.AllianceClient.GetSentRequests(group1.Id);

			Assert.Equal(1, sent.Count());

			var received = Fixture.SUGARClient.AllianceClient.GetAllianceRequests(group2.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				Accepted = false
			};

			Fixture.SUGARClient.AllianceClient.UpdateAllianceRequest(relationshipStatusUpdate);

			received = Fixture.SUGARClient.AllianceClient.GetAllianceRequests(group2.Id);

			Assert.Equal(0, received.Count());

			received = Fixture.SUGARClient.AllianceClient.GetAlliances(group2.Id);

			Assert.Equal(0, received.Count());

			sent = Fixture.SUGARClient.AllianceClient.GetSentRequests(group1.Id);

			Assert.Equal(0, sent.Count());

			sent = Fixture.SUGARClient.AllianceClient.GetAlliances(group1.Id);

			Assert.Equal(0, sent.Count());
		}

		[Fact]
		public void CannotUpdateAlreadyAcceptedRequest()
		{
			var key = "Alliance_CannotUpdateAlreadyAcceptedRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				AutoAccept = true
			};

			Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.AllianceClient.UpdateAllianceRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CannotUpdateNotExistingRequest()
		{
			var key = "Alliance_CannotUpdateNotExistingRequest";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = -1,
				AcceptorId = -1,
				Accepted = true
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.AllianceClient.UpdateAllianceRequest(relationshipStatusUpdate));
		}

		[Fact]
		public void CanUpdateRelationship()
		{
			var key = "Alliance_CanUpdateRelationship";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipRequest = new RelationshipRequest
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				AutoAccept = true
			};

			Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);

			var sent = Fixture.SUGARClient.AllianceClient.GetAlliances(group1.Id);

			Assert.Equal(1, sent.Count());

			var received = Fixture.SUGARClient.AllianceClient.GetAlliances(group2.Id);

			Assert.Equal(1, received.Count());

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id,
				Accepted = false
			};

			Fixture.SUGARClient.AllianceClient.UpdateAlliance(relationshipStatusUpdate);

			sent = Fixture.SUGARClient.AllianceClient.GetAlliances(group1.Id);

			Assert.Equal(0, sent.Count());

			received = Fixture.SUGARClient.AllianceClient.GetAlliances(group2.Id);

			Assert.Equal(0, received.Count());
		}

		[Fact]
		public void CannotUpdateNotExistingRelationship()
		{
			var key = "Alliance_CannotUpdateNotExistingRelationship";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var group2 = CreateGroup(key + "_Acc");

			var relationshipStatusUpdate = new RelationshipStatusUpdate
			{
				RequestorId = group1.Id,
				AcceptorId = group2.Id
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.AllianceClient.UpdateAlliance(relationshipStatusUpdate));
		}

		[Fact]
		public void CanGetAllianceRequests()
		{
			var key = "Alliance_CanGetAllianceRequests";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group2 = CreateGroup(key + "_Acc");

			var requestorNames = new[] {
				key + "1",
				key + "2",
				key + "3",
				key + "4",
				key + "5"
			};

			foreach (var name in requestorNames)
			{
				var group = CreateGroup(name);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = group.Id,
					AcceptorId = group2.Id,
					AutoAccept = false
				};
				Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);
			}

			var requests = Fixture.SUGARClient.AllianceClient.GetAllianceRequests(group2.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetSentRequests()
		{
			var key = "Alliance_CanGetSentRequests";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group1 = CreateGroup(key + "_Req");
			var acceptorNames = new[] {
				key + "1",
				key + "2",
				key + "3",
				key + "4",
				key + "5"
			};
			foreach (var name in acceptorNames)
			{
				var group = CreateGroup(name);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = group1.Id,
					AcceptorId = group.Id,
					AutoAccept = false
				};
				Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);
			}

			var requests = Fixture.SUGARClient.AllianceClient.GetSentRequests(group1.Id);

			Assert.Equal(5, requests.Count());

			var requestCheck = requests.Where(r => acceptorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, requestCheck.Count());
		}

		[Fact]
		public void CanGetAlliances()
		{
			var key = "Alliance_CanGetAlliances";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var group2 = CreateGroup(key + "_Acc");

			var requestorNames = new[] {
				key + "1",
				key + "2",
				key + "3",
				key + "4",
				key + "5"
			};

			foreach (var name in requestorNames)
			{
				var group = CreateGroup(name);
				var relationshipRequest = new RelationshipRequest
				{
					RequestorId = group.Id,
					AcceptorId = group2.Id,
					AutoAccept = true
				};
				Fixture.SUGARClient.AllianceClient.CreateAllianceRequest(relationshipRequest);
			}

			var alliances = Fixture.SUGARClient.AllianceClient.GetAlliances(group2.Id);

			Assert.Equal(5, alliances.Count());

			var allianceCheck = alliances.Where(r => requestorNames.Any(rn => r.Name.Contains(rn)));

			Assert.Equal(5, allianceCheck.Count());
		}

		#region Helpers
		private GroupResponse CreateGroup(string key)
		{
			var groupRequest = new GroupRequest
			{
				Name = key + "_G"
			};

			return Fixture.SUGARClient.Group.Create(groupRequest);
		}
		#endregion

		public AllianceClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}