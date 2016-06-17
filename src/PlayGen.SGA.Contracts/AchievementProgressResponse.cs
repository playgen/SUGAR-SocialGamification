namespace PlayGen.SGA.Contracts
{
    public class AchievementProgressResponse
    {
        public ActorResponse Actor { get; set; }

        public string Name { get; set; }

        public float Progress { get; set; }
    }
}
