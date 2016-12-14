using System;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;
using NLog;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class ActorDataController
	{
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Data.EntityFramework.Controllers.ActorDataController _actorDataDbController;

		public ActorDataController(Data.EntityFramework.Controllers.ActorDataController actorDataDbController)
		{
			_actorDataDbController = actorDataDbController;
		}

		public List<ActorData> Get(int? gameId = null, int? actorId = null, ICollection<string> keys = null)
		{
			var datas = _actorDataDbController.Get(gameId, actorId, keys);

			Logger.Info($"{datas?.Count} Actor Datas for GameId: {gameId}, ActorId: {actorId}, Keys: {string.Join(", ", keys)}");

			return datas;
		}

		public bool KeyExists(int? gameId, int? actorId, string key)
		{
			var keyExists = _actorDataDbController.KeyExists(gameId, actorId, key);

            Logger.Info($"Key Exists: {keyExists} for GameId: {gameId}, ActorId: {actorId}, Key: {key}");

            return keyExists;
		}

		public ActorData Add(ActorData newData)
		{
			if (ParseCheck(newData))
			{
				newData = _actorDataDbController.Create(newData);

                Logger.Info($"{newData?.Id}");

                return newData;
			}
			else
			{
				throw new ArgumentException($"Invalid Value {newData.Value} for EvaluationDataType {newData.EvaluationDataType}");
			}
		}

		public void Update(ActorData newData)
		{
			_actorDataDbController.Update(newData);

            Logger.Info($"{newData?.Id}");
        }

		protected bool ParseCheck(ActorData data)
		{
			switch (data.EvaluationDataType)
			{
				case EvaluationDataType.String:
					return true;

				case EvaluationDataType.Long:
					long tryLong;
					return long.TryParse(data.Value, out tryLong);

				case EvaluationDataType.Float:
					float tryFloat;
					return float.TryParse(data.Value, out tryFloat);

				case EvaluationDataType.Boolean:
					bool tryBoolean;
					return bool.TryParse(data.Value, out tryBoolean);

				default:
					return false;
			}
		}
	}
}
