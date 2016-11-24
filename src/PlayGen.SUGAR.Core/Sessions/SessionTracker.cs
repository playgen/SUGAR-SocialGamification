using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Core.Sessions
{
    public class SessionTracker
    {
        public event Action<Session> SessionStartedEvent;
        public event Action<Session> SessionEndedEvent;

        private bool _isDisposed;
        
        private readonly List<Session> _sessions = new List<Session>();
        
        public void StartSession(int? gameId, Actor actor)
        {
            var session = new Session
            {
                GameId = gameId,
                Actor = actor
            };

            _sessions.Add(session);

            SessionStartedEvent(session);
        }
        
        public void EndSession(int? gameId, Actor actor)
        {
            var session = _sessions.Single(s => s.GameId == gameId && s.Actor.Id == actor.Id);
            _sessions.Remove(session);

            SessionEndedEvent(session);
        }

        public List<Session> GetByGames(List<int?> gameIds)
        {
            return _sessions.Where(s => gameIds.Contains(s.GameId)).ToList();
        }
    }
}
