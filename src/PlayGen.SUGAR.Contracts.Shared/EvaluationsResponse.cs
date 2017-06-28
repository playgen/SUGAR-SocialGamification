using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class EvaluationsResponse : Response
	{
		public EvaluationResponse[] Items { get; set; }
	}
}
