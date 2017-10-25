using System.ComponentModel.DataAnnotations;
using PlayGen.SUGAR.Common.Authorization;

namespace PlayGen.SUGAR.Contracts
{
    /// <summary>
    /// Encapsulates role details.
    /// </summary>
    /// <example>
    /// JSON
    /// {
    /// Name : "Role Name"
    /// }
    /// </example>
    public class RoleRequest
    {
        /// <summary>
        /// The display name for the role.
        /// </summary>
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// The ClaimScope of this role.
        /// </summary>
        public ClaimScope ClaimScope { get; set; }
    }
}

