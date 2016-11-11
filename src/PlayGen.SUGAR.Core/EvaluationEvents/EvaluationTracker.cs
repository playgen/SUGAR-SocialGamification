using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    public class EvaluationTracker
    {
        private readonly EvaluationProgressTracker _progressTracker;
        private readonly EvaluationGameDataMapper _gameDataToEvaluationMapper;

        public EvaluationTracker(EvaluationProgressTracker progressTracker,
            EvaluationGameDataMapper gameDataToEvaluationMapper)
        {
            _progressTracker = progressTracker;
            _gameDataToEvaluationMapper = gameDataToEvaluationMapper;
        }

        public EvaluationTracker()
        {
            MapExistingEvaluations();
        }

        public void OnActorSessionStarted(int gameId, int actorId)
        {
            _progressTracker.StartTracking(gameId, actorId);
        }

        public void OnActorSessionEnded(int gameId, int actorId)
        {
            _progressTracker.StopTracking(gameId, actorId);
        }

        public void OnGameDataAdded(GameData gameData)
        {
            var evaluations = _gameDataToEvaluationMapper.GetRelated(gameData);
            _progressTracker.Evaluate(evaluations, gameData);
        }

        public void OnEvaluationAdded(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.CreateMappings(evaluation);
            _progressTracker.Evaluate(evaluation);
        }

        public void OnEvaluationUpdated(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMappings(evaluation);
            _gameDataToEvaluationMapper.CreateMappings(evaluation);

            _progressTracker.Evaluate(evaluation);
        }

        public void OnEvaluationDeleted(Evaluation evaluation)
        {
            _gameDataToEvaluationMapper.RemoveMappings(evaluation);

            _progressTracker.Remove(evaluation);
        }

        private void MapExistingEvaluations()
        {
            var evaluations = new List<Evaluation>(); // todo create mappings for all existing evaluations
            _gameDataToEvaluationMapper.MapExisting(evaluations);
        }
    }
}
