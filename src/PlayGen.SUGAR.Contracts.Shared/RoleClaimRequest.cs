using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
    /// <summary>
    /// Encapsulates roleclaim details.
    /// </summary>
    /// <example>
    /// JSON
    /// {
    /// RoleId : 1,
    /// ClaimId : 1
    /// }
    /// </example>
    public class RoleClaimRequest
    {
        /// <summary>
        /// The ID of the role related to this roleclaim.
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// The ID of the claim related to this roleclaim.
        /// </summary>
        [Required]
        public int ClaimId { get; set; }
    }
}
