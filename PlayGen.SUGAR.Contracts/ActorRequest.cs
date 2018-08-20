using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates user and group details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Name : "Actor Name",
	/// Description: "Description of Actor",
	/// Private : false
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

		/// <summary>
		/// The description/biography of the user/group.
		/// </summary>
		[StringLength(1023)]
		public string Description { get; set; }

		/// <summary>
		/// Whether this actor will be visible in searches
		/// </summary>
		public bool Private { get; set; }
	}
}