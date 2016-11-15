using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts.Shared
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
    }
}

