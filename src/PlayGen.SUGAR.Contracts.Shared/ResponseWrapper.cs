using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts
{
	public class ResponseWrapper
	{
		public Response Response;

		public List<EvaluationProgressResponse> EvaluationsProgress { get; set; }
	}
}