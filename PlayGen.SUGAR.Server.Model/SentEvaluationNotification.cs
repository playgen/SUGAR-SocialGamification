namespace PlayGen.SUGAR.Server.Model
{
    public class SentEvaluationNotification
    {
        public int GameId { get; set; }

        public int ActorId { get; set; }

        public int EvaluationId { get; set; }

        public float Progress { get; set; }
    }
}
