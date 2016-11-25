using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class GroupController : ActorController
    {
        private readonly Data.EntityFramework.Controllers.GroupController _groupDbController;
        private readonly ActorRoleController _actorRoleController;
        private readonly GroupMemberController _groupMemberController;

        public GroupController(Data.EntityFramework.Controllers.GroupController groupDbController,
                    ActorRoleController actorRoleController,
                    GroupMemberController groupMemberController)
        {
            _groupDbController = groupDbController;
            _actorRoleController = actorRoleController;
            _groupMemberController = groupMemberController;
        }
        
        public IEnumerable<Group> Get()
        {
            var groups = _groupDbController.Get();
            return groups;
        }

        public Group Get(int id)
        {
            var group = _groupDbController.Get(id);
            return group;
        }

        public IEnumerable<Group> Search(string name)
        {
            var groups = _groupDbController.Get(name);
            return groups;
        }
        
        public Group Create(Group newGroup, int creatorId)
        {
            newGroup = _groupDbController.Create(newGroup);
            _actorRoleController.Create(ClaimScope.Group.ToString(), creatorId, newGroup.Id);
			_groupMemberController.CreateMemberRequest(new UserToGroupRelationship { RequestorId = creatorId, AcceptorId = newGroup.Id }, true);
			return newGroup;
        }
        
        public void Update(Group group)
        {
            _groupDbController.Update(group);

            TriggerUpdatedEvent(group);
        }
        
        public void Delete(int id)
        {
            TriggerDeletedEvent(id);

            _groupDbController.Delete(id);
        }
    }
}