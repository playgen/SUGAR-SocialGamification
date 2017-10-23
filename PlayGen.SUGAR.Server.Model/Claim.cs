using PlayGen.SUGAR.Common.Shared.Permissions;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Data.Model
{
    public class Claim
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public string Description { get; set; }

        public ClaimScope ClaimScope { get; set; }

		public virtual List<RoleClaim> RoleClaims { get; set; }
	}
}
