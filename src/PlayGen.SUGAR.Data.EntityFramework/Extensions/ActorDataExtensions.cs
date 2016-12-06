using System.Collections.Generic;
using System.Linq;

using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
    public static class ActorDataExtensions
    {
		public static IQueryable<ActorData> FilterByIds(this SUGARContext context, List<int> ids)
		{
			return context.ActorData.Where(gd => ids.Contains(gd.Id));
		}

		public static IQueryable<ActorData> FilterByActorId(this SUGARContext context, int? actorId)
		{
			return context.ActorData.Where(gd => gd.ActorId == actorId);
		}

		public static IQueryable<ActorData> FilterByGameId(this IQueryable<ActorData> actorDataQueryable, int? gameId)
		{
			if (!gameId.HasValue || gameId.Value == 0)
			{
				return actorDataQueryable.Where(gd => gd.GameId == null || gd.GameId == 0);
			}
			return actorDataQueryable.Where(gd => gd.GameId == gameId);
		}

		public static IQueryable<ActorData> FilterByKey(this IQueryable<ActorData> actorDataQueryable, string key)
		{
			return actorDataQueryable.Where(gd => gd.Key.Equals(key));
		}

		public static IQueryable<ActorData> FilterByKeys(this IQueryable<ActorData> actorDataQueryable, ICollection<string> keys)
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
