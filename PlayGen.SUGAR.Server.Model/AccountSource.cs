namespace PlayGen.SUGAR.Data.Model
{
    public class AccountSource
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Token { get; set; }

        public bool RequiresPassword { get; set; }

        public bool AutoRegister { get; set; }

        public string UsernamePattern { get; set; }

        public string ApiSecret { get; set; }
    }
}

