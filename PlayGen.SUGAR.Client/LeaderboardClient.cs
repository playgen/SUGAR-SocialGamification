using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Leaderboard specific operations.
	/// </summary>
	public class LeaderboardClient : ClientBase
	{
		private const string ControllerPrefix = "api/leaderboards";

		public LeaderboardClient(
			string baseAddress,
			IHttpHandler httpHandler,
			Dictionary<string, string> constantHeaders,
			Dictionary<string, string> sessionHeaders,
			AsyncRequestController asyncRequestController,
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, constantHeaders, sessionHeaders, asyncRequestController, evaluationNotifications)
		{
		}

		/// <summary>
		/// Get all global leaderboards, ie. leaderboards that are not associated with a specific game
		/// </summary>
		/// <returns>Returns multiple <see cref="LeaderboardResponse"/> that hold Leaderboard details</returns>
		public IEnumerable<LeaderboardResponse> GetGlobal()
		{
			var query = GetUriBuilder(ControllerPrefix + "/global/list").ToString();
			return Get<IEnumerable<LeaderboardResponse>>(query);
		}

		public void GetGlobalAsync(Action<IEnumerable<LeaderboardResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(GetGlobal,
				onSuccess,
				onError);
		}

		/// <summary>
		/// Find a list of leaderboards that match <param name="gameId"/>.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="LeaderboardResponse"/> that hold Leaderboard details</returns>
		public IEnumerable<LeaderboardResponse> Get(int gameId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/game/{0}/list", gameId).ToString();
			return Get<IEnumerable<LeaderboardResponse>>(query);
		}

		public void GetAsync(int gameId, Action<IEnumerable<LeaderboardResponse>> onSuccess, Action<Exception> onError)
		{
		    AsyncRequestController.EnqueueRequest(() => Get(gameId),
		        onSuccess,
		        onError);
		}

		/// <summary>
		/// Find a single global leaderboard matching the token.
		/// </summary>
		/// <param name="token">Token </param>
		/// <returns>Returns a single <see cref="LeaderboardResponse"/> that holds Leaderboard details</returns>
		public LeaderboardResponse GetGlobal(string token)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/global", token).ToString();
			return Get<LeaderboardResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		public void GetGlobalAsync(string token, Action<LeaderboardResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetGlobal(token),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Find a single leaderboard matching the token and gameId.
		/// </summary>
		/// <param name="token">Token </param>
		/// <param name="gameId"></param>
		/// <returns>Returns a single <see cref="LeaderboardResponse"/> that holds Leaderboard details</returns>
		public LeaderboardResponse Get(string token, int gameId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/{1}", token, gameId).ToString();
			return Get<LeaderboardResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		public void GetAsync(string token, int gameId, Action<LeaderboardResponse> onSuccess, Action<Exception> onError)
		{
		    AsyncRequestController.EnqueueRequest(() => Get(token, gameId),
		        onSuccess,
		        onError);
		}

		/// <summary>
		/// Get the standings for a Leaderboard using a <see cref="LeaderboardStandingsRequest"/>.
		/// </summary>
		/// <param name="leaderboardDetails"><see cref="LeaderboardStandingsRequest"/> object that holds the details that are wanted from the Leaderboard.</param>
		/// <returns>Returns multiple <see cref="LeaderboardStandingsResponse"/> that hold actor positions in the leaderboard.</returns>
		public IEnumerable<LeaderboardStandingsResponse> CreateGetLeaderboardStandings(LeaderboardStandingsRequest leaderboardDetails)
		{
			var query = GetUriBuilder(ControllerPrefix + "/standings").ToString();
			return Post<LeaderboardStandingsRequest, IEnumerable<LeaderboardStandingsResponse>>(query, leaderboardDetails);
		}

		public void CreateGetLeaderboardStandingsAsync(LeaderboardStandingsRequest leaderboardDetails, Action<IEnumerable<LeaderboardStandingsResponse>> onSuccess, Action<Exception> onError)
		{
		    AsyncRequestController.EnqueueRequest(() => CreateGetLeaderboardStandings(leaderboardDetails),
		        onSuccess,
		        onError);
		}
	}
}
