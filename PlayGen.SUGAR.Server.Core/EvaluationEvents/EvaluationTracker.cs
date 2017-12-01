// todo v2.0: support group evaluations
// todo store by sessionId instead of actor.Id?

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.Core.Sessions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.EvaluationEvents
{
    public class EvaluationTracker : IDisposable
    {
        private readonly EvaluationDataMapper _EvaluationDataToEvaluationMapper = new EvaluationDataMapper();
        private readonly ConcurrentProgressCache _concurrentProgressCache = new ConcurrentProgressCache();
        private readonly ProgressNotificationCache _progressNotificationCache = new ProgressNotificationCache();

        private readonly ProgressEvaluator _progressEvaluator;
        private readonly SessionTracker _sessionTracker;
        private readonly EvaluationController _evaluationController;

        private bool _isDisposed;

        public EvaluationTracker(ProgressEvaluator progressEvaluator,
            EvaluationController evaluationController,
            SessionTracker sessionTracker)
        {
            _progressEvaluator = progressEvaluator;
            _evaluationController = evaluationController;
            _sessionTracker = sessionTracker;

            _sessionTracker.SessionStartedEvent += OnSessionStarted;
            _sessionTracker.SessionEndedEvent += OnSessionEnded;
            EvaluationDataController.EvaluationDataAddedEvent += OnEvaluationDataAdded;
            EvaluationController.EvaluationCreatedEvent += OnEvaluationCreated;
            EvaluationController.EvaluationUpdatedEvent += OnEvaluationUpdated;
            EvaluationController.EvaluationDeletedEvent += OnEvaluationDeleted;
        }

        ~EvaluationTracker()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _sessionTracker.SessionStartedEvent -= OnSessionStarted;
            _sessionTracker.SessionEndedEvent -= OnSessionEnded;
            EvaluationDataController.EvaluationDataAddedEvent -= OnEvaluationDataAdded;
            EvaluationController.EvaluationCreatedEvent -= OnEvaluationCreated;
            EvaluationController.EvaluationUpdatedEvent -= OnEvaluationUpdated;
            EvaluationController.EvaluationDeletedEvent -= OnEvaluationDeleted;

            _isDisposed = true;
        }

        // <actorId, <evaluation, progress>>
        public ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> GetPendingNotifications(int? gameId, int actorId)
        {
            return _progressNotificationCache.Get(gameId, actorId);
        }

        private void OnSessionStarted(Session session)
        {
            var evaluations = GetEvaluations(session.GameId);
            var progress = _progressEvaluator.EvaluateActor(evaluations, session);
            _progressNotificationCache.Update(progress);
        }

        private void OnSessionEnded(Session session)
        {
            _concurrentProgressCache.RemoveActor(session.GameId, session.ActorId);
            _progressNotificationCache.Remove(session.GameId, session.ActorId);
        }

        private void OnEvaluationDataAdded(EvaluationData evaluationData)
        {
            ICollection<Evaluation> evaluations;

            if (_EvaluationDataToEvaluationMapper.TryGetRelated(evaluationData, out evaluations))
            {
                var gameIds = GetGameIdsFromEvaluations(evaluations);
                var sessions = _sessionTracker.GetByGames(gameIds);

                var progress = _progressEvaluator.EvaluateSessions(sessions, evaluations);
                _progressNotificationCache.Update(progress);
            }
        }

        private void OnEvaluationCreated(Evaluation evaluation)
        {
            _EvaluationDataToEvaluationMapper.CreateMapping(evaluation);

            var gameIds = GetGameIdsFromEvaluation(evaluation);
            var sessions = _sessionTracker.GetByGames(gameIds);

            var progress = _progressEvaluator.EvaluateSessions(sessions, evaluation);
            _progressNotificationCache.Update(progress);
        }

        private void OnEvaluationUpdated(Evaluation evaluation)
        {
            OnEvaluationDeleted(evaluation);
            OnEvaluationCreated(evaluation);
        }

        private void OnEvaluationDeleted(Evaluation evaluation)
        {
            _EvaluationDataToEvaluationMapper.RemoveMapping(evaluation);
            _concurrentProgressCache.Remove(evaluation.Id);
            _progressNotificationCache.Remove(evaluation.Id);
        }

        public void MapExistingEvaluations()
        {
            var evaluations = _evaluationController.Get();
            _EvaluationDataToEvaluationMapper.CreateMappings(evaluations);
        }

        private List<Evaluation> GetEvaluations(int? gameId)
        {
            var evaluations = _evaluationController.GetByGame(gameId).ToList();

            if (gameId != null)
            {
                evaluations.AddRange(_evaluationController.GetByGame(null));
            }

            return evaluations;
        }

        private List<int?> GetGameIdsFromEvaluations(ICollection<Evaluation> evaluations)
        {
            var hasGlobal = false;
            var gameIds = evaluations.Select(e =>
            {
                hasGlobal |= e.GameId == null;
                return e.GameId;
            }).Distinct().ToList();

            if (!hasGlobal)
            {
                gameIds.Add(null);
            }

            return gameIds;
        }

        private List<int?> GetGameIdsFromEvaluation(Evaluation evaluation)
        {
            var gameIds = new List<int?>()
            {
                evaluation.GameId
            };

            if (evaluation.GameId != null)
            {
                gameIds.Add(null);
            }

            return gameIds;
        }
    }
}
