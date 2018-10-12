namespace PlayGen.SUGAR.Common
{
	/// <summary>
	/// Enum for the type of data being stored into/loaded from EvlautionData.
	/// </summary>
	public enum EvaluationDataCategory
	{
		/// <summary>
		/// Generic data related to a game, usually used for evaluations and leaderboards
		/// </summary>
		GameData = 0,
		/// <summary>
		/// Data related to quantities which can be added to, removed from and transferred between actors
		/// </summary>
		Resource = 1,
		/// <summary>
		/// Data indicating that a Skill's criteria has been completed
		/// </summary>
		Skill = 2,
		/// <summary>
		/// Data indicating that an Achievement's criteria has been completed
		/// </summary>
		Achievement = 3,
		/// <summary>
		/// Generic data related to a match
		/// </summary>
		MatchData = 4
	}
}