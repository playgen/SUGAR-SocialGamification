namespace PlayGen.SUGAR.Server.Model
{
    public class RoleClaim
    {
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public int ClaimId { get; set; }

        public Claim Claim { get; set; }
    }
}
