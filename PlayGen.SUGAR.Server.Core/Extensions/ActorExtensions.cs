using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Extensions
{
    public static class ActorExtensions
    {
		public static List<T> FilterPrivate<T>(this List<T> actors, ActorRoleController actorRoleController, int requestingId) where T : Actor
	    {
		    var includePrivate = actorRoleController.GetControlled(requestingId).Any(c => c.ClaimScope == ClaimScope.Global);
		    if (!includePrivate)
		    {
			    return actors.Where(a => !a.Private || a.Id == requestingId).ToList();
		    }
		    return actors;
	    }

	    public static T FilterPrivate<T>(this T actor, ActorRoleController actorRoleController, int requestingId) where T : Actor
	    {
		    var includePrivate = actorRoleController.GetControlled(requestingId).Any(c => c.ClaimScope == ClaimScope.Global);
		    if (!includePrivate)
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
