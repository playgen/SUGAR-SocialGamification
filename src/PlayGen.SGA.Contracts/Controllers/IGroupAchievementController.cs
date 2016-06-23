using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGroupAchievementController
    {
        IEnumerable<AchievementResponse> Get(int[] gameId);

        AchievementResponse Create(AchievementRequest achievement);

        void Delete(int[] achievementId);

        IEnumerable<AchievementProgressResponse> GetProgress(int userId, int gameId);

        IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, int[] actorIds);
    }
}