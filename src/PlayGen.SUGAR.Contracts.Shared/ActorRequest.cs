using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts.Shared
{
	/// <summary>
	/// Encapsulates user and group details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Name : "Actor Name"
	/// }
	/// </example>
	public class ActorRequest
    {
		/// <summary>
		/// The display name of the user/group.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Name { get; set; }
	}
}