using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using System.Linq;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class ActorClaimController
	{
		private readonly Data.EntityFramework.Controllers.ActorClaimController _actorClaimDbController;
		private readonly ActorRoleController _actorRoleController;
		private readonly RoleClaimController _roleClaimController;

		public ActorClaimController(Data.EntityFramework.Controllers.ActorClaimController actorClaimDbController,
					ActorRoleController actorRoleController,
					RoleClaimController roleClaimController)
		{
			_actorClaimDbController = actorClaimDbController;
			_actorRoleController = actorRoleController;
			_roleClaimController = roleClaimController;
		}

		public ActorClaim Get(int id)
		{
			var claim = _actorClaimDbController.Get(id);
			return claim;
		}

		public IEnumerable<Claim> GetActorClaims(int actorId)
		{
			var claims = _actorClaimDbController.GetActorClaims(actorId).ToList();
			var roles = _actorRoleController.GetActorRoles(actorId).Select(r => r.RoleId);
			var roleClaims = _roleClaimController.GetClaimsByRoles(roles);
			return claims.Concat(roleClaims).Distinct().ToList();
		}

		public IEnumerable<Claim> GetActorClaimsForEntity(int actorId, int? entityId)
		{
			var entityClaims = _actorClaimDbController.GetActorClaimsForEntity(actorId, entityId.Value).ToList();
			var adminClaims = _actorClaimDbController.GetActorClaimsForEntity(actorId, -1);
			var claims = entityClaims.Concat(adminClaims).Distinct();
			var roles = _actorRoleController.GetActorRolesForEntity(actorId, entityId.Value).Select(r => r.Id);
			var roleClaims = _roleClaimController.GetClaimsByRoles(roles);
			return claims.Concat(roleClaims).Distinct().ToList();
		}

		public IEnumerable<Actor> GetClaimActors(int claimId, int? entityId)
		{
			var claimActors = _actorClaimDbController.GetClaimActors(claimId, entityId.Value);
			var claimRoles = _roleClaimController.GetRolesByClaim(claimId).Select(cr => cr.Id);
			var roleActors = claimRoles.SelectMany(cr => _actorRoleController.GetRoleActors(cr, entityId.Value)).Distinct();
			return claimActors.Concat(roleActors).Distinct().ToList();
		}

		public ActorClaim Create(ActorClaim newClaim)
		{
			newClaim = _actorClaimDbController.Create(newClaim);
			return newClaim;
		}

		public void Delete(int id)
		{
			_actorClaimDbController.Delete(id);
		}
	}
}
