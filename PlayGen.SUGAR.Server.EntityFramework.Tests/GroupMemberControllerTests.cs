using System;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
	public class GroupMemberControllerTests : EntityFrameworkTestBase
	{
		#region Configuration
		private readonly RelationshipController _relationshipController = ControllerLocator.RelationshipController;
		private readonly GroupController _groupController = ControllerLocator.GroupController;
		private readonly UserController _userController = ControllerLocator.UserController;
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetGroupMemberRequest()
		{
			var groupMemberName = "CreateGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateRelationshipRequest(requestor.Id, acceptor.Id);

			var groupRequests = _relationshipController.GetRequests(newMember.AcceptorId, ActorType.User);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateGroupMemberWithNonExistingRequestor()
		{
			var groupMemberName = "CreateGroupMemberWithNonExistingRequestor";
			var acceptor = CreateGroup(groupMemberName);
			Assert.Throws<MissingRecordException>(() => CreateRelationship(-1, acceptor.Id));
		}

		[Fact]
		public void CreateGroupMemberWithNonExistingAcceptor()
		{
			var groupMemberName = "CreateGroupMemberWithNonExistingAcceptor";
			var requestor = CreateUser(groupMemberName);
			Assert.Throws<MissingRecordException>(() => CreateRelationship(requestor.Id, -1));
		}

		[Fact]
		public void CreateDuplicateGroupMember()
		{
			var groupMemberName = "CreateDuplicateGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateRelationship(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedGroupMember()
		{
			var groupMemberName = "CreateDuplicateReversedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateRelationship(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void GetNonExistingGroupMemberRequests()
		{
			var requests = _relationshipController.GetRequests(-1, ActorType.Group);

			Assert.Empty(requests);
		}

		[Fact]
		public void GetUserSentGroupRequests()
		{
			var groupMemberName = "GetUserSentGroupRequests";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateRelationshipRequest(requestor.Id, acceptor.Id);

			var groupRequests = _relationshipController.GetSentRequests(newMember.RequestorId, ActorType.Group);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void GetNonExistingGroupMemberSentRequests()
		{
			var requests = _relationshipController.GetSentRequests(-1, ActorType.Group);

			Assert.Empty(requests);
		}

		[Fact]
		public void AcceptGroupMemberRequest()
		{
			var groupMemberName = "AcceptGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateRelationshipRequest(requestor.Id, acceptor.Id);

			_relationshipController.UpdateRequest(newMember, true);

			var groupRequests = _relationshipController.GetRequests(newMember.AcceptorId, ActorType.User);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.Equal(matches, 0);

			var groupMembers = _relationshipController.GetRelationships(newMember.AcceptorId, ActorType.User);

			matches = groupMembers.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.Equal(matches, 1);

			var userGroups = _relationshipController.GetRelationships(newMember.RequestorId, ActorType.Group);

			matches = userGroups.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void RejectGroupMemberRequest()
		{
			var groupMemberName = "RejectGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateRelationshipRequest(requestor.Id, acceptor.Id);

			_relationshipController.UpdateRequest(newMember, false);

			var groupRequests = _relationshipController.GetRequests(newMember.AcceptorId, ActorType.User);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.Equal(matches, 0);

			var groupMembers = _relationshipController.GetRelationships(newMember.RequestorId, ActorType.User);

			matches = groupMembers.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.Equal(matches, 0);

			var userGroups = _relationshipController.GetRelationships(newMember.RequestorId, ActorType.Group);

			matches = userGroups.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.Equal(matches, 0);
		}

		[Fact]
		public void UpdateNonExistingGroupMemberRequest()
		{
			var groupMemberName = "UpdateNonExistingGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<InvalidOperationException>(() => _relationshipController.UpdateRequest(newMember, true));
		}

		[Fact]
		public void GetNonExistingGroupMembers()
		{
			var groupMembers = _relationshipController.GetRelationships(-1, ActorType.Group);

			Assert.Empty(groupMembers);
		}

		[Fact]
		public void GetNonExistingUserGroups()
		{
			var userGroups = _relationshipController.GetRelationships(-1, ActorType.User);

			Assert.Empty(userGroups);
		}

		[Fact]
		public void CreateDuplicateAcceptedGroupMember()
		{
			var groupMemberName = "CreateDuplicateAcceptedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateRelationship(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedAcceptedGroupMember()
		{
			var groupMemberName = "CreateDuplicateReversedAcceptedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			CreateRelationship(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateRelationship(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void UpdateGroupMember()
		{
			var groupMemberName = "UpdateGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateRelationship(requestor.Id, acceptor.Id);

			_relationshipController.Update(newMember);

			var members = _relationshipController.GetRelationships(acceptor.Id, ActorType.User);

			Assert.Empty(members);
		}

		[Fact]
		public void UpdateNonExistingGroupMember()
		{
			var groupMemberName = "UpdateNonExistingGroupMember";

			var requestor = CreateGroup(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = new ActorRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<InvalidOperationException>(() => _relationshipController.UpdateRequest(newMember, true));
		}
		#endregion

		#region Helpers
		private User CreateUser(string name)
		{
			var user = new User {
				Name = name,
			};

			_userController.Create(user);

			return user;
		}

		private Group CreateGroup(string name)
		{
			var group = new Group {
				Name = name,
			};
			_groupController.Create(group);

			return group;
		}

		private ActorRelationship CreateRelationship(int requestor, int acceptor)
		{
			var actorRelationship = new ActorRelationship{
				RequestorId = requestor,
				AcceptorId = acceptor
			};
			_relationshipController.CreateRelationship(actorRelationship);

			return actorRelationship;
		}

		private ActorRelationship CreateRelationshipRequest(int requestor, int acceptor)
		{
			var actorRelationship = new ActorRelationship
			{
				RequestorId = requestor,
				AcceptorId = acceptor
			};
			_relationshipController.CreateRelationshipRequest(actorRelationship);

			return actorRelationship;
		}
		#endregion
	}
}