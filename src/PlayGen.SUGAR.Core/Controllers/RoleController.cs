using System;
using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class RoleController
	{
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Data.EntityFramework.Controllers.RoleController _roleDbController;
		private readonly ActorRoleController _actorRoleController;

		public RoleController(Data.EntityFramework.Controllers.RoleController roleDbController,
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

		public Role Create(Role newRole, int creatorId)
		{
			newRole = _roleDbController.Create(newRole);
			_actorRoleController.Create(ClaimScope.Role.ToString(), creatorId, newRole.Id);

            Logger.Info($"Role: {newRole?.Id} for CreatorId: {creatorId}");

			return newRole;
		}

		public void Delete(int id)
		{
			_roleDbController.Delete(id);

            Logger.Info($"{id}");
		}
	}
}
