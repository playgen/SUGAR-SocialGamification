namespace PlayGen.SGA.Contracts
{
    /// <summary>
    /// Encapsulates user and token details at log-in.
    /// </summary>
    public class AccountResponse
    {
        public ActorResponse User { get; set; }

        public string Token { get; set; }
    }
}
