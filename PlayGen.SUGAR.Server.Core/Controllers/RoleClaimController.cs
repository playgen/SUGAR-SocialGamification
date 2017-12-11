using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class RoleClaimController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.RoleClaimController _roleClaimDbController;

		public RoleClaimController(
			ILogger<RoleClaimController> logger,
			EntityFramework.Controllers.RoleClaimController roleClaimDbController)
		{
			_logger = logger;
			_roleClaimDbController = roleClaimDbController;
		}

		public List<Claim> GetClaimsByRole(int roleId)
		{
			var roles = _roleClaimDbController.GetClaimsByRole(roleId);

			_logger.LogInformation($"{roles.Count} Roles for RoleId: {roleId}");

			return roles;
		}

		public List<Claim> GetClaimsByRoles(List<int> ids)
		{
			var roles = _roleClaimDbController.GetClaimsByRoles(ids);

			_logger.LogInformation($"{roles.Count} Roles for Ids: {string.Join(", ", ids)}");

			return roles;
		}

		public List<Role> GetRolesByClaim(int id)
		{
			var claims = _roleClaimDbController.GetRolesByClaim(id);

			_logger.LogInformation($"{claims.Count} Claims for Id: {id}");

			return claims;
		}

		public RoleClaim Create(RoleClaim newRoleClaim)
		{
			newRoleClaim = _roleClaimDbController.Create(newRoleClaim);

			_logger.LogInformation($"RoleId: {newRoleClaim?.RoleId}, ClaimId: {newRoleClaim?.ClaimId}");

			return newRoleClaim;
		}

		public void Delete(int roleId, int claimId)
		{
			_roleClaimDbController.Delete(roleId, claimId);

			_logger.LogInformation($"RoleId: {roleId}, ClaimId: {claimId}");
		}
	}
}
