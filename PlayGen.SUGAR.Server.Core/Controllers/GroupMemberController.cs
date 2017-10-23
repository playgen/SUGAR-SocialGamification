using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class GroupMemberController
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly GroupRelationshipController _groupRelationshipDbController;

		public GroupMemberController(GroupRelationshipController groupRelationshipDbController)
		{
			_groupRelationshipDbController = groupRelationshipDbController;
		}

		public List<User> GetMemberRequests(int groupId)
		{
			var requestingMembers = _groupRelationshipDbController.GetRequests(groupId);

			Logger.Info($"{requestingMembers?.Count} Group Members for GroupId: {groupId}");

			return requestingMembers;
		}

		public List<Group> GetSentRequests(int userId)
		{
			var requestedGroups = _groupRelationshipDbController.GetSentRequests(userId);

			Logger.Info($"{requestedGroups?.Count} Sent Requests for UserId: {userId}");

			return requestedGroups;
		}

		public List<User> GetMembers(int groupId)
		{
			var members = _groupRelationshipDbController.GetMembers(groupId);

			Logger.Info($"{members?.Count} Memebrs for GroupId: {groupId}");

			return members;
		}

		public int GetMemberCount(int groupId)
		{
			var count = _groupRelationshipDbController.GetMemberCount(groupId);
			return count;
		}

		public List<Group> GetUserGroups(int userId)
		{
			var membershipGroups = _groupRelationshipDbController.GetUserGroups(userId);

			Logger.Info($"{membershipGroups?.Count} User Groups for UserId: {userId}");

			return membershipGroups;
		}

		public void CreateMemberRequest(UserToGroupRelationship newRelationship, bool autoAccept)
		{
			_groupRelationshipDbController.Create(newRelationship, autoAccept);

			Logger.Info($"{newRelationship?.RequestorId} -> {newRelationship?.AcceptorId}, Auto Accept: {autoAccept}");
		}

		public void UpdateMemberRequest(UserToGroupRelationship relationship, bool autoAccept)
		{
			_groupRelationshipDbController.UpdateRequest(relationship, autoAccept);

			Logger.Info($"{relationship?.RequestorId} -> {relationship?.AcceptorId}, Auto Accept: {autoAccept}");
		}

		public void UpdateMember(UserToGroupRelationship relationship)
		{
			//todo Check if user is only group admin
			_groupRelationshipDbController.Update(relationship);
			//todo Remove ActorRole for group if user has permissions

			Logger.Info($"{relationship?.RequestorId} -> {relationship?.AcceptorId}");
		}
	}
}