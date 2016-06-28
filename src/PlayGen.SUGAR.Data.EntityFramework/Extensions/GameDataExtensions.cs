using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
	public static class GameDataExtensions
	{
		public static IQueryable<GameData> GetData(this SGAContext context)
		{
			return context.GameData;
		}

		public static IQueryable<GameData> FilterByGameId(this IQueryable<GameData> gameDataQueryable, int? gameId)
		{
			return gameDataQueryable.Where(gd => gameId.HasValue == false || (gd.GameId.HasValue && gd.GameId.Value == gameId.Value));
		}

		public static IQueryable<GameData> FilterByActorId(this IQueryable<GameData> gameDataQueryable, int? actorId)
		{
			return gameDataQueryable.Where(gd => actorId.HasValue == false || (gd.ActorId.HasValue && gd.ActorId.Value == actorId.Value));
		}

		public static IQueryable<GameData> FilterByKey(this IQueryable<GameData> gameDataQueryable, string key)
		{
			return gameDataQueryable.Where(gd => gd.Key.Equals(key));
		}

		public static IQueryable<GameData> FilterByKeys(this IQueryable<GameData> gameDataQueryable, IEnumerable<string> keys)
		{
			return gameDataQueryable.Where(gd => keys.Contains(gd.Key));
		}

		public static IQueryable<GameData> FilterByDataType(this IQueryable<GameData> gameDataQueryable, GameDataType type)
		{
			return gameDataQueryable.Where(gd => gd.DataType == type);
		}

		public static GameData LatestOrDefault(this IQueryable<GameData> gameDataQueryable)
		{
			return gameDataQueryable
				.OrderByDescending(s => s.DateModified)
				.FirstOrDefault();
		}
	}
}