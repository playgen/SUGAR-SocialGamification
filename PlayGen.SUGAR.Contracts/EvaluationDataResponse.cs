using System;

using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates ActorData/EvaluationData details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "GameId" : 1,
	/// "MatchId" : 1,
	/// "CreatingActorId" : 1,
	/// "Key" : "Data Key",
	/// "Value" : "10",
	/// "EvaluationDataType" : "Long",
	/// "DateCreated" : "2018-08-12T16:32:29.482146",
	/// "DateModified" : "2018-08-12T16:32:29.482146"
	/// }
	/// </example>
	public class EvaluationDataResponse
	{
		/// <summary>
		/// The id of the Game which this ActorData/EvaluationData relates to.
		/// </summary>
		public int GameId { get; set; }

		/// <summary>
		/// Id of the match this data is related to.
		/// </summary>
		public int? MatchId { get; set; }

		/// <summary>
		/// The id of the Actor which this ActorData/EvaluationData relates to.
		/// </summary>
		public int? CreatingActorId { get; set; }

		/// <summary>
		/// The identifier of the data.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// The value of the data.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// The type of the value for this data.
		/// </summary>
		public EvaluationDataType EvaluationDataType { get; set; }

		/// <summary>
		/// The DateTime at which this data was created. Can be left null for data when this does not apply (summed data totals or counts)
		/// </summary>
		public DateTime? DateCreated { get; set; }

		/// <summary>
		/// The DateTime at which this data was last edited. Can be left null for data when this does not apply (summed data totals or counts)
		/// </summary>
		public DateTime? DateModified { get; set; }
	}
}
