using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    /// <summary>
    /// Controller that facilitates UserAchievement specific operations.
    /// </summary>
    public class UserAchievementClientProxy : ClientProxyBase, IUserAchievementController
    {
        public UserAchievementClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Get a list of UserAchievements that match <param name="gameId"/>.
        /// </summary>
        /// <param name="gameId">Array of game IDs</param>
        /// <returns>Returns multiple <see cref="GameResponse"/> that hold UserAchievement details</returns>
        public IEnumerable<AchievementResponse> Get(int[] gameId)
        {
            var query = GetUriBuilder("api/userachievement")
                .AppendQueryParameters(gameId, "gameId={0}")
                .ToString();
            return Get<IEnumerable<AchievementResponse>>(query);
        }


        public IEnumerable<AchievementProgressResponse> GetProgress(int userId, int gameId)
        {
            var query = GetUriBuilder("api/userachievement/gameprogress")
                .AppendQueryParameters(new int[] { userId }, "userId={0}")
                .AppendQueryParameters(new int[] { gameId }, "gameId={0}")
                .ToString();
            return Get<IEnumerable<AchievementProgressResponse>>(query);
        }

        public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, int[] actorIds)
        {
            var query = GetUriBuilder("api/userachievement/progress")
                .AppendQueryParameters(new int[] { achievementId }, "achievementId={0}")
                .AppendQueryParameters(actorIds, "userId={0}")
                .ToString();
            return Get<IEnumerable<AchievementProgressResponse>>(query);
        }

        /// <summary>
        /// Create a new UserAchievement.
        /// Requires <see cref="newAchievement.Name"/> to be unique to that <see cref="newAchievement.GameId"/>.
        /// </summary>
        /// <param name="achievement"><see cref="AchievementRequest"/> object that holds the details of the new UserAchievement.</param>
        /// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created UserAchievement.</returns>
        public AchievementResponse Create(AchievementRequest achievement)
        {
            var query = GetUriBuilder("api/userachievement").ToString();
            return Post<AchievementRequest, AchievementResponse>(query, achievement);
        }

        /// <summary>
        /// Delete UserAchievements with the <param name="achievementId"/> provided.
        /// </summary>
        /// <param name="achievementId">Array of UserAchievement IDs</param>
        public void Delete(int[] achievementId)
        {
            var query = GetUriBuilder("api/userachievement")
                .AppendQueryParameters(achievementId, "id={0}")
                .ToString();
            Delete(query);
        }
    }
}
