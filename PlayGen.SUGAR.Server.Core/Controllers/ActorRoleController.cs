using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class ActorRoleController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.ActorRoleController _actorRoleDbController;
		private readonly EntityFramework.Controllers.RoleController _roleController;

		public ActorRoleController(
			ILogger<ActorRoleController> logger,
			EntityFramework.Controllers.ActorRoleController actorRoleDbController,
			EntityFramework.Controllers.RoleController roleController)
		{
			_logger = logger;
			_actorRoleDbController = actorRoleDbController;
			_roleController = roleController;
		}

		public ActorRole Get(int id)
		{
			var role = _actorRoleDbController.Get(id);

			_logger.LogInformation($"{role?.Id} for {id}");

			return role;
		}

		public List<ActorRole> GetActorRoles(int actorId, bool includeClaims = false)
		{
			var roles = _actorRoleDbController.GetActorRoles(actorId, includeClaims);

			_logger.LogInformation($"{roles?.Count} Actor Roles for ActorId: {actorId}, IncludeClaims: {includeClaims}");

			return roles;
		}

		public List<Role> GetActorRolesForEntity(int actorId, int entityId, ClaimScope scope)
		{
			var roles = _actorRoleDbController.GetActorRolesForEntity(actorId, entityId, scope, true).Select(ar => ar.Role).ToList();

			_logger.LogInformation($"{roles.Count} Roles for ActorId: {actorId}, EntityId: {entityId}, {nameof(ClaimScope)}: {scope}");

			return roles;
		}

		public List<Actor> GetRoleActors(int roleId, int entityId)
		{
			var roles = _actorRoleDbController.GetRoleActors(roleId, entityId);

			_logger.LogInformation($"{roles?.Count} Actors for RoleId: {roleId}, EntityId: {entityId}");

			return roles;
		}

		public List<Role> GetControlled(int actorId)
		{
			var actorRoles = _actorRoleDbController.GetActorRoles(actorId, true).ToList();
			var controlledRoles = actorRoles.Where(ar => ar.Role.ClaimScope == ClaimScope.Role).ToList();
			if (controlledRoles.Any(ar => ar.EntityId == Platform.AllId))
			{
				return _roleController.Get();
			}
			var roles = controlledRoles.Select(cr => _roleController.Get(cr.EntityId)).ToList();

			_logger.LogInformation($"{roles.Count} Roles for ActorId: {actorId}");

			return roles;
		}

		public ActorRole Create(ActorRole newRole)
		{
			newRole = _actorRoleDbController.Create(newRole);

			_logger.LogInformation($"{newRole?.Id}");

			return newRole;
		}

		public void Create(string roleName, int actorId, int entityId)
		{
			var role = _roleController.GetDefault(roleName);

			_logger.LogInformation($"RoleName: {roleName}, ActorId: {actorId}, EntityId: {entityId}");

			if (role != null)
			{
				Create(new ActorRole { ActorId = actorId, RoleId = role.Id, EntityId = entityId });
			}
		}

		public void Delete(int id)
		{
			_actorRoleDbController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}
