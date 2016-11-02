namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Enum for selecting target comparison between current value and target value.
	/// </summary>
	public enum ComparisonType
	{
		/// <summary>
		/// Current and target are equal in value.
		/// </summary>
		Equals = 0,
		/// <summary>
		/// Current and target are not equal in value.
		/// </summary>
		NotEqual,
		/// <summary>
		/// Current is greater in value than the target.
		/// </summary>
		Greater,
		/// <summary>
		/// Current is greater than or equal to the value of the target.
		/// </summary>
		GreaterOrEqual,
		/// <summary>
		/// Current is less than the value of the target.
		/// </summary>
		Less,
		/// <summary>
		/// Current is less than or equal to the value of the target.
		/// </summary>
		LessOrEqual
	}
}
