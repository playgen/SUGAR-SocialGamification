namespace PlayGen.SGA.Contracts
{
    /// <summary>
    /// Encapsulates log-in details.
    /// </summary>
    public class AccountRequest
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
