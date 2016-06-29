using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates game details.
	/// </summary>
	public class GameRequest
	{
		[Required]
		[StringLength(64)]
		public string Name { get; set; }
	}
}
