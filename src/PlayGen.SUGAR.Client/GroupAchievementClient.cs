using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates GroupAchievement specific operations.
	/// </summary>
	public class GroupAchievementClient : ClientBase
	{
		public GroupAchievementClient(string baseAddress) : base(baseAddress)
		{
		}

		/// <summary>
		/// Get a list of GroupAchievements that match <param name="gameId"/>.
		/// </summary>
		/// <param name="gameId">Array of game IDs</param>
		/// <returns>Returns multiple <see cref="GameResponse"/> that hold GroupAchievement details</returns>
		public IEnumerable<AchievementResponse> Get(int gameId)
		{
			var query = GetUriBuilder($"api/groupachievement/{gameId}").ToString();
			return Get<IEnumerable<AchievementResponse>>(query);
		}

		public IEnumerable<AchievementProgressResponse> GetProgress(int groupId, int gameId)
		{
			var query = GetUriBuilder($"api/groupachievement/gameprogress/{groupId}/{gameId}").ToString();
			return Get<IEnumerable<AchievementProgressResponse>>(query);
		}

		public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, int[] groupId)
		{
			var query = GetUriBuilder("api/groupachievement/progress")
				.AppendQueryParameter(achievementId, "achievementId={0}")
				.AppendQueryParameters(groupId, "groupId={0}")
				.ToString();
			return Get<IEnumerable<AchievementProgressResponse>>(query);
		}

		/// <summary>
		/// Create a new GroupAchievement.
		/// Requires <see cref="achievement.Name"/> to be unique to that <see cref="GameId"/>.
		/// </summary>
		/// <param name="achievement"><see cref="AchievementRequest"/> object that holds the details of the new GroupAchievement.</param>
		/// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created GroupAchievement.</returns>
		public AchievementResponse Create(AchievementRequest achievement)
		{
			var query = GetUriBuilder("api/groupachievement").ToString();
			return Post<AchievementRequest, AchievementResponse>(query, achievement);
		}

		/// <summary>
		/// Delete GroupAchievement with the <param name="achievementId"/> provided.
		/// </summary>
		/// <param name="achievementId">GroupAchievement ID</param>
		public void Delete(int achievementId)
		{
			var query = GetUriBuilder($"api/groupachievement/{achievementId}").ToString();
			Delete(query);
		}
	}
}
