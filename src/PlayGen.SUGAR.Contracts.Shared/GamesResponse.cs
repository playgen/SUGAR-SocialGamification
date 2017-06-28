using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class GamesResponse : Response
	{
		public GameResponse[] Items { get; set; }
	}
}
