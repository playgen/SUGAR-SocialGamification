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
        private readonly EvaluationGameDataMapper _gameDataToEvaluationMapper;

        public EvaluationTracker(ProgressCache progressCache,
            EvaluationGameDataMapper gameDataToEvaluationMapper)
        {
            _progressCache = progressCache;
            _gameDataToEvaluationMapper = gameDataToEvaluationMapper;
        }

        public EvaluationTracker()
        {
            MapExistingEvaluations();
        }

        public void OnActorSessionStarted(int gameId, int actorId)
        {
            _progressCache.StartTracking(gameId, actorId);
        }

        public void OnActorSessionEnded(int gameId, int actorId)
        {
            _progressCache.StopTracking(gameId, actorId);
        }

        public void OnGameDataAdded(GameData gameData)
        {
            var evaluations = _gameDataToEvaluationMapper.GetRelated(gameData);
            _progressCache.Evaluate(evaluations, gameData);
        }

        public void OnEvaluationAdded(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.CreateMappings(evaluation);
            _progressCache.Evaluate(evaluation);
        }

        public void OnEvaluationUpdated(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMappings(evaluation);
            _gameDataToEvaluationMapper.CreateMappings(evaluation);

            _progressCache.Evaluate(evaluation);
        }

        public void OnEvaluationDeleted(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMappings(evaluation);

            _progressCache.Remove(evaluation);
        }

        private void MapExistingEvaluations()
        {
            var evaluations = new List<Evaluation>();
            _gameDataToEvaluationMapper.MapExisting(evaluations);
        }
    }
}
