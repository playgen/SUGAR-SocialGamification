namespace PlayGen.SUGAR.Contracts
{
    /// <summary>
    /// Encapsulates roleclaim details from the server.
    /// </summary>
    /// <example>
    /// JSON
    /// {
    /// "RoleId" : 1,
    /// "ClaimId" : 1
    /// }
    /// </example>
    public class RoleClaimResponse
    {
        /// <summary>
        /// The ID of the role related to this roleclaim.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// The ID of the claim related to this roleclaim.
        /// </summary>
        public int ClaimId { get; set; }
    }
}
