using System;

using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Server.Core.EvaluationEvents
{
	public class StandingsRequest
	{
		public string LeaderboardToken { get; set; }

		public int GameId { get; set; }

		public int? ActorId { get; set; }

		public LeaderboardFilterType LeaderboardFilterType { get; set; }

		public int PageLimit { get; set; }

		public int PageOffset { get; set; }

		public bool MultiplePerActor { get; set; }

		public DateTime? DateStart { get; set; }

		public DateTime? DateEnd { get; set; }
	}
}
