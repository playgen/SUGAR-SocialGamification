namespace PlayGen.SGA.Contracts
{
    public class SaveData
    {
        public int Id { get; set; }

        public int ActorId { get; set; }

        public int GameId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public bool Amend { get; set; }
    }
}
