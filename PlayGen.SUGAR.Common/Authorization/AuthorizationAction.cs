namespace PlayGen.SUGAR.Common.Authorization
{
	/// <summary>
	/// Enum for the actions that can be performed on an entity
	/// </summary>
	public enum AuthorizationAction
	{
		/// <summary>
		/// Creation of the entity
		/// </summary>
		Create = 0,
		/// <summary>
		/// Retrieval of the entity
		/// </summary>
		Get = 1,
		/// <summary>
		/// Updating of the entity
		/// </summary>
		Update = 2,
		/// <summary>
		/// Deletion of the entity
		/// </summary>
		Delete = 3
	}
}
