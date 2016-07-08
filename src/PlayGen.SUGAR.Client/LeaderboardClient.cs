using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Leaderboard specific operations.
	/// </summary>
	public class LeaderboardClient : ClientBase
	{
		public LeaderboardClient(string baseAddress, Credentials credentials) : base(baseAddress, credentials)
		{
		}
		
		/// <summary>
		/// Get all global leaderboards, ie. leaderboards that are not associated with a specific game
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
		public IEnumerable<LeaderboardResponse> Get(int gameId)
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
			var query = GetUriBuilder($"api/leaderboards/update").ToString();
			Put(query, leaderboard);
		}

		/// <summary>
		/// Delete Leaderboards with the <param name="id"/> provided.
		/// </summary>
		/// <param name="token">Token of Leaderboard</param>
		/// <param name="gameId">ID of the Game the Leaderboard is for</param>
		public void Delete(string token, int gameId)
		{
			var query = GetUriBuilder($"api/leaderboards/{token}/{gameId}").ToString();
			Delete(query);
		}
	}
}
