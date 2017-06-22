namespace PlayGen.SUGAR.Contracts
{
    /// <summary>
    /// Encapsulates actorrole details from the server.
    /// </summary>
    /// <example>
    /// JSON
    /// {
    /// Id : 1,
    /// ActorId : 1,
    /// RoleId : 1,
    /// EntityId : 1
    /// }
    /// </example>
    public class ActorRoleResponse
    {
        /// <summary>
        /// The unqiue identifier for the actorrole.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the actor related to this actorrole.
        /// </summary>
        public int ActorId { get; set; }

        /// <summary>
        /// The ID of the role related to this actorrole.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// The ID of the entity (game, actor etc) related to this actorrole.
        /// </summary>
        public int? EntityId { get; set; }
    }
}