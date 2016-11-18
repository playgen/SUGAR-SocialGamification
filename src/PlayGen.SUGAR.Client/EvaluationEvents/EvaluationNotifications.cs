using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Client.EvaluationEvents
{
    public class EvaluationNotifications
    {
        private readonly List<EvaluationNotification> _pendingNotifications = new List<EvaluationNotification>();

        public bool TryDequeue(out EvaluationNotification evaluationNotification)
        {
            evaluationNotification = _pendingNotifications.FirstOrDefault();
            _pendingNotifications.Remove(evaluationNotification);
            return evaluationNotification != null;
        }

        public bool TryDequeue(EvaluationType type, out EvaluationNotification evaluationNotification)
        {
            evaluationNotification = _pendingNotifications.FirstOrDefault(p => p.Type == type);
            _pendingNotifications.Remove(evaluationNotification);
            return evaluationNotification != null;
        }

        public void Enqueue(List<EvaluationNotification> evaluationNotifications)
        {
            _pendingNotifications.InsertRange(0, evaluationNotifications);
        }
    }
}
