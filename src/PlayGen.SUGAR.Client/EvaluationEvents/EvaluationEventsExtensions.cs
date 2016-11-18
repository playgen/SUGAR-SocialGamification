using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client.EvaluationEvents
{
    public static class EvaluationEventsExtensions
    {
        public static List<EvaluationNotification> ToNotifications(this List<EvaluationProgressResponse> progressResponses)
        {
            return progressResponses.Select(ToNotification).ToList();
        }

        public static EvaluationNotification ToNotification(this EvaluationProgressResponse progressResponse)
        {
            return new EvaluationNotification
            {
                Actor = progressResponse.Actor,
                Name = progressResponse.Name,
                Progress = progressResponse.Progress,
                Type = progressResponse.Type
            };
        }
    }
}
