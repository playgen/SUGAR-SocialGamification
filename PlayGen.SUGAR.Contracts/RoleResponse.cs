using PlayGen.SUGAR.Common.Authorization;

namespace PlayGen.SUGAR.Contracts
{
    /// <summary>
    /// Encapsulates role details from the server.
    /// </summary>
    /// <example>
    /// JSON
    /// {
    /// Id : 1,
    /// Name : "Role Name"
    /// }
    /// </example>
    public class RoleResponse
    {
        /// <summary>
        /// The unqiue identifier for the role.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The display name of the role.
        /// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Is this role a default role for the ClaimScope?
		/// </summary>
		public bool Default { get; set; }

		/// <summary>
		/// The ClaimScope of this role.
		/// </summary>
		public ClaimScope ClaimScope { get; set; }
    }
}
