using System.Linq;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	// todo comment
	public class MatchClient : ClientBase
	{
		private const string ControllerPrefix = "api/match";

		public MatchClient(string baseAddress, IHttpHandler httpHandler, AsyncRequestController asyncRequestController, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, asyncRequestController, evaluationNotifications)
		{
		}

	    public MatchResponse Start()
	    {
	        var query = GetUriBuilder(ControllerPrefix + "/start").ToString();
	        return Get<MatchResponse>(query);
	    }

	    public MatchResponse End(int matchId)
	    {
	        var query = GetUriBuilder(ControllerPrefix + "/{0}/end", matchId).ToString();
	        return Get<MatchResponse>(query);
	    }
	}
}
