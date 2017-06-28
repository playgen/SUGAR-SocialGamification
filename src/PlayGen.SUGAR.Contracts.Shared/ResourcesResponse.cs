using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class ResourcesResponse : Response
	{
		public ResourceResponse[] Items { get; set; }
	}
}
