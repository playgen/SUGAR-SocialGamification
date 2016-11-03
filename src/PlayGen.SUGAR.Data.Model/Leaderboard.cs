namespace PlayGen.SUGAR.Data.Model
{
	public class Leaderboard
	{
		public int GameId { get; set; }

		public string Name { get; set; }

		public string Key { get; set; }

		public string Token { get; set; }

		public Common.Shared.GameDataType GameDataType { get; set; }

		public Common.Shared.ActorType ActorType { get; set; }

		public Common.Shared.CriteriaScope CriteriaScope { get; set; }

		public Common.Shared.LeaderboardType LeaderboardType { get; set; }
	}
}
