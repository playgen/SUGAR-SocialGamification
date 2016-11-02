using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
	public static class GameDataExtensions
	{
		public static IQueryable<GameData> GetCategoryData(this SUGARContext context, GameDataCategory category)
		{
			return context.GameData.Where(gd => gd.Category == category);
		}

		public static IQueryable<GameData> FilterByIds(this IQueryable<GameData> gameDataQueryable, IEnumerable<int> ids)
		{
			return gameDataQueryable.Where(gd => ids.Contains(gd.Id));
		}

		public static IQueryable<GameData> FilterByGameId(this IQueryable<GameData> gameDataQueryable, int? gameId)
		{
			if (!gameId.HasValue || gameId.Value == 0)
			{
				return gameDataQueryable.Where(gd => gd.GameId == null || gd.GameId == 0);
			}
			return gameDataQueryable.Where(gd => gd.GameId == gameId);
		}

		public static IQueryable<GameData> FilterByActorId(this IQueryable<GameData> gameDataQueryable, int? actorId)
		{
			return gameDataQueryable.Where(gd => gd.ActorId == actorId);
		}

		public static IQueryable<GameData> FilterByKey(this IQueryable<GameData> gameDataQueryable, string key)
		{
			return gameDataQueryable.Where(gd => gd.Key.Equals(key));
		}

		public static IQueryable<GameData> FilterByKeys(this IQueryable<GameData> gameDataQueryable, IEnumerable<string> keys)
		{
			if (keys != null && keys.Any())
			{
				return gameDataQueryable.Where(gd => keys.Contains(gd.Key));
			}
			return gameDataQueryable;
		}

		public static IQueryable<GameData> FilterByDataType(this IQueryable<GameData> gameDataQueryable, GameDataType type)
		{
			return gameDataQueryable.Where(gd => gd.DataType == type);
		}

		public static IQueryable<GameData> FilterByDateTimeRange(this IQueryable<GameData> gameDataQueryable, DateTime start, DateTime end)
		{
			return gameDataQueryable.Where(gd => gd.DateModified >= start && gd.DateModified <= end);
		}

		public static GameData LatestOrDefault(this IQueryable<GameData> gameDataQueryable)
		{
			return gameDataQueryable
				.OrderByDescending(s => s.DateModified)
				.FirstOrDefault();
		}
	}
}