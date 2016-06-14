namespace PlayGen.SGA.Contracts
{
    public class Relationship
    {
        public int Id { get; set; }

        public Actor Requestor { get; set; }

        public Actor Acceptor { get; set; }
    }
}
