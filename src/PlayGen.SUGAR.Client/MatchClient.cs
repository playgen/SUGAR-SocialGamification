using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        
        public List<MatchResponse> GetByTime(DateTime start, DateTime end)
        {
            var query = GetUriBuilder(ControllerPrefix + "/{0}/{1}", start, end).ToString();
            return Get<List<MatchResponse>>(query);
        }

        public List<MatchResponse> GetByGame(int gameId)
        {
            var query = GetUriBuilder(ControllerPrefix + "/{0}", gameId).ToString();
            return Get<List<MatchResponse>>(query);
        }

        public List<MatchResponse> GetByGame(int gameId, DateTime start, DateTime end)
        {
            var query = GetUriBuilder(ControllerPrefix + "/{0}/{1}/{2}", gameId, start, end).ToString();
            return Get<List<MatchResponse>>(query);
        }

        public List<MatchResponse> GetByCreator(int creatorId)
        {
            var query = GetUriBuilder(ControllerPrefix + "/{0}", creatorId).ToString();
            return Get<List<MatchResponse>>(query);
        }

        public List<MatchResponse> GetByCreator(int creatorId, DateTime start, DateTime end)
        {
            var query = GetUriBuilder(ControllerPrefix + "/{0}/{1}/{2}", creatorId, start, end).ToString();
            return Get<List<MatchResponse>>(query);
        }

        public List<MatchResponse> GetByGameAndCreator(int gameId, int creatorId)
        {
            var query = GetUriBuilder(ControllerPrefix + "/{0}/{1}", gameId, creatorId).ToString();
            return Get<List<MatchResponse>>(query);
        }

        public List<MatchResponse> GetByGameAndCreator(int gameId, int creatorId, DateTime start, DateTime end)
        {
            var query = GetUriBuilder(ControllerPrefix + "/{0}/{1}/{2}/{3}", gameId, creatorId, start, end).ToString();
            return Get<List<MatchResponse>>(query);
        }
    }
}
