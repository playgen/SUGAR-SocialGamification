using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
	public static class GameDataExtensions
	{
		public static IQueryable<EvaluationData> GetCategoryData(this SUGARContext context, GameDataCategory category)
		{
			return context.EvaluationData.Where(gd => gd.Category == category);
		}

		public static IQueryable<EvaluationData> FilterByIds(this IQueryable<EvaluationData> gameDataQueryable, List<int> ids)
		{
			return gameDataQueryable.Where(gd => ids.Contains(gd.Id));
		}

		public static IQueryable<EvaluationData> FilterByGameId(this IQueryable<EvaluationData> gameDataQueryable, int? gameId)
		{
			if (!gameId.HasValue || gameId.Value == 0)
			{
				return gameDataQueryable.Where(gd => gd.GameId == null || gd.GameId == 0);
			}
			return gameDataQueryable.Where(gd => gd.GameId == gameId);
		}

		public static IQueryable<EvaluationData> FilterByActorId(this IQueryable<EvaluationData> gameDataQueryable, int? actorId)
		{
			return gameDataQueryable.Where(gd => gd.ActorId == actorId);
		}

		public static IQueryable<EvaluationData> FilterByKey(this IQueryable<EvaluationData> gameDataQueryable, string key)
		{
			return gameDataQueryable.Where(gd => gd.Key.Equals(key));
		}

		public static IQueryable<EvaluationData> FilterByKeys(this IQueryable<EvaluationData> gameDataQueryable, ICollection<string> keys)
		{
		    if (keys != null && keys.Any())
			{
				return gameDataQueryable.Where(gd => keys.Contains(gd.Key));
			}
			return gameDataQueryable;
		}

		public static IQueryable<EvaluationData> FilterByDataType(this IQueryable<EvaluationData> gameDataQueryable, SaveDataType type)
		{
			return gameDataQueryable.Where(gd => gd.SaveDataType == type);
		}

		public static IQueryable<EvaluationData> FilterByDateTimeRange(this IQueryable<EvaluationData> gameDataQueryable, DateTime start, DateTime end)
		{
			return gameDataQueryable.Where(gd => gd.DateModified >= start && gd.DateModified <= end);
		}

		public static EvaluationData LatestOrDefault(this IQueryable<EvaluationData> gameDataQueryable)
		{
			return gameDataQueryable
				.OrderByDescending(s => s.DateModified)
				.FirstOrDefault();
		}
	}
}