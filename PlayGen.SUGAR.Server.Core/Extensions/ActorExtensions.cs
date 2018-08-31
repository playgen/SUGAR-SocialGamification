using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Extensions
{
    public static class ActorExtensions
    {
		public static IEnumerable<T> FilterVisibility<T>(this List<T> actors, ActorVisibilityFilter filter) where T : Actor
	    {
			return actors
				.Where(a => IsNotFilteredOut(a, filter));
	    }

		public static T FilterVisibility<T>(this T actor, ActorVisibilityFilter filter) where T : Actor
		{
			return actor != null && IsNotFilteredOut(actor, filter) ? actor : null;
		}

	    public static bool IsNotFilteredOut(Actor actor, ActorVisibilityFilter filter)
		{
			// if private should be included OR this is not private
			return (filter & ActorVisibilityFilter.Private) != 0 || !actor.Private;
		}
    }
}
