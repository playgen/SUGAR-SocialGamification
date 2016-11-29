using PlayGen.SUGAR.Client.EvaluationEvents;

namespace PlayGen.SUGAR.Client
{
    /// <summary>
    /// Controller that facilitates Session specific operations.
    /// </summary>
    public class SessionClient : ClientBase
    {
        private const string ControllerPrefix = "api/session";

        public SessionClient(string baseAddress, IHttpHandler httpHandler, EvaluationNotifications evaluationNotifications)
            : base(baseAddress, httpHandler, evaluationNotifications)
        {
        }

        
        // todo login
        // todo logout


        public void Heartbeat()
        {
            var query = GetUriBuilder(ControllerPrefix + "/heartbeat").ToString();
            Get(query);
        }
    }
}
