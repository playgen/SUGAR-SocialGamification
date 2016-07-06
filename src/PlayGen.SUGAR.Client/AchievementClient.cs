using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;


namespace PlayGen.SUGAR.Client
{
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
		/// Find the current progress for an <param name="achievementId"/> for <param name="actor"/>.
		/// </summary>
		/// <param name="achievementId">ID of Achievement</param>
		/// <param name="actorId">ID of actor/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current group progress toward achievement.</returns>
		public IEnumerable<AchievementProgressResponse> GetAchievementProgress(int achievementId, int actorId)
		{
			var query = GetUriBuilder($"api/achievements/{achievementId}/evaluate/{actorId}").ToString();
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
		/// Delete Achievements with the <param name="id"/> provided.
		/// </summary>
		/// <param name="id">Achievement ID</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder($"api/achievements/{id}").ToString();
			Delete(query);
		}
	}
}
