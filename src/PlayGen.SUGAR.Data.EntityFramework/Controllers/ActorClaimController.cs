using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
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
				var claim = context.ActorClaims.Find(context, id);
				return claim;
			}
		}

		public IEnumerable<Claim> GetActorClaimsForEntity(int actorId, int? entityId)
		{
			using (var context = ContextFactory.Create())
			{
				var claims = context.ActorClaims.Where(ac => ac.ActorId == actorId && ac.EntityId.Value == entityId.Value).Select(ac => ac.Claim).ToList();
				return claims;
			}
		}

		public IEnumerable<Claim> GetActorClaims(int actorId)
		{
			using (var context = ContextFactory.Create())
			{
				var claims = context.ActorClaims.Where(ac => ac.ActorId == actorId).Select(ac => ac.Claim).ToList();
				return claims;
			}
		}

		public IEnumerable<Actor> GetClaimActors(int claimId, int? entityId)
		{
			using (var context = ContextFactory.Create())
			{
				var actors = context.ActorClaims.Where(ac => ac.ClaimId == claimId && ac.EntityId.Value == entityId.Value).Select(ac => ac.Actor).Distinct().ToList();
				return actors;
			}
		}

		public ActorClaim Create(ActorClaim actorClaim)
		{
			using (var context = ContextFactory.Create())
			{
				context.ActorClaims.Add(actorClaim);
				SaveChanges(context);

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
