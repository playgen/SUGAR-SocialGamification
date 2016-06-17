using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.ClientAPI.Extensions;

namespace PlayGen.SGA.ClientAPI
{
    public class GroupAchievementClientProxy : ClientProxy, IGroupAchievementController
    {
        public int Create(Achievement achievement)
        {
            var query = GetUriBuilder("api/groupachievement").ToString();
            return Post<Achievement, int>(query, achievement);
        }

        public IEnumerable<Achievement> Get(int[] gameId)
        {
            var query = GetUriBuilder("api/groupachievement")
                .AppendQueryParameters(gameId, "gameId={0}")
                .ToString();
            return Get<IEnumerable<Achievement>>(query);
        }

        public void Delete(int[] achievementId)
        {
            var query = GetUriBuilder("api/groupachievement")
                .AppendQueryParameters(achievementId, "id={0}")
                .ToString();
            Delete(query);
        }


        // TODO: Do we need this stuff?
        public IEnumerable<AchievementProgress> GetProgress(int userId, int gameId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AchievementProgress> GetProgress(int achievementId, List<int> actorIds)
        {
            throw new NotImplementedException();
        }
    }
}
