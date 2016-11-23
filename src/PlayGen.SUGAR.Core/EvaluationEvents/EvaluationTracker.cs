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
        private readonly ProgressEvaluator _progressEvaluator;
        private readonly ProgressNotificationCache _progressNotificationCache;
        private readonly EvaluationGameDataMapper _gameDataToEvaluationMapper;

        public EvaluationTracker(EvaluationGameDataMapper gameDataToEvaluationMapper,
            ProgressEvaluator progressEvaluator,
            ProgressNotificationCache progressNotificationCache)
        {
            _progressEvaluator = progressEvaluator;
            _gameDataToEvaluationMapper = gameDataToEvaluationMapper;
            _progressNotificationCache = progressNotificationCache;
        
            MapExistingEvaluations();
        }

        public void OnActorSessionStarted(int? gameId, int actorId)
        {
            var progress = _progressEvaluator.StartTracking(gameId, actorId);
            _progressNotificationCache.Check(progress);
        }

        public void OnActorSessionEnded(int? gameId, int actorId)
        {
            _progressEvaluator.StopTracking(gameId, actorId);
            _progressNotificationCache.Remove(gameId, actorId);
        }

        public void OnGameDataAdded(GameData gameData)
        {
            HashSet<Evaluation> evaluations;

            if (_gameDataToEvaluationMapper.TryGetRelated(gameData, out evaluations))
            {
                var progress = _progressEvaluator.Evaluate(evaluations, gameData);
                _progressNotificationCache.Check(progress);
            }
        }

        public Dictionary<int, Queue<KeyValuePair<Evaluation, float>>> GetPendingNotifications(int? gameId, int actorId)
        {
            return _progressNotificationCache.GetNotifications(gameId, actorId);
        }

        public void OnEvaluationAdded(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.CreateMapping(evaluation);
            var progress = _progressEvaluator.Evaluate(evaluation);
            _progressNotificationCache.Check(progress);
        }

        public void OnEvaluationUpdated(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMapping(evaluation);
            _gameDataToEvaluationMapper.CreateMapping(evaluation);

            var progress = _progressEvaluator.Evaluate(evaluation);
            _progressNotificationCache.Check(progress);
        }

        public void OnEvaluationDeleted(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMapping(evaluation);

            _progressEvaluator.Remove(evaluation);
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
