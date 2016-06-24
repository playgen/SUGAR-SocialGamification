namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates requirements for completing an achievement.
	/// </summary>
	public class AchievementCriteria
	{
		public string Key { get; set; }

		public GameDataValueType DataType { get; set; }

		public ComparisonType ComparisonType { get; set; }

		public string Value { get; set; }
	}
}
