using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Core.Controllers;

namespace PlayGen.SUGAR.Core.Sessions
{
    public class SessionTracker : IDisposable
    {
        public event Action<Session> SessionStartedEvent;
        public event Action<Session> SessionEndedEvent;

        private bool _isDisposed;
        
        private readonly List<Session> _sessions = new List<Session>();

        public SessionTracker()
        {
            ActorController.ActorDeletedEvent += OnActorDeleted;
            GameController.GameDeletedEvent += OnGameDeleted;
        }

        ~SessionTracker()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            ActorController.ActorDeletedEvent -= OnActorDeleted;
            GameController.GameDeletedEvent -= OnGameDeleted;

            _isDisposed = true;
        }
        
        public void StartSession(int? gameId, int actorId)
        {
            var session = new Session
            {
                GameId = gameId,
                ActorId = actorId
            };

            _sessions.Add(session);

            SessionStartedEvent(session);
        }
        
        public void EndSession(int? gameId, Actor actor)
        {
            var session = _sessions.Single(s => s.GameId == gameId && s.ActorId == actor.Id);
            _sessions.Remove(session);

            SessionEndedEvent(session);
        }

        public List<Session> GetByActor(int actorId)
        {
            return _sessions.Where(s => s.ActorId == actorId).ToList();
        }

        public List<Session> GetByGames(List<int?> gameIds)
        {
            return _sessions.Where(s => gameIds.Contains(s.GameId)).ToList();
        }

        private void OnActorUpdated(Actor updatedActor)
        {
            var actorsSessions = GetByActor(updatedActor.Id);
            actorsSessions.ForEach(s => s.ActorId = updatedActor.Id);
        }

        private void OnActorDeleted(int actorId)
        {
            _sessions.RemoveAll(s => s.ActorId == actorId);
        }
        
        private void OnGameDeleted(int gameId)
        {
            _sessions.RemoveAll(s => s.GameId == gameId);
        }
    }
}
