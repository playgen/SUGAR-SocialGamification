using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class ActorClaimsResponse : Response
	{
		public ActorClaimResponse[] Items { get; set; }
	}
}
