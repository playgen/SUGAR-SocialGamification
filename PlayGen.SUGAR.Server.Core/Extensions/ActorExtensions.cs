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
			foreach (var actor in actors)
			{
				var hasClaim = requestingId != null &&
								(actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.User).Any() || actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.Group).Any());
				if (!hasClaim && requestingId != actor.Id && actor.Private)
				{
					actors.Remove(actor);
				}
			}
			 return actors;
	    }

	    public static T FilterPrivate<T>(this T actor,ActorClaimController actorClaimController, int? requestingId) where T : Actor
	    {
		    var hasClaim = requestingId  != null && 
				(actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.User).Any() || actorClaimController.GetActorClaimsForEntity(requestingId.Value, actor.Id, ClaimScope.Group).Any()); 

			if (!hasClaim)
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
