using System.Collections.Generic;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Leaderboard specific operations.
	/// </summary>
	public class LeaderboardClient : ClientBase
	{
		public LeaderboardClient(string baseAddress, Credentials credentials, IHttpHandler httpHandler, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, credentials, httpHandler, evaluationNotifications)
		{
		}
		
		/// <summary>
		/// Get all global leaderboards, ie. leaderboards that are not associated with a specific game
		/// </summary>
		/// <returns>Returns multiple <see cref="LeaderboardResponse"/> that hold Leaderboard details</returns>
		public IEnumerable<LeaderboardResponse> GetGlobal()
		{
			var query = GetUriBuilder("api/leaderboards/global/list").ToString();
			return Get<IEnumerable<LeaderboardResponse>>(query);
		}

		/// <summary>
		/// Find a list of leaderboards that match <param name="gameId"/>.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="LeaderboardResponse"/> that hold Leaderboard details</returns>
		public IEnumerable<LeaderboardResponse> Get(int gameId)
		{
			var query = GetUriBuilder("api/leaderboards/game/{0}/list", gameId).ToString();
			return Get<IEnumerable<LeaderboardResponse>>(query);
		}

		/// <summary>
		/// Find a single global leaderboard matching the token.
		/// </summary>
		/// <param name="token">Token </param>
		/// <returns>Returns a single <see cref="LeaderboardResponse"/> that holds Leaderboard details</returns>
		public LeaderboardResponse GetGlobal(string token)
		{
			var query = GetUriBuilder("api/leaderboards/{0}/global", token).ToString();
			return Get<LeaderboardResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Find a single leaderboard matching the token and gameId.
		/// </summary>
		/// <param name="token">Token </param>
		/// <param name="gameId"></param>
		/// <returns>Returns a single <see cref="LeaderboardResponse"/> that holds Leaderboard details</returns>
		public LeaderboardResponse Get(string token, int gameId)
		{
			var query = GetUriBuilder("api/leaderboards/{0}/{1}", token, gameId).ToString();
			return Get<LeaderboardResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Create a new Leaderboard.
		/// Requires <see cref="LeaderboardRequest.Name"/> and <see cref="LeaderboardRequest.Token"/> to be unique to that <see cref="LeaderboardRequest.GameId"/>.
		/// </summary>
		/// <param name="newLeaderboard"><see cref="LeaderboardRequest"/> object that holds the details of the new Leaderboard.</param>
		/// <returns>Returns a <see cref="LeaderboardResponse"/> object containing details for the newly created Leaderboard.</returns>
		public LeaderboardResponse Create(LeaderboardRequest newLeaderboard)
		{
			var query = GetUriBuilder("api/leaderboards/create").ToString();
			return Post<LeaderboardRequest, LeaderboardResponse>(query, newLeaderboard);
		}

		/// <summary>
		/// Get the standings for a Leaderboard using a <see cref="LeaderboardStandingsRequest"/>.
		/// </summary>
		/// <param name="leaderboardDetails"><see cref="LeaderboardStandingsRequest"/> object that holds the details that are wanted from the Leaderboard.</param>
		/// <returns>Returns multiple <see cref="LeaderboardStandingsResponse"/> that hold actor positions in the leaderboard.</returns>
		public IEnumerable<LeaderboardStandingsResponse> CreateGetLeaderboardStandings(LeaderboardStandingsRequest leaderboardDetails)
		{
			var query = GetUriBuilder("api/leaderboards/standings").ToString();
			return Post<LeaderboardStandingsRequest, IEnumerable<LeaderboardStandingsResponse>>(query, leaderboardDetails);
		}

		/// <summary>
		/// Update an existing Leaderboard.
		/// </summary>
		/// <param name="leaderboard"><see cref="LeaderboardRequest"/> object that holds the details of the Leaderboard.</param>
		public void Update(LeaderboardRequest leaderboard)
		{
			var query = GetUriBuilder("api/leaderboards/update").ToString();
			Put(query, leaderboard);
		}

		/// <summary>
		/// Delete a global leaderboard, ie. a leaderboard that is not associated with a specific game
		/// </summary>
		/// <param name="token">Token of Leaderboard</param>
		public void DeleteGlobal(string token)
		{
			var query = GetUriBuilder("api/leaderboards/{0}/global", token).ToString();
			Delete(query);
		}

        /// <summary>
        /// Delete Leaderboards with the <param name="gameId"/> and <param name="token"/> provided.
        /// </summary>
        /// <param name="token">Token of Leaderboard</param>
        /// <param name="gameId">ID of the Game the Leaderboard is for</param>
        public void Delete(string token, int gameId)
		{
			var query = GetUriBuilder("api/leaderboards/{0}/{1}", token, gameId).ToString();
			Delete(query);
		}
	}
}
