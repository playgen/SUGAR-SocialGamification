using System;
using System.Threading;

namespace PlayGen.SUGAR.Server.Core.Sessions
{
	public class Session
	{
		public long Id { get; private set; }

		public int ActorId { get; private set; }

		public int? GameId { get; private set; }

		public DateTime LastActive { get; internal set; }

		private static long _idCounter;

		public Session(int? gameId, int actorId)
		{
			Id = Interlocked.Increment(ref _idCounter);
			ActorId = actorId;
			GameId = gameId;
			LastActive = DateTime.UtcNow;
		}
	}
}
