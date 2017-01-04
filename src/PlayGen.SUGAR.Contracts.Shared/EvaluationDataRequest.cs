using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts.Shared
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
	public class ActorDetailsPostRequest
    {
		/// <summary>
		/// The id of the Actor which this ActorData/GameData is being ensigned to. Can be left null to ensign to the system/game.
		/// </summary>
		public int? ActorId { get; set; }

		/// <summary>
		/// The id of the Game which this ActorData/GameData relates to. Can be left null to relate the ActorData/GameData to the wider system.
		/// </summary>
		public int? GameId { get; set; }

        /// <summary>
        /// Id of the entity this data is related to.
        /// </summary>
        public int? EntityId { get; set; }

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
