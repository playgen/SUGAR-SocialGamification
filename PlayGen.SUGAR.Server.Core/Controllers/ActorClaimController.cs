using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class ActorClaimController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.ActorClaimController _actorClaimDbController;
		private readonly ActorRoleController _actorRoleController;
		private readonly RoleClaimController _roleClaimController;

		public ActorClaimController(
			ILogger<ActorClaimController> logger,
			EntityFramework.Controllers.ActorClaimController actorClaimDbController,
			ActorRoleController actorRoleController,
			RoleClaimController roleClaimController)
		{
			_logger = logger;
			_actorClaimDbController = actorClaimDbController;
			_actorRoleController = actorRoleController;
			_roleClaimController = roleClaimController;
		}

		public ActorClaim Get(int id)
		{
			var claim = _actorClaimDbController.Get(id);

			_logger.LogInformation($"Claim {claim?.Id} for Id: {id}");

			return claim;
		}

		public List<ActorClaim> GetActorClaims(int actorId)
		{
			var claims = _actorClaimDbController.GetActorClaims(actorId);
			var roles = _actorRoleController.GetActorRoles(actorId, true);

			var roleClaims = roles
				.Select(r => new { actorRole = r, claims = r.Role.RoleClaims.Select(rc => rc.Claim) })
				.SelectMany(x => x.claims.Select(c => new ActorClaim { ActorId = x.actorRole.ActorId, EntityId = x.actorRole.EntityId, ClaimId = c.Id, Claim = c }));

			var results = claims.Concat(roleClaims).Distinct().ToList();

			_logger.LogInformation($"{results.Count} Claims for ActorId: {actorId}");

			return results;
		}

		public List<ActorClaim> GetActorClaimsByScope(int actorId, ClaimScope scope)
		{
			var claims = GetActorClaims(actorId).ToList();
			claims = claims.Where(c => c.Claim.ClaimScope == scope).ToList();

			_logger.LogInformation($"{claims.Count} Actor Claims for ActorId: {actorId}, {nameof(ClaimScope)}: {scope}");

			return claims;
		}

		public List<Claim> GetActorClaimsForEntity(int actorId, int entityId, ClaimScope scope)
		{
			var claims = _actorClaimDbController.GetActorClaimsForEntity(actorId, entityId, scope);
			var roleClaims = _actorRoleController.GetActorRolesForEntity(actorId, entityId, scope).SelectMany(r => r.RoleClaims).Select(rc => rc.Claim).ToList();
			var totalClaims = claims.Concat(roleClaims).Distinct().ToList();

			_logger.LogInformation($"{totalClaims.Count} Claims for ActorId: {actorId}, EntityId: {entityId}, {nameof(ClaimScope)}: {scope}");

			return totalClaims;
		}

		public List<Actor> GetClaimActors(int claimId, int entityId)
		{
			var claimActors = _actorClaimDbController.GetClaimActors(claimId, entityId);
			var claimRoles = _roleClaimController.GetRolesByClaim(claimId).Select(cr => cr.Id);
			var roleActors = claimRoles.SelectMany(cr => _actorRoleController.GetRoleActors(cr, entityId)).Distinct();

			var results = claimActors.Concat(roleActors).Distinct().ToList();

			_logger.LogInformation($"{results.Count} Actors for ClaimId: {claimId}, EntityId: {entityId}");

			return results;
		}

		public ActorClaim Create(ActorClaim newClaim)
		{
			newClaim = _actorClaimDbController.Create(newClaim);

			_logger.LogInformation($"{newClaim?.Id}");

			return newClaim;
		}

		public void Delete(int id)
		{
			_actorClaimDbController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}
