using System.Collections.Generic;
using System.Linq;

namespace PlayGen.SUGAR.Client.EvaluationEvents
{
    public class EvaluationNotifications
    {
        private readonly Queue<EvaluationNotification> _pendingNotifications = new Queue<EvaluationNotification>();

        public bool TryDequeue(out EvaluationNotification evaluationNotification)
        {
            var didDequeue = false;
            evaluationNotification = null;

            if (_pendingNotifications.Any())
            {
                _pendingNotifications.Dequeue();
                didDequeue = true;
            }

            return didDequeue;
        }

        public void Enqueue(List<EvaluationNotification> evaluationNotifications)
        {
            evaluationNotifications.ForEach(e => _pendingNotifications.Enqueue(e));
        }
    }
}
