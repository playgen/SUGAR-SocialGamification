using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;

namespace PlayGen.SUGAR.Client
{
	public class LeaderboardClient : ClientBase
	{
		public LeaderboardClient(string baseAddress, Credentials credentials) : base(baseAddress, credentials)
		{
		}
		
		/// <summary>
		/// Get all global leaderboards, ie. leaderboards that are not associated with a specific game
		/// 
		/// Example Usage: GET api/leaderboards/list
		/// </summary>
		/// <returns>Returns multiple <see cref="LeaderboardResponse"/> that hold Leaderboard details</returns>
		public IEnumerable<LeaderboardResponse> Get()
		{
			var query = GetUriBuilder("api/leaderboards/list").ToString();
			return Get<IEnumerable<LeaderboardResponse>>(query);
		}

		/// <summary>
		/// Find a list of leaderboards that match <param name="gameId"/>.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="LeaderboardResponse"/> that hold Leaderboard details</returns>
		public IEnumerable<LeaderboardResponse> Get(string gameId)
		{
			var query = GetUriBuilder($"api/leaderboards/game/{gameId}/list").ToString();
			return Get<IEnumerable<LeaderboardResponse>>(query);
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
		/// Get the standings for a Leaderboard using a <see cref="LeaderboardStandingRequest"/>.
		/// 
		/// Example Usage: POST api/leaderboards/standings
		/// </summary>
		/// <param name="leaderboardDetails"><see cref="LeaderboardStandingsRequest"/> object that holds the details that are wanted from the Leaderboard.</param>
		/// <returns>Returns multiple <see cref="LeaderboardStandingsResponse"/> that hold actor positions in the leaderboard.</returns>
		public IEnumerable<LeaderboardStandingsResponse> CreateGetLeaderboardStandings(LeaderboardStandingsRequest leaderboardDetails)
		{
			var query = GetUriBuilder("api/leaderboards/standings").ToString();
			return Post<LeaderboardStandingsRequest, IEnumerable<LeaderboardStandingsResponse>>(query, leaderboardDetails);
		}

		/// <summary>
		/// Delete Leaderboards with the <param name="id"/> provided.
		/// </summary>
		/// <param name="id">Leaderboard ID</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder($"api/leaderboards/{id}").ToString();
			Delete(query);
		}
	}
}
