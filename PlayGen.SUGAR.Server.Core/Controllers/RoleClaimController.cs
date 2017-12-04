using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class RoleClaimController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly EntityFramework.Controllers.RoleClaimController _roleClaimDbController;

		public RoleClaimController(EntityFramework.Controllers.RoleClaimController roleClaimDbController)
		{
			_roleClaimDbController = roleClaimDbController;
		}

		public List<Claim> GetClaimsByRole(int roleId)
		{
			var roles = _roleClaimDbController.GetClaimsByRole(roleId);

			Logger.Info($"{roles.Count} Roles for RoleId: {roleId}");

			return roles;
		}

		public List<Claim> GetClaimsByRoles(List<int> ids)
		{
			var roles = _roleClaimDbController.GetClaimsByRoles(ids);

			Logger.Info($"{roles.Count} Roles for Ids: {string.Join(", ", ids)}");

			return roles;
		}

		public List<Role> GetRolesByClaim(int id)
		{
			var claims = _roleClaimDbController.GetRolesByClaim(id);

			Logger.Info($"{claims.Count} Claims for Id: {id}");

			return claims;
		}

		public RoleClaim Create(RoleClaim newRoleClaim)
		{
			newRoleClaim = _roleClaimDbController.Create(newRoleClaim);

			Logger.Info($"RoleId: {newRoleClaim?.RoleId}, ClaimId: {newRoleClaim?.ClaimId}");

			return newRoleClaim;
		}

		public void Delete(int roleId, int claimId)
		{
			_roleClaimDbController.Delete(roleId, claimId);

			Logger.Info($"RoleId: {roleId}, ClaimId: {claimId}");
		}
	}
}
