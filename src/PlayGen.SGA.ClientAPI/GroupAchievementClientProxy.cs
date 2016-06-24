using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.ClientAPI.Extensions;

namespace PlayGen.SGA.ClientAPI
{
    /// <summary>
    /// Controller that facilitates GroupAchievement specific operations.
    /// </summary>
    public class GroupAchievementClientProxy : ClientProxyBase, IGroupAchievementController
    {
        public GroupAchievementClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Get a list of GroupAchievements that match <param name="gameId"/>.
        /// </summary>
        /// <param name="gameId">Array of game IDs</param>
        /// <returns>Returns multiple <see cref="GameResponse"/> that hold GroupAchievement details</returns>
        public IEnumerable<AchievementResponse> Get(int[] gameId)
        {
            var query = GetUriBuilder("api/groupachievement")
                .AppendQueryParameters(gameId, "gameId={0}")
                .ToString();
            return Get<IEnumerable<AchievementResponse>>(query);
        }

        public IEnumerable<AchievementProgressResponse> GetProgress(int groupId, int gameId)
        {
            var query = GetUriBuilder("api/groupachievement/gameprogress")
                .AppendQueryParameters(new int[] { groupId }, "groupId={0}")
                .AppendQueryParameters(new int[] { gameId }, "gameId={0}")
                .ToString();
            return Get<IEnumerable<AchievementProgressResponse>>(query);
        }

        public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, int[] groupId)
        {
            var query = GetUriBuilder("api/groupachievement/progress")
                .AppendQueryParameters(new int[] { achievementId }, "achievementId={0}")
                .AppendQueryParameters(groupId, "groupId={0}")
                .ToString();
            return Get<IEnumerable<AchievementProgressResponse>>(query);
        }

        /// <summary>
        /// Create a new GroupAchievement.
        /// Requires <see cref="newAchievement.Name"/> to be unique to that <see cref="newAchievement.GameId"/>.
        /// </summary>
        /// <param name="achievement"><see cref="AchievementRequest"/> object that holds the details of the new GroupAchievement.</param>
        /// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created GroupAchievement.</returns>
        public AchievementResponse Create(AchievementRequest achievement)
        {
            var query = GetUriBuilder("api/groupachievement").ToString();
            return Post<AchievementRequest, AchievementResponse>(query, achievement);
        }

        /// <summary>
        /// Delete GroupAchievements with the <param name="achievementId"/> provided.
        /// </summary>
        /// <param name="achievementId">Array of GroupAchievement IDs</param>
        public void Delete(int[] achievementId)
        {
            var query = GetUriBuilder("api/groupachievement")
                .AppendQueryParameters(achievementId, "id={0}")
                .ToString();
            Delete(query);
        }
    }
}
