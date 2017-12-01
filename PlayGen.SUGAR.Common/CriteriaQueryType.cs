namespace PlayGen.SUGAR.Common
{
	/// <summary>
	/// Enum for selecting what values will be used to query if criteria is met.
	/// </summary>
	public enum CriteriaQueryType
	{
		/// <summary>
		/// Check if any singular piece of GameData matches the criteria.
		/// </summary>
		Any = 0,
		/// <summary>
		/// Check if a sum of all GameData for that key matches the crteria (Long and Float only).
		/// </summary>
		Sum = 1,
		/// <summary>
		/// Check if the most recent piece of GameData matches the criteria.
		/// </summary>
		Latest = 2
	}
}
