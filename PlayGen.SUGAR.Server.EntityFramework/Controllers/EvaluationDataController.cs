using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
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

		public bool KeyExists(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterBy(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
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

		public List<EvaluationData> Get(int? gameId = null, int? actorId = null, ICollection<string> keys = null)
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

		public List<EvaluationData> Get(int? gameId = null, int? entityId = null, int? actorId = null, ICollection<string> keys = null)
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

		public List<int?> GetGameActors(int? gameId = null)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.ToList()
					.Select(d => d.ActorId)
					.Distinct()
					.ToList();
			}
		}

		public List<KeyValuePair<string, EvaluationDataType>> GetGameKeys(int? gameId = null)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.ToList()
					.Select(d => new KeyValuePair<string, EvaluationDataType>(d.Key, d.EvaluationDataType))
					.Distinct()
					.ToList();
			}
		}

		public List<EvaluationData> GetActorData(int? actorId = null)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
					.FilterByActorId(actorId)
					.ToList();
			}
		}

		private List<EvaluationData> GetContextEvaluationData(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			using (var context = ContextFactory.Create())
			{
				return context.EvaluationData
					.FilterBy(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end);
			}
		}

		public List<T> All<T>(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType,
			EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Select(s => (T)Convert.ChangeType(s.Value, typeof(T))).ToList();
		}

		public T Sum<T>(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType,
			EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var total = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end).ToList();
			var sum = total.Sum(s => Convert.ToDouble(s.Value));
			return (T) Convert.ChangeType(sum, typeof(T));
		}

		public T Max<T>(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType,
			EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var max = All<T>(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end).Max(s => (dynamic)s);
			return max != null ? (T)Convert.ChangeType(max, typeof(T)) : default (T);
		}

		public T Min<T>(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType,
			EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var min = All<T>(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end).Min(s => (dynamic)s);
			return min != null ? (T)Convert.ChangeType(min, typeof(T)) : default(T);
		}

		public bool TryGetLatest<T>(int? gameId, int? actorId, string key, out T latestLong, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var latest = All<T>(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end).FirstOrDefault();
			latestLong = latest != null ? latest : default(T);
			return latest != null;
		}

		public EvaluationData GetEvaluationDataByHighestFloat(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.First();
		}

		public EvaluationData GetEvaluationDataByHighestLong(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.First();
		}

		public int CountKeys(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Count;
		}

		// todo change to bool TryGet[name](out value) pattern
		public DateTime TryGetEarliestKey(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var data = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.FirstOrDefault();
			return data?.DateCreated ?? default(DateTime);
		}

		public DateTime TryGetLatestKey(int? gameId, int? actorId, string key, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var data = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.FirstOrDefault();
			return data?.DateModified ?? default(DateTime);
		}

		public EvaluationData Create(EvaluationData data)
		{
			using (var context = ContextFactory.Create())
			{
				context.HandleDetatchedGame(data.GameId);
				context.HandleDetatchedActor(data.ActorId);

				context.EvaluationData.Add(data);
				SaveChanges(context);

				return data;
			}
		}

		public void Create(List<EvaluationData> datas)
		{
			using (var context = ContextFactory.Create())
			{
				context.EvaluationData.AddRange(datas);
				SaveChanges(context);
			}
		}

		public EvaluationData Update(EvaluationData updatedData)
		{
			using (var context = ContextFactory.Create())
			{
				// todo replace with entire block with: (and update unit tests)
				// context.[tablename].Update(entity);
				// context.SaveChanges();

				var existingData = context.EvaluationData
					.Find(updatedData.Id);

				if (existingData == null)
				{
					throw new MissingRecordException("Cannot find the object to update.");
				}

				existingData.Value = updatedData.Value;

				SaveChanges(context);

				return existingData;
			}
		}

		protected DateTime EndSet(DateTime end)
		{
			return end == default(DateTime) ? DateTime.Now : end;
		}
	}
}