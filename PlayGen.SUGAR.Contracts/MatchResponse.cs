using System;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates a match entity's details from the database.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "Id" : 1,
	/// "Game" : {
	/// "Id" : 1,
	/// "Name" : "Game Name"
	/// },
	/// "Creator" : {
	/// "Id" : 1,
	/// "Name" : "Actor Name"
	/// "Description" : "Description of Actor"
	/// },
	/// "Started" : "2018-08-12T16:32:29.482146",
	/// "Ended" : "2018-08-12T18:32:29.482146"
	/// }
	/// </example>
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
