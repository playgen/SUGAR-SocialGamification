namespace PlayGen.SGA.Contracts
{
    /// <summary>
    /// Encapsulates relationship details.
    /// </summary>
    public class RelationshipRequest
    {
        public int RequestorId { get; set; }

        public int AcceptorId { get; set; }

        public bool AutoAccept { get; set; }
    }
}
