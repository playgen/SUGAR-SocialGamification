using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class LeaderboardsStandingsResponse : Response
	{
		public LeaderboardStandingsResponse[] Items { get; set; }
	}
}
