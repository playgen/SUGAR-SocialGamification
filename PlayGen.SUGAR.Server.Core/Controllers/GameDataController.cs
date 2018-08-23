using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class GameDataController
	{
		private readonly ILogger _logger;
		private readonly EvaluationDataController _evaluationDataController;
		private readonly ActorController _actorController;

		public GameDataController(
			ILogger<GameDataController> logger,
			ILogger<EvaluationDataController> evaluationDataLogger,
			SUGARContextFactory contextFactory, 
			ActorController actorController)
		{
			_logger = logger;
			_evaluationDataController = new EvaluationDataController(evaluationDataLogger, contextFactory, EvaluationDataCategory.GameData);
			_actorController = actorController;
		}

		public List<EvaluationData> Get(int gameId, int actorId, params string[] keys)
		{
			return _evaluationDataController.Get(gameId, actorId, keys);
		}

		public EvaluationData Get(int gameId, int actorId, string key, EvaluationDataType dataType, LeaderboardType sortType)
		{
			return _evaluationDataController.Get(gameId, actorId, key, dataType, sortType);
		}

		public List<Actor> GetGameActors(int gameId)
		{
			var ids = _evaluationDataController.GetGameActors(gameId);
			return ids
				.Where(id => id != null)
				.Select(id => _actorController.Get(id.Value)).ToList();
		}

		public List<KeyValuePair<string, EvaluationDataType>> GetGameKeys(int gameId)
		{
			return _evaluationDataController.GetGameKeys(gameId);
		}

		public List<EvaluationData> GetActorData(int actorId)
		{
			return _evaluationDataController.GetActorData(actorId);
		}

		public EvaluationData Add(EvaluationData newData)
		{
			return _evaluationDataController.Add(newData);
		}

		public void Add(List<EvaluationData> evaluationData)
		{
			_evaluationDataController.Add(evaluationData);
		}
	}
}