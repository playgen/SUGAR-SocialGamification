using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.Model;
// ReSharper disable SimplifyLinqExpression

namespace PlayGen.SUGAR.Server.Core.Extensions
{
    public static class ActorExtensions
    {
		public static List<T> FilterPrivate<T>(this List<T> actors, ActorClaimController actorClaimController, int? requestingId) where T : Actor
	    {
			// TODO iterate over list
			// admins have ClaimScope.Global, they can see all actors
			foreach (var actor in actors)
			{
				var hasClaim = actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.User).Any() || actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.Group).Any();
				if (!hasClaim && requestingId != actor.Id && actor.Private)
				{
					actors.Remove(actor);
				}
			}

		 //   var claims = actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.User).Any() || actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.Group).Any(); ;

			//if (requestingId == null || !actorClaimController.GetControlled(requestingId.Value).Any(c => c.ClaimScope == ClaimScope.Global))
			//{
			//    return actors.Where(a => !a.Private || a.Id == requestingId).ToList();
		 //   }
			 return actors;
	    }

	    public static T FilterPrivate<T>(this T actor,ActorClaimController actorClaimController, int? requestingId) where T : Actor
	    {
		    var hasClaim = actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.User).Any() || actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.Group).Any(); ;

			if (requestingId == null || !hasClaim)
			{
				if (actor != null && actor.Private && actor.Id != requestingId)
				{
					return null;
				}
			}
			return actor;
	    }
	}
}
