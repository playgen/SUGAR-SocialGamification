using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GameDataController : DbController
	{
		private readonly GameDataCategory _category = GameDataCategory.GameData;

		public GameDataController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        public GameDataController(SUGARContextFactory contextFactory, GameDataCategory category) 
			: base(contextFactory)
		{
			_category = category;
		}

		public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				return context.GetCategoryData(_category)
                    .FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDateTimeRange(start, end)
					.Any();
			}
		}

		public IEnumerable<GameData> Get(IEnumerable<int> ids)
		{
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByIds(ids)
					.ToList();
				return data;
			}
		}

		public IEnumerable<GameData> Get(int? gameId = null, int? actorId = null, IEnumerable<string> keys = null)
		{
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKeys(keys)
					.ToList();
				return data;
			}
		}

		public IEnumerable<long> AllLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Long)
					.FilterByDateTimeRange(start, end)
					.ToList();

				var list = data.Select(s => long.Parse(s.Value));
				return list;
			}
		}

		public IEnumerable<float> AllFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Float)
					.FilterByDateTimeRange(start, end)
					.ToList();

				var list = data.Select(s => float.Parse(s.Value));
				return list;
			}
		}

		public IEnumerable<string> AllStrings(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.String)
					.FilterByDateTimeRange(start, end)
					.ToList();

				var list = data.Select(s => s.Value);
				return list;
			}
		}

		public IEnumerable<bool> AllBools(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Boolean)
					.FilterByDateTimeRange(start, end)
					.ToList();

				var list = data.Select(s => bool.Parse(s.Value));
				return list;
			}
		}

		public float SumFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Float)
					.FilterByDateTimeRange(start, end)
					.ToList();

				var sum = data.Sum(s => float.Parse(s.Value));
				return sum;
			}
		}

		public long SumLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Long)
					.FilterByDateTimeRange(start, end)
					.ToList();

				var sum = data.Sum(s => long.Parse(s.Value));
				return sum;
			}
		}

		public float GetHighestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Float)
					.FilterByDateTimeRange(start, end)
					.ToList();

				if (data.Count == 0)
				{
					return 0;
				}

				var sum = data.Max(s => float.Parse(s.Value));
				return sum;
			}
		}

		public long GetHighestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Long)
					.FilterByDateTimeRange(start, end)
					.ToList();

				if (data.Count == 0)
				{
					return 0;
				}

				var sum = data.Max(s => long.Parse(s.Value));
				return sum;
			}
		}

		public float GetLowestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Float)
					.FilterByDateTimeRange(start, end)
					.ToList();

				if (data.Count == 0)
				{
					return 0;
				}

				var sum = data.Min(s => float.Parse(s.Value));
				return sum;
			}
		}

		public long GetLowestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Long)
					.FilterByDateTimeRange(start, end)
					.ToList();

				if (data.Count == 0)
				{
					return 0;
				}

				var sum = data.Min(s => long.Parse(s.Value));
				return sum;
			}
		}

		public bool TryGetLatestLong(int? gameId, int? actorId, string key, out long latestLong, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Long)
					.FilterByDateTimeRange(start, end)
					.LatestOrDefault();

				if (data == null)
				{
					latestLong = default(long);
					return false;
				}

				latestLong = long.Parse(data.Value);
				return true;
			}
		}

		public bool TryGetLatestFloat(int? gameId, int? actorId, string key, out float latestFloat, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Float)
					.FilterByDateTimeRange(start, end)
					.LatestOrDefault();

				if (data == null)
				{
					latestFloat = default(float);
					return false;
				}

				latestFloat = float.Parse(data.Value);
				return true;
			}
		}

		public bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latestBool, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.Boolean)
					.FilterByDateTimeRange(start, end)
					.LatestOrDefault();
				
				if (data == null)
				{
					latestBool = default(bool);
					return false;
				}

				latestBool = bool.Parse(data.Value);
				return true;
			}
		}

		public bool TryGetLatestString(int? gameId, int? actorId, string key, out string latestString, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType.String)
					.FilterByDateTimeRange(start, end)
					.LatestOrDefault();

				if (data == null)
				{
					latestString = default(string);
					return false;
				}

				latestString = data.Value;
				return true;
			}
		}

		public int CountKeys(int? gameId, int? actorId, string key, SaveDataType SaveDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType)
					.FilterByDateTimeRange(start, end)
					.Count();

				return data;
			}
		}

        // todo change to bool TryGet[name](out value) pattern
		public DateTime TryGetEarliestKey(int? gameId, int? actorId, string key, SaveDataType SaveDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
		    using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType)
					.FilterByDateTimeRange(start, end)
					.FirstOrDefault();

				var dataDateTime = data?.DateCreated ?? default(DateTime);
				
				return dataDateTime;
			}
		}

		public DateTime TryGetLatestKey(int? gameId, int? actorId, string key, SaveDataType SaveDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
		    using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(SaveDataType)
					.FilterByDateTimeRange(start, end)
					.LatestOrDefault();

				var dataDateTime = data?.DateModified ?? default(DateTime);

				return dataDateTime;
			}
		}

		public GameData Create(GameData data)
		{
		    using (var context = ContextFactory.Create())
		    {
		        context.HandleDetatchedGame(data.GameId);
		        context.HandleDetatchedActor(data.ActorId);

		        context.GameData.Add(data);
		        SaveChanges(context);

		        return data;
		    }
		}

		public void Create(IEnumerable<GameData> datas)
		{
			using (var context = ContextFactory.Create())
			{
				context.GameData.AddRange(datas);
				SaveChanges(context);
			}
		}

		public void Update(GameData updatedData)
		{
			using (var context = ContextFactory.Create())
			{
                // todo replace with entire block with: (and update unit tests)
                // context.[tablename].Update(entity);
                // context.SaveChanges();

                var existingData = context.GameData
                    .Find(context, updatedData.Id);

				if (existingData == null)
				{
					throw new MissingRecordException("Cannot find the object to update.");
				}

				existingData.Value = updatedData.Value;

				SaveChanges(context);
			}
		}

        protected DateTime EndSet(DateTime end)
        {
            return end == default(DateTime) ? DateTime.Now : end;
        }
    }
}