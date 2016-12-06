using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class GroupMemberController
	{
		private readonly Data.EntityFramework.Controllers.GroupRelationshipController _groupRelationshipDbController;

		public GroupMemberController(Data.EntityFramework.Controllers.GroupRelationshipController groupRelationshipDbController)
		{
			_groupRelationshipDbController = groupRelationshipDbController;
		}
		
		public IEnumerable<User> GetMemberRequests(int groupId)
		{
			var requestingMembers = _groupRelationshipDbController.GetRequests(groupId);
			return requestingMembers;
		}
		
		public IEnumerable<Group> GetSentRequests(int userId)
		{
			var requestedGroups = _groupRelationshipDbController.GetSentRequests(userId);
			return requestedGroups;
		}
		
		public IEnumerable<User> GetMembers(int groupId)
		{
			var members = _groupRelationshipDbController.GetMembers(groupId);
			return members;
		}

		public int GetMemberCount(int groupId)
		{
			var count = _groupRelationshipDbController.GetMemberCount(groupId);
			return count;
		}

		public IEnumerable<Group> GetUserGroups(int userId)
		{
			var membershipGroups = _groupRelationshipDbController.GetUserGroups(userId);
			return membershipGroups;
		}
		
		public void CreateMemberRequest(UserToGroupRelationship newRelationship, bool autoAccept)
		{
			_groupRelationshipDbController.Create(newRelationship, autoAccept);
		}
	  
		public void UpdateMemberRequest(UserToGroupRelationship relationship, bool autoAccept)
		{
			_groupRelationshipDbController.UpdateRequest(relationship, autoAccept);
		}
	  
		public void UpdateMember(UserToGroupRelationship relationship)
		{
			//todo Check if user is only group admin
			_groupRelationshipDbController.Update(relationship);
			//todo Remove ActorRole for group if user has permissions
		}
	}
}