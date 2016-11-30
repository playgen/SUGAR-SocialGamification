using System;
using System.Collections.Generic;

using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class RoleController
	{
		private readonly Data.EntityFramework.Controllers.RoleController _roleDbController;
		private readonly ActorRoleController _actorRoleController;

		public RoleController(Data.EntityFramework.Controllers.RoleController roleDbController,
					ActorRoleController actorRoleController)
		{
			_roleDbController = roleDbController;
			_actorRoleController = actorRoleController;
		}

		public IEnumerable<Role> Get()
		{
			var roles = _roleDbController.Get();
			return roles;
		}

		public Role GetByName(string name)
		{
			var role = _roleDbController.Get(name);
			return role;
		}

		public Role GetById(int id)
		{
			var role = _roleDbController.Get(id);
			return role;
		}

		public IEnumerable<Role> GetByScope(ClaimScope scope)
		{
			var roles = _roleDbController.Get(scope);
			return roles;
		}

		public Role Create(Role newRole, int creatorId)
		{
			if (newRole.Name == newRole.ClaimScope.ToString())
			{
				throw new ArgumentException("Roles cannot be created with the same name as the ClaimScope they are for.");
			}
			newRole = _roleDbController.Create(newRole);
			_actorRoleController.Create(ClaimScope.Role.ToString(), creatorId, newRole.Id);
			return newRole;
		}

		public void Delete(int id)
		{
			_roleDbController.Delete(id);
		}
	}
}
