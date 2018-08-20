namespace PlayGen.SUGAR.Common.Authorization
{
	/// <summary>
	/// Enum for the Entity type a Claim or Role is for
	/// </summary>
	public enum ClaimScope
	{
		/// <summary>
		/// Intended for platform functionality that cannot be assigned to one entity
		/// </summary>
		Global = 0,
		/// <summary>
		/// Functionality related to game management
		/// </summary>
		Game = 1,
		/// <summary>
		/// Functionality related to group management
		/// </summary>
		Group = 2,
		/// <summary>
		/// Functionality related to user management
		/// </summary>
		User = 3,
		/// <summary>
		/// Functionality related to account management
		/// </summary>
		Account = 4,
		/// <summary>
		/// Functionality related to role management
		/// </summary>
		Role = 5
	}
}
