using System.Collections.Generic;
using PlayGen.SUGAR.Common.Permissions;
using PlayGen.SUGAR.Data.Model;
using System.Linq;
using NLog;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class ActorRoleController
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();

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

			Logger.Info($"{role?.Id} for {id}");

			return role;
		}

		public List<ActorRole> GetActorRoles(int actorId, bool includeClaims = false)
		{
			var roles = _actorRoleDbController.GetActorRoles(actorId, includeClaims);

			Logger.Info($"{roles?.Count} Actor Roles for ActorId: {actorId}, IncludeClaims: {includeClaims}");

			return roles;
		}

		public List<Role> GetActorRolesForEntity(int actorId, int? entityId, ClaimScope scope)
		{
			var roles = _actorRoleDbController.GetActorRolesForEntity(actorId, entityId.Value, scope, true).Select(ar => ar.Role).ToList();

			Logger.Info($"{roles?.Count} Roles for ActorId: {actorId}, EntityId: {entityId}, {nameof(ClaimScope)}: {scope}");

			return roles;
		}

		public List<Actor> GetRoleActors(int roleId, int? entityId)
		{
			var roles = _actorRoleDbController.GetRoleActors(roleId, entityId.Value);

			Logger.Info($"{roles?.Count} Actors for RoleId: {roleId}, EntityId: {entityId}");

			return roles;
		}

		public List<Role> GetControlled(int actorId)
		{
			var actorRoles = _actorRoleDbController.GetActorRoles(actorId, true).ToList();
			var controlledRoles = actorRoles.Where(ar => ar.Role.ClaimScope == ClaimScope.Role).ToList();
			if (controlledRoles.Any(ar => ar.EntityId.Value == -1))
			{
				return _roleController.Get();
			}
			var roles = controlledRoles.Select(cr => _roleController.Get(cr.EntityId.Value)).ToList();

			Logger.Info($"{roles?.Count} Roles for ActorId: {actorId}");

			return roles;
		}

		public ActorRole Create(ActorRole newRole)
		{
			newRole = _actorRoleDbController.Create(newRole);

			Logger.Info($"{newRole?.Id}");

			return newRole;
		}

		public void Create(string roleName, int actorId, int? entityId)
		{
			var role = _roleController.GetDefault(roleName);

			Logger.Info($"RoleName: {roleName}, ActorId: {actorId}, EntityId: {entityId}");

			if (role != null)
			{
				Create(new ActorRole { ActorId = actorId, RoleId = role.Id, EntityId = entityId.Value });
			}
		}

		public void Delete(int id)
		{
			_actorRoleDbController.Delete(id);

			Logger.Info($"{id}");
		}
	}
}
