using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class ActorDataController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.ActorDataController _actorDataDbController;

		public ActorDataController(
			ILogger<ActorDataController> logger,
			EntityFramework.Controllers.ActorDataController actorDataDbController)
		{
			_logger = logger;
			_actorDataDbController = actorDataDbController;
		}

		public List<ActorData> Get(int? gameId = null, int? actorId = null, ICollection<string> keys = null)
		{
			var datas = _actorDataDbController.Get(gameId, actorId, keys);

			_logger.LogInformation($"{datas?.Count} Actor Datas for GameId: {gameId}, ActorId: {actorId}, Keys: {string.Join(", ", keys)}");

			return datas;
		}

		public bool KeyExists(int? gameId, int? actorId, string key)
		{
			var keyExists = _actorDataDbController.KeyExists(gameId, actorId, key);

			_logger.LogInformation($"Key Exists: {keyExists} for GameId: {gameId}, ActorId: {actorId}, Key: {key}");

			return keyExists;
		}

		public ActorData Add(ActorData newData)
		{
			if (ParseCheck(newData))
			{
				newData = _actorDataDbController.Create(newData);

				_logger.LogInformation($"{newData?.Id}");

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

			_logger.LogInformation($"{newData?.Id}");
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
