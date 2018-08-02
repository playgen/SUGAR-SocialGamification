namespace PlayGen.SUGAR.Common
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
		Sum = 1,
		/// <summary>
		/// Check if the most recent piece of EvaluationData matches the criteria.
		/// </summary>
		Latest = 2,
        /// <summary>
		/// Check if the count of all EvaluationData matches the criteria.
        /// </summary>
        Count = 3
	}
}
