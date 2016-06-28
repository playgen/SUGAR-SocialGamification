using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GroupMemberControllerTests : TestController
	{

		#region Configuration

		private readonly GroupRelationshipController _groupMemberDbController;

		public GroupMemberControllerTests()
		{
			_groupMemberDbController = new GroupRelationshipController(NameOrConnectionString);
		}

		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetGroupMemberRequest()
		{
			const string groupMemberName = "CreateGroupMemberRequest";
			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");
			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);
			var groupRequests = _groupMemberDbController.GetRequests(newMember.AcceptorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");
			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateGroupMemberWithNonExistingRequestor()
		{
			const string groupMemberName = "CreateGroupMemberWithNonExistingRequestor";
			var acceptor = CreateGroup(groupMemberName);

			Assert.Throws<MissingRecordException>(() => CreateGroupMember(-1, acceptor.Id));
		}

		[Fact]
		public void CreateGroupMemberWithNonExistingAcceptor()
		{
			const string groupMemberName = "CreateGroupMemberWithNonExistingAcceptor";
			var requestor = CreateUser(groupMemberName);

			Assert.Throws<MissingRecordException>(() => CreateGroupMember(requestor.Id, -1));
		}

		[Fact]
		public void CreateDuplicateGroupMember()
		{
			const string groupMemberName = "CreateDuplicateGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");
			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedGroupMember()
		{
			const string groupMemberName = "CreateDuplicateReversedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");
			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void GetNonExistingGroupMemberRequests()
		{
			var groupMembers = _groupMemberDbController.GetRequests(-1);

			Assert.Empty(groupMembers);
		}

		[Fact]
		public void AcceptGroupMemberRequest()
		{
			const string groupMemberName = "AcceptGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");
			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);
			_groupMemberDbController.UpdateRequest(newMember, true);
			var groupRequests = _groupMemberDbController.GetRequests(newMember.AcceptorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");
			Assert.Equal(matches, 0);

			var groupMembers = _groupMemberDbController.GetMembers(newMember.AcceptorId);

			matches = groupMembers.Count(g => g.Name == groupMemberName + " Requestor");
			Assert.Equal(matches, 1);
		}

		[Fact]
		public void RejectGroupMemberRequest()
		{
			const string groupMemberName = "RejectGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");
			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);
			_groupMemberDbController.UpdateRequest(newMember, false);
			var groupRequests = _groupMemberDbController.GetRequests(newMember.AcceptorId);

			var matches = groupRequests.Count(g => g.Name == groupMemberName + " Requestor");
			Assert.Equal(matches, 0);

			var groupMembers = _groupMemberDbController.GetMembers(newMember.RequestorId);
			matches = groupMembers.Count(g => g.Name == groupMemberName + " Acceptor");
			Assert.Equal(matches, 0);
		}

		[Fact]
		public void UpdateNonExistingGroupMemberRequest()
		{
			const string groupMemberName = "UpdateNonExistingGroupMemberRequest";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = new UserToGroupRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			//TODO: make exception type specific
			Assert.Throws<Exception>(() => _groupMemberDbController.UpdateRequest(newMember, true));
		}

		[Fact]
		public void GetNonExistingGroupMembers()
		{
			var groupMembers = _groupMemberDbController.GetMembers(-1);

			Assert.Empty(groupMembers);
		}

		[Fact]
		public void CreateDuplicateAcceptedGroupMember()
		{
			const string groupMemberName = "CreateDuplicateAcceptedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);

			_groupMemberDbController.UpdateRequest(newMember, true);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(requestor.Id, acceptor.Id));
		}

		[Fact]
		public void CreateDuplicateReversedAcceptedGroupMember()
		{
			const string groupMemberName = "CreateDuplicateReversedAcceptedGroupMember";

			var requestor = CreateUser(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");
			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);
			_groupMemberDbController.UpdateRequest(newMember, true);

			Assert.Throws<DuplicateRecordException>(() => CreateGroupMember(acceptor.Id, requestor.Id));
		}

		[Fact]
		public void UpdateGroupMember()
		{
			const string groupMemberName = "UpdateGroupMember";

			var requestor = CreateGroup(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");
			var newMember = CreateGroupMember(requestor.Id, acceptor.Id);
			_groupMemberDbController.UpdateRequest(newMember, true);
			_groupMemberDbController.Update(newMember);
			var members = _groupMemberDbController.GetMembers(acceptor.Id);

			Assert.Empty(members);
		}

		[Fact]
		public void UpdateNonExistingGroupMember()
		{
			string groupMemberName = "UpdateNonExistingGroupMember";

			var requestor = CreateGroup(groupMemberName + " Requestor");
			var acceptor = CreateGroup(groupMemberName + " Acceptor");

			var newMember = new UserToGroupRelationship
			{
				RequestorId = requestor.Id,
				AcceptorId = acceptor.Id
			};

			//TODO: make exception type specific
			Assert.Throws<Exception>(() => _groupMemberDbController.Update(newMember));
		}

		#endregion

		#region Helpers

		private User CreateUser(string name)
		{
			var userDbController = new UserController(NameOrConnectionString);
			var user = new User
			{
				Name = name,
			};

			userDbController.Create(user);

			return user;
		}

		private Group CreateGroup(string name)
		{
			var groupDbController = new GroupController(NameOrConnectionString);
			var group = new Group
			{
				Name = name,
			};
			groupDbController.Create(group);

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