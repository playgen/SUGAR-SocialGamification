using System;

namespace PlayGen.SUGAR.Common
{
	/// <summary>
    /// Enum for filtering the visibility type of an actor
    /// </summary>
	[Flags]
	public enum ActorVisibilityFilter
	{
		/// <summary>
        /// Public only
        /// </summary>
		Public = 0,

		/// <summary>
        /// Private only
        /// </summary>
		Private = 1 << 0,

		/// <summary>
        /// Public and Private
        /// </summary>
		All = Private | Public
    }
}