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
        private static int _idCounter;
        
        private readonly Dictionary<int, Session> _sessions =  new Dictionary<int, Session>();

        public SessionTracker()
        {
            ActorController.ActorDeletedEvent += OnActorDeleted;
            GameController.GameDeletedEvent += OnGameDeleted;
        }

        public void SetLastActive(int sessionId, DateTime lastActive)
        {
            _sessions[sessionId].LastActive = lastActive;
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
        
        public Session StartSession(int? gameId, int actorId)
        {
            var session = new Session
            {
                Id = ++_idCounter,
                GameId = gameId,
                ActorId = actorId
            };

            _sessions[session.Id] = session;

            SessionStartedEvent(session);

            return session;
        }
        
        public void EndSession(int sessionId)
        {
            var session = _sessions[sessionId];
            _sessions.Remove(sessionId);

            SessionEndedEvent(session);
        }

        public List<Session> GetByActor(int actorId)
        {
            return _sessions.Values.Where(s => s.ActorId == actorId).ToList();
        }

        public List<Session> GetByGames(List<int?> gameIds)
        {
            return _sessions.Values.Where(s => gameIds.Contains(s.GameId)).ToList();
        }

        private void OnActorDeleted(int actorId)
        {
            var kvps = _sessions
                .Where(kvp => kvp.Value.ActorId == actorId)
                .Select(kvp => kvp.Key).ToList();

            kvps.ForEach(k => _sessions.Remove(k));
        }
        
        private void OnGameDeleted(int gameId)
        {
            var kvps = _sessions
                .Where(kvp => kvp.Value.GameId == gameId)
                .Select(kvp => kvp.Key).ToList();

            kvps.ForEach(k => _sessions.Remove(k));
        }
    }
}
