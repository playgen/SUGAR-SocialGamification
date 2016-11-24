using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Data.Model
{
	public class ActorClaim
	{
		public int Id { get; set; }

		public int ActorId { get; set; }

		public Actor Actor { get; set; }

		public int ClaimId { get; set; }

		public Claim Claim { get; set; }

		public int? EntityId { get; set; }
	}
}