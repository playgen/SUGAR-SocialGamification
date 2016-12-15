using System;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;
using NLog;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class GameDetailsController
	{
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Data.EntityFramework.Controllers.GameDetailsController _GameDetailsDbController;

		public GameDetailsController(Data.EntityFramework.Controllers.GameDetailsController GameDetailsDbController)
		{
			_GameDetailsDbController = GameDetailsDbController;
		}

		public List<GameDetails> Get(int? gameId = null, int? actorId = null, ICollection<string> keys = null)
		{
			var datas = _GameDetailsDbController.Get(gameId, actorId, keys);

			Logger.Info($"{datas?.Count} Actor Datas for GameId: {gameId}, ActorId: {actorId}, Keys: {string.Join(", ", keys)}");

			return datas;
		}

		public bool KeyExists(int? gameId, int? actorId, string key)
		{
			var keyExists = _GameDetailsDbController.KeyExists(gameId, actorId, key);

            Logger.Info($"Key Exists: {keyExists} for GameId: {gameId}, ActorId: {actorId}, Key: {key}");

            return keyExists;
		}

		public GameDetails Add(GameDetails newDetails)
		{
			if (ParseCheck(newDetails))
			{
				newDetails = _GameDetailsDbController.Create(newDetails);

                Logger.Info($"{newDetails?.Id}");

                return newDetails;
			}
			else
			{
				throw new ArgumentException($"Invalid Value {newDetails.Value} for SaveDataType {newDetails.SaveDataType}");
			}
		}

		public void Update(GameDetails newDetails)
		{
			_GameDetailsDbController.Update(newDetails);

            Logger.Info($"{newDetails?.Id}");
        }

		protected bool ParseCheck(GameDetails details)
		{
			switch (details.SaveDataType)
			{
				case SaveDataType.String:
					return true;

				case SaveDataType.Long:
					long tryLong;
					return long.TryParse(details.Value, out tryLong);

				case SaveDataType.Float:
					float tryFloat;
					return float.TryParse(details.Value, out tryFloat);

				case SaveDataType.Boolean:
					bool tryBoolean;
					return bool.TryParse(details.Value, out tryBoolean);

				default:
					return false;
			}
		}
	}
}
