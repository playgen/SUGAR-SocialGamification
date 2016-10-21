using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates GameData details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// ActorId : 1,
	/// GameId : 1,
	/// Key : "Data Key",
	/// Value : "10",
	/// GameDataType : "Long"
	/// }
	/// </example>
	public class GameDataRequest : IRequest
    {
		/// <summary>
		/// The id of the Actor which this GameData is being ensigned to. Can be left null to ensign to the system/game.
		/// </summary>
		public int? ActorId { get; set; }

		/// <summary>
		/// The id of the Game which this GameData relates to. Can be left null to relate the GameData to the wider system.
		/// </summary>
		public int? GameId { get; set; }

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
		public GameDataType GameDataType { get; set; }
	}
}
