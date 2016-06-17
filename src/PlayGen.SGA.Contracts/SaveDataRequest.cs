namespace PlayGen.SGA.Contracts
{
    public class SaveDataRequest
    {
        public int ActorId { get; set; }

        public int GameId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public DataType DataType { get; set; }
    }
}
