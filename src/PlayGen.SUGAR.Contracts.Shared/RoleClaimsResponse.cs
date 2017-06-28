using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class RoleClaimsResponse : Response
	{
		public RoleClaimResponse[] Items { get; set; }
	}
}
