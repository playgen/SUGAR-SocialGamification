using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;

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

		public IEnumerable<ActorRole> GetActorRoles(int actorId, bool includeClaims = false)
		{
			var roles = _actorRoleDbController.GetActorRoles(actorId, includeClaims);
			return roles;
		}

		public IEnumerable<Role> GetActorRolesForEntity(int actorId, int? entityId, ClaimScope scope)
		{
			var roles = _actorRoleDbController.GetActorRolesForEntity(actorId, entityId.Value, scope, true).Select(ar => ar.Role).ToList();
			return roles;
		}

		public IEnumerable<Actor> GetRoleActors(int roleId, int? entityId)
		{
			var roles = _actorRoleDbController.GetRoleActors(roleId, entityId.Value);
			return roles;
		}

		public IEnumerable<Role> GetControlled(int actorId)
		{
			var actorRoles = _actorRoleDbController.GetActorRoles(actorId, true).ToList();
			var controlledRoles = actorRoles.Where(ar => ar.Role.ClaimScope == ClaimScope.Role).ToList();
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
