using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class GroupsResponse : Response
	{
		public GroupResponse[] Items { get; set; }
	}
}
