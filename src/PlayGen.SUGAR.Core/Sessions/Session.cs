using System;

namespace PlayGen.SUGAR.Core.Sessions
{
	public class Session
	{
		private static long _idCounter;

		public Session(int? gameId, int actorId)
		{
			Id = ++_idCounter;
			ActorId = actorId;
			GameId = gameId;
			LastActive = DateTime.UtcNow;
		}

		public long Id { get; }

		public int ActorId { get; }

		public int? GameId { get; }

		public DateTime LastActive { get; internal set; }
	}
}