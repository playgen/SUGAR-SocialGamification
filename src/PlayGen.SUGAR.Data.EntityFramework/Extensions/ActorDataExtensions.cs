using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
    public static class ActorDataExtensions
    {
		public static IQueryable<ActorDetails> FilterByIds(this IQueryable<ActorDetails> actorDetails, List<int> ids)
		{
			return actorDetails.Where(gd => ids.Contains(gd.Id));
		}

		public static IQueryable<ActorDetails> FilterByActorId(this SUGARContext context, int? actorId)
		{
			return context.ActorDetails.Where(gd => gd.ActorId == actorId);
		}

		public static IQueryable<ActorDetails> FilterByGameId(this IQueryable<ActorDetails> actorDataQueryable, int? gameId)
		{
			if (!gameId.HasValue || gameId.Value == 0)
			{
				return actorDataQueryable.Where(gd => gd.GameId == null || gd.GameId == 0);
			}
			return actorDataQueryable.Where(gd => gd.GameId == gameId);
		}

		public static IQueryable<ActorDetails> FilterByKey(this IQueryable<ActorDetails> actorDataQueryable, string key)
		{
			return actorDataQueryable.Where(gd => gd.Key.Equals(key));
		}

		public static IQueryable<ActorDetails> FilterByKeys(this IQueryable<ActorDetails> actorDataQueryable, ICollection<string> keys)
		{
			var keyList = keys as List<string> ?? keys.ToList();
			if (keys != null && keyList.Any())
			{
				return actorDataQueryable.Where(gd => keyList.Contains(gd.Key));
			}
			return actorDataQueryable;
		}
	}
}
