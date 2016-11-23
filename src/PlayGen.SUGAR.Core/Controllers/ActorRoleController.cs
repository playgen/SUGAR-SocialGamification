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
		private readonly Data.EntityFramework.Controllers.RoleController _roleController;

        public ActorRoleController(Data.EntityFramework.Controllers.ActorRoleController actorRoleDbController,
					Data.EntityFramework.Controllers.RoleController roleController)
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

        public IEnumerable<Role> GetActorRolesForEntity(int actorId, int? entityId)
        {
            var roles = _actorRoleDbController.GetActorRolesForEntity(actorId, entityId.Value).ToList();
			var adminRoles = _actorRoleDbController.GetActorRolesForEntity(actorId, -1).ToList();
			return roles.Concat(adminRoles).Distinct();
        }

        public IEnumerable<Actor> GetRoleActors(int roleId, int? entityId)
        {
            var roles = _actorRoleDbController.GetRoleActors(roleId, entityId.Value);
            return roles;
        }

        public IEnumerable<Role> GetControlled(int actorId)
        {
            var actorRoles = _actorRoleDbController.GetActorRoles(actorId).ToList();
			var controlledRoles = actorRoles.Where(ar => _roleController.Get(ar.RoleId).ClaimScope == ClaimScope.Role).ToList();
			if (controlledRoles.Any(ar => ar.EntityId.Value == -1))
			{
				return _roleController.Get();
			}
			var roles = controlledRoles.Select(cr => _roleController.Get(cr.EntityId.Value));
			return roles;
        }

        public ActorRole Create(ActorRole newRole)
        {
            newRole = _actorRoleDbController.Create(newRole);
            return newRole;
        }

        public void Create(string roleName, int actorId, int? entityId)
        {
            var role = _roleController.Get(roleName);
            if (role != null)
            {
                Create(new ActorRole { ActorId = actorId, RoleId = role.Id, EntityId = entityId.Value });
            }
        }

        public void Delete(int id)
        {
            _actorRoleDbController.Delete(id);
        }
    }
}
