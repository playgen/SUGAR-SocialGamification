using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework.Extensions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class EvaluationDataController : DbController
	{
		private readonly EvaluationDataCategory _category = EvaluationDataCategory.GameData;

		[Obsolete]
		public EvaluationDataController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public EvaluationDataController(SUGARContextFactory contextFactory, EvaluationDataCategory category)
			: base(contextFactory)
		{
			_category = category;
		}

		public bool KeyExists(int gameId, int actorId, string key, EvaluationDataType evaluationDataType, DateTime start, DateTime end)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(evaluationDataType)
					.FilterByDateTimeRange(start, end)
					.Any();
			}
		}

		public List<EvaluationData> Get(ICollection<int> ids)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByIds(ids)
					.ToList();
			}
		}

		public List<EvaluationData> Get(int gameId, int actorId, ICollection<string> keys = null)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKeys(keys)
					.ToList();
			}
		}

		public List<EvaluationData> Get(int entityId, ICollection<string> keys = null)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByMatchId(entityId)
					.FilterByKeys(keys)
					.ToList();
			}
		}

		public List<EvaluationData> Get(int gameId, int actorId, int? entityId = null, ICollection<string> keys = null)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByMatchId(entityId)
					.FilterByActorId(actorId)
					.FilterByKeys(keys)
					.ToList();
			}
		}

		public List<int> GetGameActors(int gameId)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.Select(d => d.ActorId)
					.Distinct()
					.ToList();
			}
		}

		public List<int> GetGameKeyActors(int gameId, string key, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByKey(key)
					.FilterByDataType(evaluationDataType)
					.FilterByDateTimeRange(start, end)
					.Select(d => d.ActorId)
					.Distinct()
					.ToList();
			}
		}

		public List<KeyValuePair<string, EvaluationDataType>> GetGameKeys(int gameId)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.Select(d => new KeyValuePair<string, EvaluationDataType>(d.Key, d.EvaluationDataType))
					.Distinct()
					.ToList();
			}
		}

		public List<EvaluationData> GetActorData(int actorId)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByActorId(actorId)
					.ToList();
			}
		}

		public List<EvaluationData> List(int gameId, int actorId, string key, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			using (var context = ContextFactory.CreateReadOnly())
			{
				return Query(context, gameId, actorId, key, evaluationDataType, start, end).ToList();
			}
        }
		
        public IQueryable<EvaluationData> Query(SUGARContext context, int gameId, int actorId, string key, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{		
			return context.GetCategoryData(_category)
				.FilterByGameId(gameId)
				.FilterByActorId(actorId)
				.FilterByKey(key)
				.FilterByDataType(evaluationDataType)
				.FilterByDateTimeRange(start, end);
		}

		public float SumFloat(int gameId, int actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			using (var context = ContextFactory.CreateReadOnly())
			{
				var matchingData = Query(context, gameId, actorId, key, EvaluationDataType.Float, start, end);
				return matchingData.Sum(s => Convert.ToSingle(s.Value));
			}
		}

        public long SumLong(int gameId, int actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			using (var context = ContextFactory.CreateReadOnly())
			{
				var matchingData = Query(context, gameId, actorId, key, EvaluationDataType.Long, start, end);
				return matchingData.Sum(s => Convert.ToInt64(s.Value));
			}
		}

		public bool TryGetMax(int gameId, int actorId, string key, out EvaluationData value, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var list = List(gameId, actorId, key, evaluationDataType, start, end);
			if (list.Count > 0)
			{
				value = list.OrderByDescending(s => Convert.ToDouble(s.Value)).First();
				return true;
			}
			value = null;
			return false;
		}

		public bool TryGetMin(int gameId, int actorId, string key, out EvaluationData value, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var list = List(gameId, actorId, key, evaluationDataType, start, end);
			if (list.Count > 0)
			{
				value = list.OrderBy(s => Convert.ToDouble(s.Value)).First();
				return true;
			}
			value = null;
			return false;
		}

		public bool TryGetLatest(int gameId, int actorId, string key, out EvaluationData value, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var list = List(gameId, actorId, key, evaluationDataType, start, end);
			if (list.Count > 0)
			{
				value = list.OrderByDescending(s => s.DateModified).First();
				return true;
			}
			value = null;
			return false;
		}

		public bool TryGetEarliest(int gameId, int actorId, string key, out EvaluationData value, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var list = List(gameId, actorId, key, evaluationDataType, start, end);
			if (list.Count > 0)
			{
				value = list.OrderBy(s => s.DateCreated).First();
				return true;
			}
			value = null;
			return false;
		}

		public int CountKeys(int gameId, int actorId, string key, EvaluationDataType evaluationDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return List(gameId, actorId, key, evaluationDataType, start, end).Count;
		}

		public EvaluationData Create(EvaluationData data)
		{
			using (var context = ContextFactory.Create())
			{
				context.HandleDetatchedGame(data.GameId);
				context.HandleDetatchedActor(data.ActorId);

				context.EvaluationData.Add(data);
				context.SaveChanges();

				return data;
			}
		}

		public void Create(List<EvaluationData> datas)
		{
			var batchSize = 10000;
			var offset = 0;
			while (offset < datas.Count)
			{
				var batch = datas.Skip(offset).Take(batchSize);
				offset += batchSize;

				using (var context = ContextFactory.Create())
				{
					context.ChangeTracker.AutoDetectChangesEnabled = false;
					context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
					context.EvaluationData.AddRange(batch);
					context.SaveChanges(false);
				}
			}
		}

		public EvaluationData Update(EvaluationData updatedData)
		{
			using (var context = ContextFactory.Create())
			{
				context.EvaluationData.Update(updatedData);
				context.SaveChanges();
				return updatedData;
			}
		}
	}
}