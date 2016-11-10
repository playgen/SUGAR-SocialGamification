namespace PlayGen.SUGAR.Data.Model
{
    public class ActorClaim
    {
        public int ActorId { get; set; }

        public Actor Actor { get; set; }

        public int PermissionId { get; set; }

        public Claim Permission { get; set; }

        public int EntityId { get; set; }
    }
}