using System.Collections.Generic;

namespace PlayGen.SGA.Contracts
{
    public class AchievementRequest
    {
        public int GameId { get; set; }

        public string Name { get; set; }

        public List<AchievementCriteria> CompletionCriteria { get; set; }
    }
}
