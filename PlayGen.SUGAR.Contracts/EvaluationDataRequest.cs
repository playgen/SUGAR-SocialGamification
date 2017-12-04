using System.ComponentModel.DataAnnotations;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates ActorData/GameData details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// ActorId : 1,
	/// GameId : 1,
	/// Key : "Data Key",
	/// Value : "10",
	/// EvaluationDataType : "Long"
	/// }
	/// </example>
	public class EvaluationDataRequest
    {
		/// <summary>
		/// The id of the Actor which this ActorData/GameData is being ensigned to.
		/// </summary>
		[Required]
		public int CreatingActorId { get; set; }

		/// <summary>
		/// The id of the Game which this ActorData/GameData relates to.
		/// </summary>
		[Required]
		public int GameId { get; set; }

        /// <summary>
        /// Id of the match this data is related to.
        /// </summary>
        public int? MatchId { get; set; }

		/// <summary>
		/// The identifier of the data being stored.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Key { get; set; }

		/// <summary>
		/// The value of the data being stored.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Value { get; set; }

		/// <summary>
		/// The type of data which is being stored.
		/// </summary>
		[Required]
		public EvaluationDataType EvaluationDataType { get; set; }
	}
}
