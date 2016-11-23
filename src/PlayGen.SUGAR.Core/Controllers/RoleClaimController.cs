using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class RoleClaimController
	{
		private readonly Data.EntityFramework.Controllers.RoleClaimController _roleClaimDbController;

		public RoleClaimController(Data.EntityFramework.Controllers.RoleClaimController roleClaimDbController)
		{
			_roleClaimDbController = roleClaimDbController;
		}

		public IEnumerable<Claim> GetClaimsByRole(int roleId)
		{
			var roles = _roleClaimDbController.GetClaimsByRole(roleId);
			return roles;
		}

		public IEnumerable<Claim> GetClaimsByRoles(IEnumerable<int> ids)
		{
			var roles = _roleClaimDbController.GetClaimsByRoles(ids);
			return roles;
		}

		public IEnumerable<Role> GetRolesByClaim(int id)
		{
			var claims = _roleClaimDbController.GetRolesByClaim(id);
			return claims;
		}

		public RoleClaim Create(RoleClaim newRoleClaim)
		{
			newRoleClaim = _roleClaimDbController.Create(newRoleClaim);
			return newRoleClaim;
		}

		public void Delete(int roleID, int claim)
		{
			_roleClaimDbController.Delete(roleID, claim);
		}
	}
}
