using System.Collections.Generic;
using PlayGen.SUGAR.Common.Permissions;

namespace PlayGen.SUGAR.Server.Model
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
