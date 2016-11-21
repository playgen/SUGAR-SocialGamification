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
        private readonly RoleClaimController _roleClaimController;

        public ActorRoleController(Data.EntityFramework.Controllers.ActorRoleController actorRoleDbController,
                    RoleController roleController,
                    RoleClaimController roleClaimController)
        {
            _actorRoleDbController = actorRoleDbController;
            _roleController = roleController;
            _roleClaimController = roleClaimController;
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
            var roles = _actorRoleDbController.GetActorRolesForEntity(actorId, entityId.Value);
            return roles;
        }

        public IEnumerable<Actor> GetRoleActors(int roleId, int? entityId)
        {
            var roles = _actorRoleDbController.GetRoleActors(roleId, entityId.Value);
            return roles;
        }

        public IEnumerable<Role> GetControlled(int actorId)
        {
            var actorRoles = _actorRoleDbController.GetActorRoles(actorId).ToList();
            var roles = actorRoles.Where(ar => ar.EntityId.Value != -1).Select(ar => _roleController.GetById(ar.EntityId.Value)).ToList();
            roles = roles.Where(r => r.ClaimScope == ClaimScope.Role).ToList();
            if (actorRoles.Any(ar => ar.EntityId.Value == -1))
            {
                actorRoles = actorRoles.Where(ar => ar.EntityId.Value == -1).ToList();
                var newRoleScopes = actorRoles.Select(ar => _roleController.GetById(ar.RoleId)).Select(nr => nr.ClaimScope).ToList();
                var newRoles = newRoleScopes.SelectMany(nr => _roleController.GetByScope(nr)).Distinct().ToList();
                roles.AddRange(newRoles);
            }
            return roles;
        }

        public ActorRole Create(ActorRole newRole)
        {
            newRole = _actorRoleDbController.Create(newRole);
            return newRole;
        }

        public ActorRole Create(ActorRole newRole, int actorId)
        {
            var creatorRoles = GetActorRoles(actorId).ToList();
            var creatorClaims = _roleClaimController.GetClaimsByRoles(creatorRoles.Select(r => r.RoleId)).Select(c => c.Id);
            var newClaims = _roleClaimController.GetClaimsByRole(newRole.RoleId).Select(c => c.Id);
            if (!newClaims.All(nc => creatorClaims.Contains(nc)))
            {
                throw new UnauthorizedAccessException($"User does not have correct permissions");
            }
            newRole = _actorRoleDbController.Create(newRole);
            return newRole;
        }

        public void Create(string roleName, int actorId, int? entityId)
        {
            var role = _roleController.GetByName(roleName);
            if (role != null)
            {
                Create(new ActorRole { ActorId = actorId, RoleId = role.Id, EntityId = entityId.Value });
            }
        }

        public void Delete(int id, int actorId)
        {
            var actorRole = Get(id);
            var role = _roleController.GetById(actorRole.RoleId);
            if (role.Name == role.ClaimScope.ToString())
            {
                var roleCount = GetRoleActors(actorRole.RoleId, actorRole.EntityId.Value).ToList().Count;
                if (roleCount <= 1)
                {
                    throw new ArgumentException($"Permission cannot be removed");
                }
            }
            _actorRoleDbController.Delete(id);
        }
    }
}
