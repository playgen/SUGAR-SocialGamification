using System;
using System.Collections.Concurrent;
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

        private readonly TimeSpan _sessionTimeout;
        private bool _isDisposed;
        
        private readonly ConcurrentDictionary<long, Session> _sessions =  new ConcurrentDictionary<long, Session>();

        public SessionTracker(TimeSpan sessionTimeout)
        {
            _sessionTimeout = sessionTimeout;
            ActorController.ActorDeletedEvent += OnActorDeleted;
            GameController.GameDeletedEvent += OnGameDeleted;
        }

        public void SetLastActive(long sessionId, DateTime lastActive)
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
            var session = new Session(gameId, actorId);

            _sessions.TryAdd(session.Id, session);

            SessionStartedEvent?.Invoke(session);

            return session;
        }
        
        public void EndSession(long sessionId)
        {
            Session session;
            _sessions.TryRemove(sessionId, out session);

            SessionEndedEvent?.Invoke(session);
        }

        public bool IsActive(long sessionId)
        {
            return _sessions.ContainsKey(sessionId);
        }

        public List<Session> GetByActor(int actorId)
        {
            return _sessions.Values.Where(s => s.ActorId == actorId).ToList();
        }

        public List<Session> GetByGames(List<int?> gameIds)
        {
            return _sessions.Values.Where(s => gameIds.Contains(s.GameId)).ToList();
        }

        public void RemoveTimedOut()
        {
            var activityThreshold = DateTime.UtcNow - _sessionTimeout;

            var sessionIds = _sessions
                .Where(kvp => kvp.Value.LastActive < activityThreshold)
                .Select(kvp => kvp.Key).ToList();

            sessionIds.ForEach(EndSession);
        }

        private void OnActorDeleted(int actorId)
        {
            var sessionIds = _sessions
                .Where(kvp => kvp.Value.ActorId == actorId)
                .Select(kvp => kvp.Key).ToList();

            sessionIds.ForEach(EndSession);
        }
        
        private void OnGameDeleted(int gameId)
        {
            var sessionIds = _sessions
                .Where(kvp => kvp.Value.GameId == gameId)
                .Select(kvp => kvp.Key).ToList();

            sessionIds.ForEach(EndSession);
        }
    }
}
