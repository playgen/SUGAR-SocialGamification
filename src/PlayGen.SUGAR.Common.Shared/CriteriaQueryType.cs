namespace PlayGen.SUGAR.Common.Shared
{
	/// <summary>
	/// Enum for selecting what values will be used to query if criteria is met.
	/// </summary>
	public enum CriteriaQueryType
	{
		/// <summary>
		/// Check if any singular piece of EvaluationData matches the criteria.
		/// </summary>
		Any = 0,
		/// <summary>
		/// Check if a sum of all EvaluationData for that key matches the crteria (Long and Float only).
		/// </summary>
		Sum,
		/// <summary>
		/// Check if the most recent piece of EvaluationData matches the criteria.
		/// </summary>
		Latest
	}
}
