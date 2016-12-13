using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
	public static class SaveDataExtensions
	{
		public static IQueryable<SaveData> GetCategoryData(this SUGARContext context, SaveDataCategory category)
		{
			return context.SaveData.Where(gd => gd.Category == category);
		}

		public static IQueryable<SaveData> FilterByIds(this IQueryable<SaveData> gameDataQueryable, ICollection<int> ids)
		{
			return gameDataQueryable.Where(gd => ids.Contains(gd.Id));
		}

		public static IQueryable<SaveData> FilterByGameId(this IQueryable<SaveData> gameDataQueryable, int? gameId)
		{
			if (!gameId.HasValue || gameId.Value == 0)
			{
				return gameDataQueryable.Where(gd => gd.GameId == null || gd.GameId == 0);
			}
			return gameDataQueryable.Where(gd => gd.GameId == gameId);
		}

		public static IQueryable<SaveData> FilterByActorId(this IQueryable<SaveData> gameDataQueryable, int? actorId)
		{
			return gameDataQueryable.Where(gd => gd.ActorId == actorId);
		}

		public static IQueryable<SaveData> FilterByKey(this IQueryable<SaveData> gameDataQueryable, string key)
		{
			return gameDataQueryable.Where(gd => gd.Key.Equals(key));
		}

		public static IQueryable<SaveData> FilterByKeys(this IQueryable<SaveData> gameDataQueryable, ICollection<string> keys)
		{
		    if (keys != null && keys.Any())
			{
				return gameDataQueryable.Where(gd => keys.Contains(gd.Key));
			}
			return gameDataQueryable;
		}

		public static IQueryable<SaveData> FilterByDataType(this IQueryable<SaveData> gameDataQueryable, SaveDataType type)
		{
			return gameDataQueryable.Where(gd => gd.SaveDataType == type);
		}

		public static IQueryable<SaveData> FilterByDateTimeRange(this IQueryable<SaveData> gameDataQueryable, DateTime start, DateTime end)
		{
			return gameDataQueryable.Where(gd => gd.DateModified >= start && gd.DateModified <= end);
		}

		public static SaveData LatestOrDefault(this IQueryable<SaveData> gameDataQueryable)
		{
			return gameDataQueryable
				.OrderByDescending(s => s.DateModified)
				.FirstOrDefault();
		}
	}
}