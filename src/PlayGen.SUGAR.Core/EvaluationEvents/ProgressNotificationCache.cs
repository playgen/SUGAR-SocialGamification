using System;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Check newly evaluated progress and store notifications for values of any significance
    /// </summary>
    public class ProgressNotificationCache
    {
        // <gameId, <actorId, <evaluationId, progress>>>
        public void Check(Dictionary<int, Dictionary<int, Dictionary<int, float>>> progress)
        {
            throw new NotImplementedException();
        }

        public void Remove(int gameId, int actorId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int evaluationId)
        {
            throw new NotImplementedException();
        }

        // <actorId, <evaluationId, progress>>
        public Dictionary<int, Dictionary<int, float>> GetNotifications(int gameId, int actorId)
        {
            throw new NotImplementedException();
        }
    }
}
