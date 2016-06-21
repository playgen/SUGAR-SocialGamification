namespace PlayGen.SGA.Contracts
{
    public class AccountRequest
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public bool AutoLogin { get; set; }
    }
}
