using System;
using System.Linq;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
	public class GroupMemberControllerTests : EntityFrameworkTestBase
	{
		#region Configuration
		private readonly GroupRelationshipController _groupMemberController = ControllerLocator.GroupRelationshipController;
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

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			var groupRequests = _groupMemberController.GetRequests(newMember.AcceptorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateGroupMemberWithNonExistingRequestor()
		{
			var groupMemberName = "CreateGroupMemberWithNonExistingRequestor";
			var acceptor = CreateGroup(groupMemberName);
			Assert.Throws<MissingRecordException>(() => CreateGroupMember(-1, acceptor.Id));
		}

		[Fact]
		public void CreateGroupMemberWithNonExistingAcceptor()
		{
			var groupMemberName = "CreateGroupMemberWithNonExistingAcceptor";
			var requestor = CreateUser(groupMemberName);
			Assert.Throws<MissingRecordException>(() => CreateGroupMember(requestor.Id, -1));
		}

		[Fact]
		public void CreateDuplicateGroupMember()
		{
			var groupMemberName = "CreateDuplicateGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedGroupMember()
		{
			var groupMemberName = "CreateDuplicateReversedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void GetNonExistingGroupMemberRequests()
		{
			var requests = _groupMemberController.GetRequests(-1);

			Assert.Empty(requests);
		}

		[Fact]
		public void GetUserSentGroupRequests()
		{
			var groupMemberName = "GetUserSentGroupRequests";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			var groupRequests = _groupMemberController.GetSentRequests(newMember.RequestorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void GetNonExistingGroupMemberSentRequests()
		{
			var requests = _groupMemberController.GetSentRequests(-1);

			Assert.Empty(requests);
		}

		[Fact]
		public void AcceptGroupMemberRequest()
		{
			var groupMemberName = "AcceptGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberController.UpdateRequest(newMember, true);

			var groupRequests = _groupMemberController.GetRequests(newMember.AcceptorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.Equal(matches, 0);

			var groupMembers = _groupMemberController.GetMembers(newMember.AcceptorId);

			matches = groupMembers.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.Equal(matches, 1);

			var userGroups = _groupMemberController.GetUserGroups(newMember.RequestorId);

			matches = userGroups.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void RejectGroupMemberRequest()
		{
			var groupMemberName = "RejectGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberController.UpdateRequest(newMember, false);

			var groupRequests = _groupMemberController.GetRequests(newMember.AcceptorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.Equal(matches, 0);

			var groupMembers = _groupMemberController.GetMembers(newMember.RequestorId);

			matches = groupMembers.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.Equal(matches, 0);

			var userGroups = _groupMemberController.GetUserGroups(newMember.RequestorId);

			matches = userGroups.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.Equal(matches, 0);
		}

		[Fact]
		public void UpdateNonExistingGroupMemberRequest()
		{
			var groupMemberName = "UpdateNonExistingGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = new UserToGroupRelationship {
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<InvalidOperationException>(() => _groupMemberController.UpdateRequest(newMember, true));
		}

		[Fact]
		public void GetNonExistingGroupMembers()
		{
			var groupMembers = _groupMemberController.GetMembers(-1);

			Assert.Empty(groupMembers);
		}

		[Fact]
		public void GetNonExistingUserGroups()
		{
			var userGroups = _groupMemberController.GetUserGroups(-1);

			Assert.Empty(userGroups);
		}

		[Fact]
		public void CreateDuplicateAcceptedGroupMember()
		{
			var groupMemberName = "CreateDuplicateAcceptedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberController.UpdateRequest(newMember, true);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedAcceptedGroupMember()
		{
			var groupMemberName = "CreateDuplicateReversedAcceptedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberController.UpdateRequest(newMember, true);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void UpdateGroupMember()
		{
			var groupMemberName = "UpdateGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberController.UpdateRequest(newMember, true);

			_groupMemberController.Update(newMember);
			var members = _groupMemberController.GetMembers(acceptor.Id);

			Assert.Empty(members);
		}

		[Fact]
		public void UpdateNonExistingGroupMember()
		{
			var groupMemberName = "UpdateNonExistingGroupMember";

			var requestor = CreateGroup(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = new UserToGroupRelationship {
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<InvalidOperationException>(() => _groupMemberController.Update(newMember));
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

		private UserToGroupRelationship CreateGroupMember(int requestor, int acceptor)
		{
			var groupMember = new UserToGroupRelationship {
				RequestorId = requestor,
				AcceptorId = acceptor
			};
			_groupMemberController.Create(groupMember, false);

			return groupMember;
		}
		#endregion
	}
}