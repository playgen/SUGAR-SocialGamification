using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Data.Model
{
	public class Leaderboard
	{
		public int GameId { get; set; }

		public string Token { get; set; }

		public string Name { get; set; }

		public EvaluationDataCategory EvaluationDataCategory { get; set; }

		public string EvaluationDataKey { get; set; }

		public EvaluationDataType EvaluationDataType { get; set; }

		public ActorType ActorType { get; set; }

		public CriteriaScope CriteriaScope { get; set; }

		public LeaderboardType LeaderboardType { get; set; }
	}
}