using System;
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
            HashSet<Evaluation> evaluations;

            if (_gameDataToEvaluationMapper.TryGetRelated(gameData, out evaluations))
            {
                var progress = _progressCache.Evaluate(evaluations, gameData);
                _progressNotificationCache.Check(progress);
            }
        }

        public Dictionary<int, Dictionary<Evaluation, float>> GetPendingNotifications(int gameId, int actorId)
        {
            return _progressNotificationCache.GetNotifications(gameId, actorId);
        }

        public void OnEvaluationAdded(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.CreateMapping(evaluation);
            var progress = _progressCache.Evaluate(evaluation);
            _progressNotificationCache.Check(progress);
        }

        public void OnEvaluationUpdated(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMapping(evaluation);
            _gameDataToEvaluationMapper.CreateMapping(evaluation);

            var progress = _progressCache.Evaluate(evaluation);
            _progressNotificationCache.Check(progress);
        }

        public void OnEvaluationDeleted(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMapping(evaluation);

            _progressCache.Remove(evaluation);
            _progressNotificationCache.Remove(evaluation);
        }

        private void MapExistingEvaluations()
        {
            // todo read from database  - and write tests
            var evaluations = new List<Evaluation>();
            _gameDataToEvaluationMapper.CreateMappings(evaluations);
        }
    }
}
