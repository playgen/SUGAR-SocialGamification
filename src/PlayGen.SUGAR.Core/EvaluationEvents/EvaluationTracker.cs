using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Evaluation tracking system.
    /// </summary>
    public class EvaluationTracker
    {
        private readonly ProgressCache _progressCache;
        private readonly ProgressNotificationCache _progressNotificationCache;
        private readonly EvaluationGameDataMapper _gameDataToEvaluationMapper;

        public EvaluationTracker(EvaluationGameDataMapper gameDataToEvaluationMapper,
            ProgressCache progressCache,
            ProgressNotificationCache progressNotificationCache)
        {
            _progressCache = progressCache;
            _gameDataToEvaluationMapper = gameDataToEvaluationMapper;
            _progressNotificationCache = progressNotificationCache;
        
            MapExistingEvaluations();
        }

        public void OnActorSessionStarted(int gameId, int actorId)
        {
            var progress = _progressCache.StartTracking(gameId, actorId);
            _progressNotificationCache.Check(progress);
        }

        public void OnActorSessionEnded(int gameId, int actorId)
        {
            _progressCache.StopTracking(gameId, actorId);
            _progressNotificationCache.Remove(gameId, actorId);
        }

        public void OnGameDataAdded(GameData gameData)
        {
            var evaluations = _gameDataToEvaluationMapper.GetRelated(gameData);
            var progress = _progressCache.Evaluate(evaluations, gameData);
            _progressNotificationCache.Check(progress);
        }

        public void OnEvaluationAdded(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.CreateMappings(evaluation);
            var progress = _progressCache.Evaluate(evaluation);
            _progressNotificationCache.Check(progress);
        }

        public void OnEvaluationUpdated(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMappings(evaluation);
            _gameDataToEvaluationMapper.CreateMappings(evaluation);

            var progress = _progressCache.Evaluate(evaluation);
            _progressNotificationCache.Check(progress);
        }

        public void OnEvaluationDeleted(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMappings(evaluation);

            _progressCache.Remove(evaluation.Id);
            _progressNotificationCache.Remove(evaluation.Id);
        }

        private void MapExistingEvaluations()
        {
            var evaluations = new List<Evaluation>();
            _gameDataToEvaluationMapper.MapExisting(evaluations);
        }
    }
}
