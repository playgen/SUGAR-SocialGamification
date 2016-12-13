using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class SaveDataController : DbController
	{
		private readonly SaveDataCategory _category = SaveDataCategory.GameData;

        [Obsolete]
		public SaveDataController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        public SaveDataController(SUGARContextFactory contextFactory, SaveDataCategory category) 
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

		public List<SaveData> Get(List<int> ids)
		{
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByIds(ids)
					.ToList();
				return data;
			}
		}

		public List<SaveData> Get(int? gameId = null, int? actorId = null, ICollection<string> keys = null)
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

		public List<long> AllLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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

				var list = data.Select(s => long.Parse(s.Value)).ToList();
				return list;
			}
		}

		public List<float> AllFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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

				var list = data.Select(s => float.Parse(s.Value)).ToList();
				return list;
			}
		}

		public List<string> AllStrings(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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

				var list = data.Select(s => s.Value).ToList();
				return list;
			}
		}

		public List<bool> AllBools(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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

				var list = data.Select(s => bool.Parse(s.Value)).ToList();
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

		public float GetHighestFloat(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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

        public SaveData GetSaveDataByHighestFloat(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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
                    return null;
                }

                var max = data.OrderByDescending(gameData => float.Parse(gameData.Value)).First();
                return max;
            }
        }

        public long GetHighestLong(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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

        public SaveData GetSaveDataByHighestLong(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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
                    return null;
                }

                var max = data.OrderByDescending(gameData => float.Parse(gameData.Value)).First();
                return max;
            }
        }


        public float GetLowestFloat(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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

		public long GetLowestLong(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
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

		public SaveData Create(SaveData data)
		{
		    using (var context = ContextFactory.Create())
		    {
		        context.HandleDetatchedGame(data.GameId);
		        context.HandleDetatchedActor(data.ActorId);

		        context.SaveData.Add(data);
		        SaveChanges(context);

		        return data;
		    }
		}

		public void Create(List<SaveData> datas)
		{
			using (var context = ContextFactory.Create())
			{
				context.SaveData.AddRange(datas);
				SaveChanges(context);
			}
		}

		public void Update(SaveData updatedData)
		{
			using (var context = ContextFactory.Create())
			{
                // todo replace with entire block with: (and update unit tests)
                // context.[tablename].Update(entity);
                // context.SaveChanges();

                var existingData = context.SaveData
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