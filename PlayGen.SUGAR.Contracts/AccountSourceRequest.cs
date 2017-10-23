using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts.Shared
{
    /// <summary>
    /// Encapsulates log-in source details.
    /// </summary>
    /// <example>
    /// JSON
    /// {
    /// Name : "SUGAR",
    /// RequiresPassword : true
    /// }
    /// </example>
    public class AccountSourceRequest
    {
        /// <summary>
        /// The source description.
        /// </summary>
        [Required]
        [StringLength(64)]
        public string Description { get; set; }

        /// <summary>
        /// The source token.
        /// </summary>
        [Required]
        [StringLength(128)]
        public string Token { get; set; }

        /// <summary>
        /// Whether the user needs to pass a password when logging in via this source
        /// </summary>
        public bool RequiresPassword { get; set; }

        /// <summary>
        /// Whether an account is created if one does not already exist for this source
        /// </summary>
        public bool AutoRegister { get; set; }
    }
}

