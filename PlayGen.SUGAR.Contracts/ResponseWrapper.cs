using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts.Shared
{
    public class ResponseWrapper<TResponse>
    {
        public TResponse Response { get; set; }

        public List<EvaluationProgressResponse> EvaluationsProgress { get; set; }
    }
}