using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Achievement specific operations.
	/// </summary>
	public class AchievementClient : ClientBase
	{
		public AchievementClient(string baseAddress, Credentials credentials) : base(baseAddress, credentials)
		{
		}

		/// <summary>
		/// Get all global achievements, ie. achievements that are not associated with a specific game
		/// </summary>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Achievement details</returns>
		public IEnumerable<AchievementResponse> Get()
		{
			var query = GetUriBuilder($"api/achievements/list").ToString();
			return Get<IEnumerable<AchievementResponse>>(query);
		}

		/// <summary>
		/// Find a list of Achievements that match <param name="gameId"/>.
		/// </summary>
		/// <param name="gameId">game ID</param>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Achievement details</returns>
		public IEnumerable<AchievementResponse> Get(int gameId)
		{
			var query = GetUriBuilder($"api/achievements/game/{gameId}/list").ToString();
			return Get<IEnumerable<AchievementResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for all achievements for a <param name="gameId"/> for <param name="actorId"/>.
		/// </summary>
		/// <param name="gameId">ID of Game</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold Achievement progress details</returns>
		public IEnumerable<AchievementProgressResponse> GetGameProgress(int gameId, int actorId)
		{
			var query = GetUriBuilder($"api/achievements/game/{gameId}/evaluate/{actorId}").ToString();
			return Get<IEnumerable<AchievementProgressResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for an Achievement for <param name="actorId"/>.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		/// <param name="actorId">ID of actor/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current progress toward achievement.</returns>
		public IEnumerable<AchievementProgressResponse> GetAchievementProgress(string token, int gameId, int actorId)
		{
			var query = GetUriBuilder($"api/achievements/{token}/{gameId}/evaluate/{actorId}").ToString();
			return Get<IEnumerable<AchievementProgressResponse>>(query);
		}

		/// <summary>
		/// Create a new Achievement.
		/// Requires <see cref="AchievementRequest.Name"/> to be unique to that <see cref="AchievementRequest.GameId"/>.
		/// </summary>
		/// <param name="newAchievement"><see cref="AchievementRequest"/> object that holds the details of the new Achievement.</param>
		/// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created Achievement.</returns>
		public AchievementResponse Create(AchievementRequest newAchievement)
		{
			var query = GetUriBuilder("api/achievements/create").ToString();
			return Post<AchievementRequest, AchievementResponse>(query, newAchievement);
		}

		/// <summary>
		/// Update an existing Achievement.
		/// </summary>
		/// <param name="achievement"><see cref="AchievementRequest"/> object that holds the details of the Achievement.</param>
		public void Update(int id, AchievementRequest achievement)
		{
			var query = GetUriBuilder($"api/achievements/update").ToString();
			Put(query, achievement);
		}

		/// <summary>
		/// Delete Achievement with the <param name="token"/> and <param name="gameId"/> provided.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		public void Delete(string token, int gameId)
		{
			var query = GetUriBuilder($"api/achievements/{token}/{gameId}").ToString();
			Delete(query);
		}
	}
}
