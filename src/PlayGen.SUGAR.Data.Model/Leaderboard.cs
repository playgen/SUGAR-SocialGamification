using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Data.Model
{
	public class Leaderboard
	{
		public int GameId { get; set; }

		public string Name { get; set; }

		public string Key { get; set; }

		public string Token { get; set; }

		public GameDataType GameDataType { get; set; }

		public ActorType ActorType { get; set; }

		public CriteriaScope CriteriaScope { get; set; }

		public LeaderboardType LeaderboardType { get; set; }
	}
}
