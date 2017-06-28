using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class RewardsResponse : Response
	{
		public RewardResponse[] Items { get; set; }
	}
}
