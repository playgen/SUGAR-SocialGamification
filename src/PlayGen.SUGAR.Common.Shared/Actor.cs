namespace PlayGen.SUGAR.Common.Shared
{
    public abstract class Actor
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public abstract Common.Shared.ActorType ActorType { get; }
    }
}
