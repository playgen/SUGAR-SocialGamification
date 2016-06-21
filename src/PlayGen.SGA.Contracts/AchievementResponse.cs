using System.Collections.Generic;

namespace PlayGen.SGA.Contracts
{
    public class AchievementResponse
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public string Name { get; set; }

        public List<DatabaseQueryParams> CompletionCriteria { get; set; }
    }
}
