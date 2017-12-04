using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NLog;
using PlayGen.SUGAR.Server.Core.Controllers;

namespace PlayGen.SUGAR.Server.Core.Sessions
{
    public class SessionTracker : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public event Action<Session> SessionStartedEvent;
        public event Action<Session> SessionEndedEvent;

        private readonly ConcurrentDictionary<long, Session> _sessions = new ConcurrentDictionary<long, Session>();
        private readonly TimeSpan _sessionTimeout;
        private readonly Timer _timer;

        private bool _isDisposed;

        public SessionTracker(TimeSpan sessionTimeout, TimeSpan timeoutCheckInterval)
        {
            _sessionTimeout = sessionTimeout;
            ActorController.ActorDeletedEvent += OnActorDeleted;
            GameController.GameDeletedEvent += OnGameDeleted;

            _timer = new Timer(state => RemoveTimedOut(), new object(), timeoutCheckInterval, timeoutCheckInterval);
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

            _timer.Dispose();

            ActorController.ActorDeletedEvent -= OnActorDeleted;
            GameController.GameDeletedEvent -= OnGameDeleted;

            _isDisposed = true;
        }

        public Session StartSession(int gameId, int actorId)
        {
            var session = new Session(gameId, actorId);

            _sessions.TryAdd(session.Id, session);

            SessionStartedEvent?.Invoke(session);

            Logger.Info($"SessionId: {session.Id} for GameId: {gameId}, ActorId: {actorId}");

            return session;
        }
        
        public void EndSession(long sessionId)
        {
            Session session;
            _sessions.TryRemove(sessionId, out session);

            SessionEndedEvent?.Invoke(session);

            Logger.Info($"SessionId: {session.Id}");
        }

        public bool IsActive(long sessionId)
        {
            return _sessions.ContainsKey(sessionId);
        }

        public List<Session> GetByActor(int actorId)
        {
            return _sessions.Values.Where(s => s.ActorId == actorId).ToList();
        }

        public List<Session> GetByGames(List<int> gameIds)
        {
            return _sessions.Values.Where(s => gameIds.Contains(s.GameId)).ToList();
        }

        private void RemoveTimedOut()
        {
            var activityThreshold = DateTime.UtcNow - _sessionTimeout;

            var sessionIds = _sessions
                .Where(kvp => kvp.Value.LastActive < activityThreshold)
                .Select(kvp => kvp.Key).ToList();

            Logger.Info($"Timedout: {string.Join(", ", sessionIds)}");

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
