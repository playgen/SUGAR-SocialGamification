using System;

namespace PlayGen.SUGAR.Contracts.Shared
{
	/// <summary>
	/// Encapsulates a match entity's details from the database.
	/// </summary>
	public class MatchResponse
	{
		/// <summary>
		/// Unique Id of the match
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Game that the match was in
		/// </summary>
		public GameResponse Game { get; set; }

		/// <summary>
		/// User that created the match
		/// </summary>
		public ActorResponse Creator { get; set; }

		/// <summary>
		/// When the match was started
		/// </summary>
		public DateTime? Started { get; set; }

		/// <summary>
		/// When the match was ended
		/// </summary>
		public DateTime? Ended { get; set; }
	}
}
