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

		public bool KeyExists(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
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

		private List<EvaluationData> GetContextEvaluationData(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			using (var context = ContextFactory.Create())
			{
				return context.EvaluationData
					.FilterBy(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end);
			}
		}

		public List<long> AllLongs(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Select(s => long.Parse(s.Value)).ToList();
		}

		public List<float> AllFloats(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Select(s => float.Parse(s.Value)).ToList();
		}

		public List<string> AllStrings(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Select(s => s.Value).ToList();
			
		}

		public List<bool> AllBools(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Select(s => bool.Parse(s.Value)).ToList();
		}

		public float SumFloats(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Sum(s => float.Parse(s.Value));
		}

		public long SumLongs(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Sum(s => long.Parse(s.Value));
		}

		public float GetHighestFloat(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var data = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end);
			return data.Count > 0 ? data.Max(s => float.Parse(s.Value)) : default(float);
		}

		public EvaluationData GetEvaluationDataByHighestFloat(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.First();
		}

		public long GetHighestLong(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var data = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end);
			return data.Count > 0 ? data.Max(s => long.Parse(s.Value)) : default(int);
		}

		public EvaluationData GetEvaluationDataByHighestLong(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.First();
		}

		public float GetLowestFloat(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Min(s => float.Parse(s.Value));
		}

		public long GetLowestLong(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Min(s => long.Parse(s.Value));
		}

		public bool TryGetLatestLong(int? gameId, int? actorId, string key, out long latestLong, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var data = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.FirstOrDefault();
			latestLong = data != null ? long.Parse(data.Value) : default(long);
			return data != null;
		}

		public bool TryGetLatestFloat(int? gameId, int? actorId, string key, out float latestFloat, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var data = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.FirstOrDefault();
			latestFloat = data != null ? float.Parse(data.Value) : default(float);
			return data != null;
		}

		public bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latestBool, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var data = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.FirstOrDefault();
			latestBool = data != null ? bool.Parse(data.Value) : default(bool);
			return data != null;
		}

		public bool TryGetLatestString(int? gameId, int? actorId, string key, out string latestString, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var data = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.FirstOrDefault();
			latestString = data?.Value;
			return data != null;
		}

		public int CountKeys(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.Count;
		}

		// todo change to bool TryGet[name](out value) pattern
		public DateTime TryGetEarliestKey(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			var data = GetContextEvaluationData(gameId, actorId, key, evaluationDataType, evaluationDataCategory, start, end)
				.FirstOrDefault();
			return data?.DateCreated ?? default(DateTime);
		}

		public DateTime TryGetLatestKey(int? gameId, int? actorId, string key, EvaluationDataType? evaluationDataType, EvaluationDataCategory? evaluationDataCategory, DateTime start = default(DateTime), DateTime end = default(DateTime))
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
					.Find(context, updatedData.Id);

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