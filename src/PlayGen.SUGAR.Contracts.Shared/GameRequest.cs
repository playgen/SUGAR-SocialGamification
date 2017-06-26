using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	///     Encapsulates game details.
	/// </summary>
	/// <example>
	///     JSON
	///     {
	///     Name : "Game Name"
	///     }
	/// </example>
	public class GameRequest
	{
		/// <summary>
		///     The display name for the game.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Name { get; set; }
	}
}