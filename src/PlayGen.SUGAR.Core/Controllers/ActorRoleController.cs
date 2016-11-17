using System;
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

        public ActorRole Get(int id)
        {
            var role = _actorRoleDbController.Get(id);
            return role;
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

        public IEnumerable<Actor> GetRoleActors(int roleId, int entityId, int actorId)
        {
            var roles = GetRoleActors(roleId, entityId);
            return roles;
        }

        public ActorRole Create(ActorRole newRole)
        {
            //todo Add additional permissions for every actor/game if global (or global claimscope?)
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

        public void Delete(int id, int actorId)
        {
            var actor = Get(id).ActorId;
            if (actor != actorId)
            {
                var isAdmin = GetActorRoles(actor).Select(r => _roleController.GetById(r.RoleId)).Any(r => r.ClaimScope == ClaimScope.Global);
                if (isAdmin && GetActorRoles(actorId).Select(r => _roleController.GetById(r.RoleId)).All(r => r.ClaimScope != ClaimScope.Global))
                {
                    throw new ArgumentException($"Permission cannot be removed");
                }
            }
            var actorRole = Get(id);
            var role = _roleController.GetById(actorRole.RoleId);
            if (role.Name == role.ClaimScope.ToString())
            {
                var roleCount = GetRoleActors(actorRole.RoleId, actorRole.EntityId).ToList().Count;
                if (roleCount <= 1)
                {
                    throw new ArgumentException($"Permission cannot be removed");
                }
            }
            _actorRoleDbController.Delete(id);
        }
    }
}
