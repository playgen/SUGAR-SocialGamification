using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class EvaluationsProgressResponse : Response
	{
		public EvaluationProgressResponse[] Items { get; set; }
	}
}
