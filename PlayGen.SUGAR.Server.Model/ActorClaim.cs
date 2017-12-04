using System;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Server.Model
{
	public class ActorClaim : IEquatable<ActorClaim>
	{
		public int Id { get; set; }

		public int ActorId { get; set; }

		public Actor Actor { get; set; }

		public int ClaimId { get; set; }

		public Claim Claim { get; set; }

		public int EntityId { get; set; }

		public bool Equals(ActorClaim other)
		{
			return Id == other.Id;
		}
	}
}