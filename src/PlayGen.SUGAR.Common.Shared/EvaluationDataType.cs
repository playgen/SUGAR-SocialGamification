namespace PlayGen.SUGAR.Common
{
	/// <summary>
	///     Enum for selecting the type of data being stored or looked for.
	/// </summary>
	public enum EvaluationDataType
	{
		/// <summary>
		///     A non-numeric set of characters.
		/// </summary>
		String = 0,

		/// <summary>
		///     Numeric value with no decimal places.
		/// </summary>
		Long,

		/// <summary>
		///     Numeric value with decimal places.
		/// </summary>
		Float,

		/// <summary>
		///     True or false.
		/// </summary>
		Boolean
	}
}