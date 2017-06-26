using System.Collections.Generic;
using System.Linq;
using NLog;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class ActorClaimController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Data.EntityFramework.Controllers.ActorClaimController _actorClaimDbController;
		private readonly ActorRoleController _actorRoleController;
		private readonly GroupMemberController _groupMemberController;
		private readonly RoleClaimController _roleClaimController;

		public ActorClaimController(Data.EntityFramework.Controllers.ActorClaimController actorClaimDbController,
			ActorRoleController actorRoleController,
			RoleClaimController roleClaimController,
			GroupMemberController groupMemberController)
		{
			_actorClaimDbController = actorClaimDbController;
			_actorRoleController = actorRoleController;
			_roleClaimController = roleClaimController;
			_groupMemberController = groupMemberController;
		}

		public ActorClaim Get(int id)
		{
			var claim = _actorClaimDbController.Get(id);

			Logger.Info($"Claim {claim?.Id} for Id: {id}");

			return claim;
		}

		public List<ActorClaim> GetActorClaims(int actorId)
		{
			var claims = _actorClaimDbController.GetActorClaims(actorId)
				.ToList();
			var groups = _groupMemberController.GetUserGroups(actorId)
				.ToList();
			var groupClaims = groups.SelectMany(g => GetActorClaims(g.Id))
				.Distinct()
				.ToList();
			var roles = _actorRoleController.GetActorRoles(actorId, true);

			var roleClaims = roles
				.Select(r => new
				{
					actorRole = r,
					claims = r.Role.RoleClaims.Select(rc => rc.Claim)
				})
				.SelectMany(x => x.claims.Select(c => new ActorClaim
				{
					ActorId = x.actorRole.ActorId,
					EntityId = x.actorRole.EntityId,
					ClaimId = c.Id,
					Claim = c
				}));

			var results = claims.Concat(roleClaims)
				.Concat(groupClaims)
				.Distinct()
				.ToList();

			Logger.Info($"{results?.Count} Claims for ActorId: {actorId}");

			return results;
		}

		public List<ActorClaim> GetActorClaimsByScope(int actorId, ClaimScope scope)
		{
			var claims = GetActorClaims(actorId)
				.ToList();
			claims = claims.Where(c => c.Claim.ClaimScope == scope)
				.ToList();

			Logger.Info($"{claims?.Count} Actor Claims for ActorId: {actorId}, {nameof(ClaimScope)}: {scope}");

			return claims;
		}

		public List<Claim> GetActorClaimsForEntity(int actorId, int? entityId, ClaimScope scope)
		{
			var claims = _actorClaimDbController.GetActorClaimsForEntity(actorId, entityId.Value, scope)
				.ToList();
			var groups = _groupMemberController.GetUserGroups(actorId)
				.ToList();
			var groupClaims = groups.SelectMany(g => GetActorClaimsForEntity(g.Id, entityId.Value, scope))
				.Distinct()
				.ToList();
			var roleClaims = _actorRoleController.GetActorRolesForEntity(actorId, entityId.Value, scope)
				.SelectMany(r => r.RoleClaims)
				.Select(rc => rc.Claim)
				.ToList();
			var totalClaims = claims.Concat(roleClaims)
				.Concat(groupClaims)
				.Distinct()
				.ToList();

			Logger.Info(
				$"{totalClaims?.Count} Claims for ActorId: {actorId}, EntityId: {entityId}, {nameof(ClaimScope)}: {scope}");

			return totalClaims;
		}

		public List<Actor> GetClaimActors(int claimId, int? entityId)
		{
			var claimActors = _actorClaimDbController.GetClaimActors(claimId, entityId.Value);
			var claimRoles = _roleClaimController.GetRolesByClaim(claimId)
				.Select(cr => cr.Id);
			var roleActors = claimRoles.SelectMany(cr => _actorRoleController.GetRoleActors(cr, entityId.Value))
				.Distinct();

			var results = claimActors.Concat(roleActors)
				.Distinct()
				.ToList();

			Logger.Info($"{results?.Count} Actors for ClaimId: {claimId}, EntityId: {entityId}");

			return results;
		}

		public ActorClaim Create(ActorClaim newClaim)
		{
			newClaim = _actorClaimDbController.Create(newClaim);

			Logger.Info($"{newClaim?.Id}");

			return newClaim;
		}

		public void Delete(int id)
		{
			_actorClaimDbController.Delete(id);

			Logger.Info($"{id}");
		}
	}
}