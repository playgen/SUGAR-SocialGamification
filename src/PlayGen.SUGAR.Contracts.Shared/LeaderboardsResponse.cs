using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class LeaderboardsResponse : Response
	{
		public LeaderboardResponse[] Items { get; set; }
	}
}
