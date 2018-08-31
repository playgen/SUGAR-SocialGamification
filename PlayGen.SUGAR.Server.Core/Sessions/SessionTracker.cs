using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Server.Core.Controllers;

namespace PlayGen.SUGAR.Server.Core.Sessions
{
	public class SessionTracker : IDisposable
	{
		public event Action<Session> SessionStartedEvent;
		public event Action<Session> SessionEndedEvent;

		private readonly ConcurrentDictionary<long, Session> _sessions = new ConcurrentDictionary<long, Session>();
		private readonly ILogger _logger;
		private readonly TimeSpan _sessionTimeout;
		private readonly Timer _timer;

		private bool _isDisposed;

		public SessionTracker(
			ILogger<SessionTracker> logger,
			TimeSpan sessionTimeout, 
			TimeSpan timeoutCheckInterval)
		{
			_logger = logger;
			_sessionTimeout = sessionTimeout;
			ActorController.DeleteActorEvent += OnDeleteActor;
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

			ActorController.DeleteActorEvent -= OnDeleteActor;
			GameController.GameDeletedEvent -= OnGameDeleted;

			_isDisposed = true;
		}

		public Session StartSession(int gameId, int actorId)
		{
			var session = new Session(gameId, actorId);

			_sessions.TryAdd(session.Id, session);

			SessionStartedEvent?.Invoke(session);

			_logger.LogInformation($"SessionId: {session.Id} for GameId: {gameId}, ActorId: {actorId}");

			return session;
		}
		
		public void EndSession(long sessionId)
		{
			_sessions.TryRemove(sessionId, out var session);

			SessionEndedEvent?.Invoke(session);

			_logger.LogInformation($"SessionId: {session.Id}");
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

			_logger.LogInformation($"Timedout: {string.Join(", ", sessionIds)}");

			sessionIds.ForEach(EndSession);
		}

		private void OnDeleteActor(int actorId)
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
