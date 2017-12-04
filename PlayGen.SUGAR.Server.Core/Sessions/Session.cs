using System;
using System.Threading;

namespace PlayGen.SUGAR.Server.Core.Sessions
{
	public class Session
	{
		public long Id { get; }

		public int ActorId { get; }

		public int GameId { get; }

		public DateTime LastActive { get; internal set; }

		private static long _idCounter;

		public Session(int gameId, int actorId)
		{
			Id = Interlocked.Increment(ref _idCounter);
			ActorId = actorId;
			GameId = gameId;
			LastActive = DateTime.UtcNow;
		}
	}
}
