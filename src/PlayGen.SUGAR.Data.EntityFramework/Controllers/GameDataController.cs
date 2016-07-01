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

		public bool KeyExists(int? gameId, int? actorId, string key)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				return context.GetData()
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
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
		
		public float SumFloats(int? gameId, int? actorId, string key)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(GameDataType.Float)
					.ToList();

				var sum = data.Sum(s => float.Parse(s.Value));
				return sum;
			}
		}

		public long SumLongs(int? gameId, int? actorId, string key)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(GameDataType.Long)
					.ToList();

				var sum = data.Sum(s => long.Parse(s.Value));
				return sum;
			}
		}
		
		public bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latestBool)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(GameDataType.Boolean)
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

		// TODO expose via WebAPI
		public bool TryGetLatestString(int? gameId, int? actorId, string key, out string latestString)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(GameDataType.String)
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

		public int CountKeys(int? gameId, int? actorId, string key, GameDataType gameDataType)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetData()
					.FilterByGameId(gameId)
					.FilterByActorId(actorId)
					.FilterByKey(key)
					.FilterByDataType(gameDataType)
					.Count();

				return data;
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
	}
}