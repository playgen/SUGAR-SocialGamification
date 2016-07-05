using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GameDataController : DbController, IGameDataController
	{
		public GameDataController(string nameOrConnectionString) 
			: base(nameOrConnectionString)
		{
		}

		public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				return context.GetData()
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDateTimeRange(start, end)
					.Any();
			}
		}

		public IEnumerable<GameData> Get(int? actorId, int? gameId, IEnumerable<string> keys)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKeys(keys)
					.ToList();
				return data;
			}
		}
		
		public float SumFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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

		public bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latestBool, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			end = EndSet(end);
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				context.GameData.Add(data);
				SaveChanges(context);
			}
		}

		public void Create(GameData[] data)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				foreach (var d in data)
				{
					context.GameData.Add(d);
				}
				SaveChanges(context);
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