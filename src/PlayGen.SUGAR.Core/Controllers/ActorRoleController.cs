using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;
using System.Linq;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class ActorRoleController
    {
        private readonly Data.EntityFramework.Controllers.ActorRoleController _actorRoleDbController;
        private readonly RoleController _roleController;

        public ActorRoleController(Data.EntityFramework.Controllers.ActorRoleController actorRoleDbController,
                    RoleController roleController)
        {
            _actorRoleDbController = actorRoleDbController;
            _roleController = roleController;
        }

        public IEnumerable<ActorRole> GetActorRoles(int actorId)
        {
            var roles = _actorRoleDbController.GetActorRoles(actorId);
            return roles;
        }

        public IEnumerable<Actor> GetRoleActors(int roleId, int entityId)
        {
            var roles = _actorRoleDbController.GetRoleActors(roleId, entityId);
            return roles;
        }

        public ActorRole Create(ActorRole newRole)
        {
            newRole = _actorRoleDbController.Create(newRole);
            return newRole;
        }

        public void Create(string roleName, int actorId, int entityId)
        {
            var role = _roleController.GetByName(roleName);
            if (role != null)
            {
                Create(new ActorRole { ActorId = actorId, RoleId = role.Id, EntityId = entityId });
                var adminRole = _roleController.GetByName(ClaimScope.Global.ToString());
                var admins = GetRoleActors(adminRole.Id, 0);
                admins = admins.Where(a => a.Id != actorId);
                foreach (var admin in admins)
                {
                    Create(new ActorRole { ActorId = admin.Id, RoleId = role.Id, EntityId = entityId });
                }
            }
        }

        public void Delete(int id)
        {
            _actorRoleDbController.Delete(id);
        }
    }
}
