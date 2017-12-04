using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates actorrole details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// ActorId : 1,
	/// RoleId : 1,
	/// EntityId : 1
	/// }
	/// </example>
	public class ActorRoleRequest
    {
        /// <summary>
        /// The ID of the actor related to this actorrole.
        /// </summary>
        [Required]
        public int ActorId { get; set; }

        /// <summary>
        /// The ID of the role related to this actorrole.
        /// </summary>
        [Required]
        public int RoleId { get; set; }

		/// <summary>
		/// The ID of the entity (game, actor etc) related to this actorrole.
		/// </summary>
		[Required]
		public int EntityId { get; set; }
    }
}