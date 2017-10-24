using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Server.Model
{
    public class ActorRole
    {
        public int Id { get; set; }

        public int ActorId { get; set; }

        public Actor Actor { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

		// TODO confirm whether this needs to be nullable and have the option for -1 = global
        public int? EntityId { get; set; }
    }
}