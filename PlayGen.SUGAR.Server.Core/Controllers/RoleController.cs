using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class RoleController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.RoleController _roleDbController;
		private readonly ActorRoleController _actorRoleController;

		public RoleController(
			ILogger<RoleController> logger,
			EntityFramework.Controllers.RoleController roleDbController,
			ActorRoleController actorRoleController)
		{
			_logger = logger;
			_roleDbController = roleDbController;
			_actorRoleController = actorRoleController;
		}

		public List<Role> Get()
		{
			var roles = _roleDbController.Get();

			_logger.LogInformation($"{roles.Count} Roles");

			return roles;
		}

		public Role GetById(int id)
		{
			var role = _roleDbController.Get(id);

			_logger.LogInformation($"Role: {role?.Id} for Id: {id}");

			return role;
		}

		public List<Role> GetByScope(ClaimScope scope)
		{
			var roles = _roleDbController.Get(scope);

			_logger.LogInformation($"{roles?.Count} Roles for {nameof(ClaimScope)}: {scope}");

			return roles;
		}

		public Role GetDefaultForScope(ClaimScope scope)
		{
			var role = _roleDbController.GetDefault(scope);

			_logger.LogInformation($"{role?.Id} Role ID for {nameof(ClaimScope)}: {scope}");

			return role;
		}

		public Role Create(Role newRole, int creatorId)
		{
			newRole = _roleDbController.Create(newRole);
			_actorRoleController.Create(ClaimScope.Role, creatorId, newRole.Id);

			_logger.LogInformation($"Role: {newRole.Id} for CreatorId: {creatorId}");

			return newRole;
		}

		public void Delete(int id)
		{
			_roleDbController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}
