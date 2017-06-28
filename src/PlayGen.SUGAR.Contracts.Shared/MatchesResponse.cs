using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class MatchesResponse : Response
	{
		public MatchResponse[] Items { get; set; }
	}
}
