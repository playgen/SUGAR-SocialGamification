using System;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using NUnit.Framework;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GroupMemberControllerTests
	{

		#region Configuration
		private readonly GroupRelationshipController _groupMemberDbController;
		private readonly GroupController _groupController;
		private readonly UserController _userController;

		public GroupMemberControllerTests()
		{
			_groupMemberDbController = TestEnvironment.GroupRelationshipController;
			_userController = TestEnvironment.UserController;
			_groupController = TestEnvironment.GroupController;
		}
		#endregion


		#region Tests
		[Test]
		public void CreateAndGetGroupMemberRequest()
		{
			var groupMemberName = "CreateGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			var groupRequests = _groupMemberDbController.GetRequests(newMember.AcceptorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.AreEqual(matches, 1);
		}

		[Test]
		public void CreateGroupMemberWithNonExistingRequestor()
		{
			var groupMemberName = "CreateGroupMemberWithNonExistingRequestor";
			var acceptor = CreateGroup(groupMemberName);
			Assert.Throws<MissingRecordException>(() => CreateGroupMember(-1, acceptor.Id));
		}

		[Test]
		public void CreateGroupMemberWithNonExistingAcceptor()
		{
			var groupMemberName = "CreateGroupMemberWithNonExistingAcceptor";
			var requestor = CreateUser(groupMemberName);
			Assert.Throws<MissingRecordException>(() => CreateGroupMember(requestor.Id, -1));
		}

		[Test]
		public void CreateDuplicateGroupMember()
		{
			var groupMemberName = "CreateDuplicateGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(requestor.Id, acceptor.Id));
		}

		[Test]
		public void CreateDuplicateReversedGroupMember()
		{
			var groupMemberName = "CreateDuplicateReversedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(acceptor.Id, requestor.Id));
		}

		[Test]
		public void GetNonExistingGroupMemberRequests()
		{
			var requests = _groupMemberDbController.GetRequests(-1);

			Assert.IsEmpty(requests);
		}

		[Test]
		public void GetUserSentGroupRequests()
		{
			var groupMemberName = "GetUserSentGroupRequests";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			var groupRequests = _groupMemberDbController.GetSentRequests(newMember.RequestorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.AreEqual(matches, 1);
		}

		[Test]
		public void GetNonExistingGroupMemberSentRequests()
		{
			var requests = _groupMemberDbController.GetSentRequests(-1);

			Assert.IsEmpty(requests);
		}

		[Test]
		public void AcceptGroupMemberRequest()
		{
			var groupMemberName = "AcceptGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberDbController.UpdateRequest(newMember, true);

			var groupRequests = _groupMemberDbController.GetRequests(newMember.AcceptorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.AreEqual(matches, 0);

			var groupMembers = _groupMemberDbController.GetMembers(newMember.AcceptorId);

			matches = groupMembers.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.AreEqual(matches, 1);

			var userGroups = _groupMemberDbController.GetUserGroups(newMember.RequestorId);

			matches = userGroups.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.AreEqual(matches, 1);
		}

		[Test]
		public void RejectGroupMemberRequest()
		{
			var groupMemberName = "RejectGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberDbController.UpdateRequest(newMember, false);

			var groupRequests = _groupMemberDbController.GetRequests(newMember.AcceptorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");

			Assert.AreEqual(matches, 0);

			var groupMembers = _groupMemberDbController.GetMembers(newMember.RequestorId);

			matches = groupMembers.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.AreEqual(matches, 0);

			var userGroups = _groupMemberDbController.GetUserGroups(newMember.RequestorId);

			matches = userGroups.Count(g => g.Name == groupMemberName + " Acceptor");

			Assert.AreEqual(matches, 0);
		}

		[Test]
		public void UpdateNonExistingGroupMemberRequest()
		{
			var groupMemberName = "UpdateNonExistingGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = new UserToGroupRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<InvalidOperationException>(() => _groupMemberDbController.UpdateRequest(newMember, true));
		}

		[Test]
		public void GetNonExistingGroupMembers()
		{
			var groupMembers = _groupMemberDbController.GetMembers(-1);

			Assert.IsEmpty(groupMembers);
		}

		[Test]
		public void GetNonExistingUserGroups()
		{
			var userGroups = _groupMemberDbController.GetUserGroups(-1);

			Assert.IsEmpty(userGroups);
		}

		[Test]
		public void CreateDuplicateAcceptedGroupMember()
		{
			var groupMemberName = "CreateDuplicateAcceptedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberDbController.UpdateRequest(newMember, true);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(requestor.Id, acceptor.Id));
		}

		[Test]
		public void CreateDuplicateReversedAcceptedGroupMember()
		{
			var groupMemberName = "CreateDuplicateReversedAcceptedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberDbController.UpdateRequest(newMember, true);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(acceptor.Id, requestor.Id));
		}

		[Test]
		public void UpdateGroupMember()
		{
			var groupMemberName = "UpdateGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberDbController.UpdateRequest(newMember, true);

			_groupMemberDbController.Update(newMember);
			var members = _groupMemberDbController.GetMembers(acceptor.Id);

			Assert.IsEmpty(members);
		}

		[Test]
		public void UpdateNonExistingGroupMember()
		{
			var groupMemberName = "UpdateNonExistingGroupMember";

			var requestor = CreateGroup(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = new UserToGroupRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			Assert.Throws<InvalidOperationException>(() => _groupMemberDbController.Update(newMember));
		}
		#endregion

		#region Helpers
		private User CreateUser(string name)
		{
			var user = new User
			{
				Name = name,
			};

			_userController.Create(user);

			return user;
		}

		private Group CreateGroup(string name)
		{
			var group = new Group
			{
				Name = name,
			};
			_groupController.Create(group);

			return group;
		}

		private UserToGroupRelationship CreateGroupMember(int requestor, int acceptor)
		{
			var groupMember = new UserToGroupRelationship
			{
				RequestorId = requestor,
				AcceptorId = acceptor
			};
			_groupMemberDbController.Create(groupMember, false);

			return groupMember;
		}
		#endregion
	}
}