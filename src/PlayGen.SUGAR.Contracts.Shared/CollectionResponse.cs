using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class CollectionResponse : IResponse
	{
		public IResponse[] Items { get; set; } 
	}
}
