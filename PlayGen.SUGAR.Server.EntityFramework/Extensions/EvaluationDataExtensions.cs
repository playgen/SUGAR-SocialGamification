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

		public static IQueryable<EvaluationData> FilterByIds(this IQueryable<EvaluationData> evaluationDataQueryable,
			ICollection<int> ids)
		{
			return evaluationDataQueryable.Where(data => ids.Contains(data.Id));
		}

		public static IQueryable<EvaluationData> FilterByGameId(this IQueryable<EvaluationData> evaluationDataQueryable,
			int gameId)
		{
			return evaluationDataQueryable.Where(data => data.GameId == gameId);
		}

		public static IQueryable<EvaluationData> FilterByMatchId(this IQueryable<EvaluationData> evaluationDataQueryable,
			int? entityId)
		{
			if (entityId == null || entityId.Value == 0)
			{
				return evaluationDataQueryable.Where(data => data.MatchId == null || data.MatchId == 0);
			}
			return evaluationDataQueryable.Where(data => data.MatchId == entityId);
		}

		public static IQueryable<EvaluationData> FilterByActorId(this IQueryable<EvaluationData> evaluationDataQueryable,
			int? actorId)
		{
			return evaluationDataQueryable.Where(data => data.ActorId == actorId);
		}

		public static IQueryable<EvaluationData> FilterByKey(this IQueryable<EvaluationData> evaluationDataQueryable, string key)
		{
			return evaluationDataQueryable.Where(data => data.Key.Equals(key));
		}

		public static IQueryable<EvaluationData> FilterByKeys(this IQueryable<EvaluationData> evaluationDataQueryable,
			ICollection<string> keys)
		{
			if (keys != null && keys.Any())
			{
				return evaluationDataQueryable.Where(data => keys.Contains(data.Key));
			}
			return evaluationDataQueryable;
		}

		public static IQueryable<EvaluationData> FilterByDataType(this IQueryable<EvaluationData> evaluationDataQueryable,
			EvaluationDataType type)
		{
			return evaluationDataQueryable.Where(data => data.EvaluationDataType == type);
		}

		public static IQueryable<EvaluationData> FilterByDateTimeRange(
			this IQueryable<EvaluationData> evaluationDataQueryable,
			DateTime? start, 
			DateTime? end)
		{
			var modifiedEvaluationDataQueryable = evaluationDataQueryable;

            if (start.HasValue)
			{
				if (start != default(DateTime)) // todo: Remove this check for v2 of the API. It has been added for compatibility with a previous version of the V1 data contracts
				{
					modifiedEvaluationDataQueryable =
						modifiedEvaluationDataQueryable.Where(data => data.DateModified >= start);
				}
			}

			if (end.HasValue)
			{
				if (end != default(DateTime)) // todo: Remove this check for v2 of the API. It has been added for compatibility with a previous version of the V1 data contracts
				{
					modifiedEvaluationDataQueryable = evaluationDataQueryable.Where(data => data.DateModified <= end);
				}
			}

			return modifiedEvaluationDataQueryable;

		}
	}
}