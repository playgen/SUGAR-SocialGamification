using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class ActorClaimController : DbController
	{
		public ActorClaimController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public ActorClaim Get(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var claim = context.ActorClaims.Find(id);
				return claim;
			}
		}

		public List<Claim> GetActorClaimsForEntity(int actorId, int entityId, ClaimScope scope)
		{
			using (var context = ContextFactory.Create())
			{
				var claims = context.ActorClaims
					.Where(ac => ac.ActorId == actorId && (ac.EntityId == entityId || ac.EntityId == Platform.AllId))
					.Select(ac => ac.Claim)
					.Where(c => c.ClaimScope == scope).ToList();
				return claims;
			}
		}

		public List<ActorClaim> GetActorClaims(int actorId)
		{
			using (var context = ContextFactory.Create())
			{
				var claims = context.ActorClaims.Include(c => c.Claim).Where(ac => ac.ActorId == actorId).ToList();
				return claims;
			}
		}

		public List<Actor> GetClaimActors(int claimId, int entityId)
		{
			using (var context = ContextFactory.Create())
			{
				var actors = context.ActorClaims.Where(ac => ac.ClaimId == claimId && ac.EntityId == entityId).Select(ac => ac.Actor).Distinct().ToList();
				return actors;
			}
		}

		public ActorClaim Create(ActorClaim actorClaim)
		{
			using (var context = ContextFactory.Create())
			{
				context.ActorClaims.Add(actorClaim);
				SaveChanges(context);
				actorClaim.Claim = context.Claims.Find(actorClaim.ClaimId);

				return actorClaim;
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var actorClaim = context.ActorClaims
					.Where(r => id == r.Id);

				context.ActorClaims.RemoveRange(actorClaim);
				SaveChanges(context);
			}
		}
	}
}
