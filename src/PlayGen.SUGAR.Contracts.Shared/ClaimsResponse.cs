using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class ClaimsResponse : Response
	{
		public ClaimResponse[] Items { get; set; }
	}
}
