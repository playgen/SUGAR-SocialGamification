using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates user and group details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Name : "Name"
	/// }
	/// </example>
	public class ActorRequest
	{
		[Required]
		[StringLength(64)]
		public string Name { get; set; }
	}
}