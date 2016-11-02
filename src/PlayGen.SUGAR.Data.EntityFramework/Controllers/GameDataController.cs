using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GameDataController : DbController, IGameDataController
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
					.FilterByDataType(GameDataType.Long)
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
					.FilterByDataType(GameDataType.Float)
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
					.FilterByDataType(GameDataType.String)
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
					.FilterByDataType(GameDataType.Boolean)
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
					.FilterByDataType(GameDataType.Float)
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
					.FilterByDataType(GameDataType.Long)
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
					.FilterByDataType(GameDataType.Float)
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
					.FilterByDataType(GameDataType.Long)
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
					.FilterByDataType(GameDataType.Float)
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
					.FilterByDataType(GameDataType.Long)
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
					.FilterByDataType(GameDataType.Long)
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
					.FilterByDataType(GameDataType.Float)
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
					.FilterByDataType(GameDataType.Boolean)
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
					.FilterByDataType(GameDataType.String)
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

		public int CountKeys(int? gameId, int? actorId, string key, GameDataType gameDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(gameDataType)
					.FilterByDateTimeRange(start, end)
					.Count();

				return data;
			}
		}

		public DateTime TryGetEarliestKey(int? gameId, int? actorId, string key, GameDataType gameDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			DateTime dataDateTime;
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(gameDataType)
					.FilterByDateTimeRange(start, end)
					.FirstOrDefault();

				if (data == null)
				{
					dataDateTime = default(DateTime);
				}
				else
				{
					dataDateTime = data.DateCreated;
				}
				
				return dataDateTime;
			}
		}

		public DateTime TryGetLatestKey(int? gameId, int? actorId, string key, GameDataType gameDataType, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			DateTime dataDateTime;
			using (var context = ContextFactory.Create())
			{
				var data = context.GetCategoryData(_category)
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(gameDataType)
					.FilterByDateTimeRange(start, end)
					.LatestOrDefault();

				if (data == null)
				{
					dataDateTime = default(DateTime);
				}
				else
				{
					dataDateTime = data.DateModified;
				}

				return dataDateTime;
			}
		}

		public void Create(GameData data)
		{
			using (var context = ContextFactory.Create())
			{
				context.HandleDetatchedGame(data.GameId);
				context.HandleDetatchedActor(data.ActorId);

				if (ParseCheck(data))
				{
					context.GameData.Add(data);
					SaveChanges(context);
				}
				else
				{
					throw new ArgumentException($"Invalid Value {data.Value} for GameDataType {data.DataType}");
				}
			}
		}

		public void Create(GameData[] data)
		{
			List<GameData> dataList = new List<GameData>();
			foreach (var d in data)
			{
				if (ParseCheck(d))
				{
					dataList.Add(d);
				}
				else
				{
					throw new ArgumentException($"Invalid Value {d.Value} for GameDataType {d.DataType}");
				}
				if (dataList.Count >= 1000)
				{
					using (var context = ContextFactory.Create())
					{
						context.GameData.AddRange(dataList);
						SaveChanges(context);
						dataList.Clear();
					}
				}
			}
			if (dataList.Count > 0)
			{
				using (var context = ContextFactory.Create())
				{
					context.GameData.AddRange(dataList);
					SaveChanges(context);
					dataList.Clear();
				}
			}
		}

		public void Update(GameData updatedData)
		{
			using (var context = ContextFactory.Create())
			{
				var existingData = context.GameData.Find(updatedData.Id);
				if (existingData == null)
				{
					throw new MissingRecordException("Cannot find the object to update.");
				}

				existingData.Value = updatedData.Value;

				SaveChanges(context);
			}
		}

		protected bool ParseCheck(GameData data)
		{
			switch (data.DataType) {
				case GameDataType.String:
					return true;
				case GameDataType.Long:
					long tryLong;
					if (long.TryParse(data.Value, out tryLong))
					{
						return true;
					}
					else
					{
						return false;
					}
				case GameDataType.Float:
					float tryFloat;
					if (float.TryParse(data.Value, out tryFloat))
					{
						return true;
					}
					else
					{
						return false;
					}
				case GameDataType.Boolean:
					bool tryBoolean;
					if (bool.TryParse(data.Value, out tryBoolean))
					{
						return true;
					}
					else
					{
						return false;
					}
				default:
					return false;
			}
		}

		protected DateTime EndSet (DateTime end)
		{
			if (end == default(DateTime))
			{
				return DateTime.Now;
			} else
			{
				return end;
			}
		}
	}
}