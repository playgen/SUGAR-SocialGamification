using System.Collections.Generic;
using System.Linq;

using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
    public static class GameDetailsExtensions
	{
		public static IQueryable<GameDetails> FilterByIds(this IQueryable<GameDetails> actorDetails, List<int> ids)
		{
			return actorDetails.Where(gd => ids.Contains(gd.Id));
		}

		public static IQueryable<GameDetails> FilterByGameId(this IQueryable<GameDetails> actorDetails, int gameId)
		{
			return actorDetails.Where(gd => gd.GameId == gameId);
		}

		public static IQueryable<GameDetails> FilterByKey(this IQueryable<GameDetails> actorDataQueryable, string key)
		{
			return actorDataQueryable.Where(gd => gd.Key.Equals(key));
		}

		public static IQueryable<GameDetails> FilterByKeys(this IQueryable<GameDetails> actorDataQueryable, ICollection<string> keys)
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
