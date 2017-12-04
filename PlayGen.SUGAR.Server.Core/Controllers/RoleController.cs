using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class RoleController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly EntityFramework.Controllers.RoleController _roleDbController;
		private readonly ActorRoleController _actorRoleController;

		public RoleController(EntityFramework.Controllers.RoleController roleDbController,
					ActorRoleController actorRoleController)
		{
			_roleDbController = roleDbController;
			_actorRoleController = actorRoleController;
		}

		public List<Role> Get()
		{
			var roles = _roleDbController.Get();

			Logger.Info($"{roles.Count} Roles");

			return roles;
		}

		public Role GetById(int id)
		{
			var role = _roleDbController.Get(id);

			Logger.Info($"Role: {role?.Id} for Id: {id}");

			return role;
		}

		public List<Role> GetByScope(ClaimScope scope)
		{
			var roles = _roleDbController.Get(scope);

			Logger.Info($"{roles?.Count} Roles for {nameof(ClaimScope)}: {scope}");

			return roles;
		}

		public Role GetDefaultForScope(ClaimScope scope)
		{
			var role = _roleDbController.GetDefault(scope.ToString());

			Logger.Info($"{role?.Id} Role ID for {nameof(ClaimScope)}: {scope}");

			return role;
		}

		public Role Create(Role newRole, int creatorId)
		{
			newRole = _roleDbController.Create(newRole);
			_actorRoleController.Create(ClaimScope.Role.ToString(), creatorId, newRole.Id);

			Logger.Info($"Role: {newRole.Id} for CreatorId: {creatorId}");

			return newRole;
		}

		public void Delete(int id)
		{
			_roleDbController.Delete(id);

			Logger.Info($"{id}");
		}
	}
}
