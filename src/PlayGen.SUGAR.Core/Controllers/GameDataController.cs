using System;
using PlayGen.SUGAR.Data.Model;
using NLog;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.EntityFramework;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class GameDataController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly EvaluationDataController _evaluationDataController;

        public GameDataController(SUGARContextFactory contextFactory)
        {
            _evaluationDataController = new EvaluationDataController(contextFactory, EvaluationDataCategory.GameData);
        }

        public List<EvaluationData> Get(int? gameId, int? actorId, string[] key)
        {
            return _evaluationDataController.Get(gameId, actorId, key);
        }

        public EvaluationData Add(EvaluationData newData)
        {
            return _evaluationDataController.Add(newData);
        }

        public void Add(EvaluationData[] evaluationData)
        {
            _evaluationDataController.Add(evaluationData);
        }

        public EvaluationData GetEvaluationDataByHighestFloat(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            return _evaluationDataController.GetEvaluationDataByHighestFloat(gameId, actorId, key, start, end);
        }

        public EvaluationData GetEvaluationDataByHighestLong(int? gameId, int? actorId, string key,
            DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            return _evaluationDataController.GetEvaluationDataByHighestLong(gameId, actorId, key, start, end);
        }
    }
}