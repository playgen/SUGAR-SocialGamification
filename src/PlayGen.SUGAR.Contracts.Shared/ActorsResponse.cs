using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class ActorsResponse : Response
	{
		public ActorResponse[] Items { get; set; }
	}
}
