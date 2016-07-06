using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
    public class ResourceController
    {
	    private GameDataController _gameDataController;

		public ResourceController(string nameOrConnectionString)
		{
			_gameDataController = new GameDataController(nameOrConnectionString, GameDataCategory.Resource);
		}

		public bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
		{
			return _gameDataController.KeyExists(gameId, actorId, key, start = default(DateTime), end = default(DateTime));
		}

		public IEnumerable<GameData> Get(int? gameId, int? actorId, IEnumerable<string> keys)
		{
			return _gameDataController.Get(gameId, actorId, keys);
		}

		public void Create(GameData data)
		{
			if (data.Category != GameDataCategory.Resource)
			{
				// TODO error
			}
			else if (data.DataType != GameDataType.Long)
			{
				// TODO error
			}

			_gameDataController.Create(data);
		}
	}
}
