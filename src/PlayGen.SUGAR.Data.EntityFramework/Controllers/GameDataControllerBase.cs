using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public abstract class GameDataControllerBase<TEntity> : DbController, IGameDataController
		where TEntity : GameData
	{
		protected GameDataControllerBase(string nameOrConnectionString) 
			: base(nameOrConnectionString)
		{
		}
		
		public IEnumerable<TEntity> Get(int gameId, int actorId, IEnumerable<string> keys)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetGameData<TEntity>()
					.Where(d => d.ActorId == actorId 
					&& d.GameId == gameId 
					&& keys.Contains(d.Key)).ToList();
				return data.Cast<TEntity>();
			}
		}

		public float SumFloats(int gameId, int actorId, string key)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var datas = context.GetGameData<TEntity>()
							.Where(s => s.GameId == gameId
										&& s.ActorId == actorId
										&& s.Key == key
										&& s.DataType == GameDataValueType.Float)
							.ToList();

				var sum = datas.Sum(s => float.Parse(s.Value));
				return sum;
			}
		}

		public long SumLongs(int gameId, int actorId, string key)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var datas = context.GetGameData<TEntity>()
					.Where(s => s.GameId == gameId
							&& s.ActorId == actorId
							&& s.Key == key
							&& s.DataType == GameDataValueType.Long).ToList();

				long sum = datas.Sum(s => long.Parse(s.Value));
				return sum;
			}
		}

		public bool TryGetLatestBool(int gameId, int actorId, string key, out bool latestBool)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetGameData<TEntity>()
					.Where(s => s.GameId == gameId
							&& s.ActorId == actorId
							&& s.Key == key
							&& s.DataType == GameDataValueType.Boolean)
					.OrderByDescending(s => s.DateModified)
					.FirstOrDefault();

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
		public bool TryGetLatestString(int gameId, int actorId, string key, out string latestString)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var data = context.GetGameData<TEntity>()
					.Where(s => s.GameId == gameId
							&& s.ActorId == actorId
							&& s.Key == key
							&& s.DataType == GameDataValueType.String)
					.OrderByDescending(s => s.DateModified)
					.FirstOrDefault();

				if (data == null)
				{
					latestString = default(string);
					return false;
				}

				latestString = data.Value;
				return true;
			}
		}
	}
}