namespace PlayGen.SGA.Contracts
{
    public class AchievementAdminResponse
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public string Name { get; set; }

        public string CompletionCriteria { get; set; }
    }
}
