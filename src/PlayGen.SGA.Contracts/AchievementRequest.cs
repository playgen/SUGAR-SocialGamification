namespace PlayGen.SGA.Contracts
{
    public class AchievementRequest
    {
        public int GameId { get; set; }

        public string Name { get; set; }

        public string CompletionCriteria { get; set; }
    }
}
