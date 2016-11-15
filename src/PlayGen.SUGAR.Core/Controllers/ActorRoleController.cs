using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class ActorRoleController
    {
        private readonly Data.EntityFramework.Controllers.ActorRoleController _actorRoleDbController;

        public ActorRoleController(Data.EntityFramework.Controllers.ActorRoleController actorRoleDbController)
        {
            _actorRoleDbController = actorRoleDbController;
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

        public void Delete(int id)
        {
            _actorRoleDbController.Delete(id);
        }
    }
}
