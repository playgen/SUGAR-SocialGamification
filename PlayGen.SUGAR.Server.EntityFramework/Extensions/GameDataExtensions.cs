using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Extensions
{
	public static class EvaluationDataExtensions
	{
		public static IQueryable<EvaluationData> GetCategoryData(this SUGARContext context, EvaluationDataCategory category)
		{
			return context.EvaluationData.Where(data => data.Category == category);
		}

		public static IQueryable<EvaluationData> FilterByIds(this IQueryable<EvaluationData> gameDataQueryable, ICollection<int> ids)
		{
			return gameDataQueryable.Where(data => ids.Contains(data.Id));
		}

		public static IQueryable<EvaluationData> FilterByGameId(this IQueryable<EvaluationData> gameDataQueryable, int? gameId)
		{
			if (!gameId.HasValue || gameId.Value == 0)
			{
				return gameDataQueryable.Where(data => data.GameId == null || data.GameId == 0);
			}
			return gameDataQueryable.Where(data => data.GameId == gameId);
		}

		public static IQueryable<EvaluationData> FilterByMatchId(this IQueryable<EvaluationData> gameDataQueryable, int? entityId)
		{
			if (!entityId.HasValue || entityId.Value == 0)
			{
				return gameDataQueryable.Where(data => data.MatchId == null || data.MatchId == 0);
			}
			return gameDataQueryable.Where(data => data.MatchId == entityId);
		}

		public static IQueryable<EvaluationData> FilterByActorId(this IQueryable<EvaluationData> gameDataQueryable, int? actorId)
		{
			return gameDataQueryable.Where(data => data.ActorId == actorId);
		}

		public static IQueryable<EvaluationData> FilterByKey(this IQueryable<EvaluationData> gameDataQueryable, string key)
		{
			return gameDataQueryable.Where(data => data.Key.Equals(key));
		}

		public static IQueryable<EvaluationData> FilterByKeys(this IQueryable<EvaluationData> gameDataQueryable, ICollection<string> keys)
		{
			if (keys != null && keys.Any())
			{
				return gameDataQueryable.Where(data => keys.Contains(data.Key));
			}
			return gameDataQueryable;
		}

		public static IQueryable<EvaluationData> FilterByDataType(this IQueryable<EvaluationData> gameDataQueryable, EvaluationDataType type)
		{
			return gameDataQueryable.Where(data => data.EvaluationDataType == type);
		}

		public static IQueryable<EvaluationData> FilterByDateTimeRange(this IQueryable<EvaluationData> gameDataQueryable, DateTime start, DateTime end)
		{
			return gameDataQueryable.Where(data => data.DateModified >= start && data.DateModified <= end);
		}

		public static EvaluationData LatestOrDefault(this IQueryable<EvaluationData> gameDataQueryable)
		{
			return gameDataQueryable
				.OrderByDescending(s => s.DateModified)
				.FirstOrDefault();
		}
	}
}