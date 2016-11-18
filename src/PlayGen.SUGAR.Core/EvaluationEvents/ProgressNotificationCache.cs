using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Check newly evaluated progress and store notifications for values of any significance
    /// </summary>
    public class ProgressNotificationCache
    {
        // <gameId, <actorId, <evaluation, progress>>>
        public void Check(Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> progress)
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

        // <actorId, <evaluation, progress>>
        public Dictionary<int, Dictionary<Evaluation, float>> GetNotifications(int gameId, int actorId)
        {
            throw new NotImplementedException();
        }
    }
}
