using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class GroupController
    {
        private readonly Data.EntityFramework.Controllers.GroupController _groupDbController;
        private readonly Data.EntityFramework.Controllers.ActorRoleController _actorRoleController;
        private readonly Data.EntityFramework.Controllers.RoleController _roleController;

        public GroupController(Data.EntityFramework.Controllers.GroupController groupDbController,
                    Data.EntityFramework.Controllers.ActorRoleController actorRoleController,
                    Data.EntityFramework.Controllers.RoleController roleController)
        {
            _groupDbController = groupDbController;
            _actorRoleController = actorRoleController;
            _roleController = roleController;
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
            var role = _roleController.Get(ClaimScope.Actor.ToString());
            if (role != null)
            {
                _actorRoleController.Create(new ActorRole { ActorId = creatorId, RoleId = role.Id, EntityId = newGroup.Id });
                var admins = _actorRoleController.GetRoleActors(role.Id, 0);
                foreach (var admin in admins)
                {
                    _actorRoleController.Create(new ActorRole { ActorId = admin.Id, RoleId = role.Id, EntityId = newGroup.Id });
                }
            }
            return newGroup;
        }
        
        public void Update(Group group)
        {
            _groupDbController.Update(group);
        }
        
        public void Delete(int id)
        {
            _groupDbController.Delete(id);
        }
    }
}