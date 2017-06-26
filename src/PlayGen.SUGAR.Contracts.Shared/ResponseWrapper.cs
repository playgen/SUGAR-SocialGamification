using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts
{
	public class ResponseWrapper
	{
		public IResponse Response { get; set; }

		public List<EvaluationProgressResponse> EvaluationsProgress { get; set; }
	}
}