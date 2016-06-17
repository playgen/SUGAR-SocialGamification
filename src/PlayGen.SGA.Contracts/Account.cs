namespace PlayGen.SGA.Contracts
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public Permissions Permission { get; set; }

        public enum Permissions
        {
            Default,
            Admin
        }
    }
}
