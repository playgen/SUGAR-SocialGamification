namespace PlayGen.SUGAR.Contracts.Shared
{
	/// <summary>
	/// Encapsulates ActorData/GameData details from the server.
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
	public class EvaluationDataResponse
	{
		/// <summary>
		/// The id of the Game which this ActorData/GameData relates to.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// Id of the entity this data is related to.
		/// </summary>
		public int? EntityId { get; set; }

		/// <summary>
		/// The id of the Actor which this ActorData/GameData relates to.
		/// </summary>
		public int? ActorId { get; set; }

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
	}
}
