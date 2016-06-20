using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    public class UserAchievementClientProxy : ClientProxy, IUserAchievementController
    {
        public UserAchievementClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        public IEnumerable<AchievementResponse> Get(int[] gameId)
        {
            var query = GetUriBuilder("api/userachievement")
                .AppendQueryParameters(gameId, "gameId={0}")
                .ToString();
            return Get<IEnumerable<AchievementResponse>>(query);
        }

        public AchievementResponse Create(AchievementRequest achievement)
        {
            var query = GetUriBuilder("api/userachievement").ToString();
            return Post<AchievementRequest, AchievementResponse>(query, achievement);
        }

        public void Delete(int[] achievementId)
        {
            var query = GetUriBuilder("api/userachievement")
                .AppendQueryParameters(achievementId, "id={0}")
                .ToString();
            Delete(query);
        }


        // TODO: Need these?
        public IEnumerable<AchievementProgressResponse> GetProgress(int userId, int gameId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, List<int> actorIds)
        {
            throw new NotImplementedException();
        }
    }
}
