using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGroupAchievementController
    {
        AchievementResponse Create(AchievementRequest achievement);

        IEnumerable<AchievementResponse> Get(int[] gameId);

        void Delete(int[] achievementId);

        IEnumerable<AchievementProgressResponse> GetProgress(int userId, int gameId);

        IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, List<int> actorIds);
    }
}