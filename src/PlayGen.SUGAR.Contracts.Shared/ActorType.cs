namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Enum for selecting the target type of actor.
	/// </summary>
	public enum ActorType
	{
		/// <summary>
		/// Intended for either Users or Groups.
		/// </summary>
		Undefined = 0,
		/// <summary>
		/// Intended for just Users.
		/// </summary>
		User,
		/// <summary>
		/// Intended for just Groups.
		/// </summary>
		Group
	}
}