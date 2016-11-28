using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
    public class SentEvaluationNotificationController : DbController
    {
        public SentEvaluationNotificationController(SUGARContextFactory contextFactory) 
			: base(contextFactory)
		{
        }

        public SentEvaluationNotification Create(SentEvaluationNotification sentEvaluationNotification)
        {
            using (var context = ContextFactory.Create())
            {
                context.SentEvaluationNotifications.Add(sentEvaluationNotification);
                SaveChanges(context);

                return sentEvaluationNotification;
            }
        }

        public SentEvaluationNotification Get(int? gameId, int actorId, int evaluationId)
        {
            using (var context = ContextFactory.Create())
            {
                var sentEvaluationNotification = context.SentEvaluationNotifications
                    .Find(context, gameId, actorId, evaluationId);

                return sentEvaluationNotification;
            }
        }

        public SentEvaluationNotification Update(SentEvaluationNotification sentEvaluationNotification)
        {
            using (var context = ContextFactory.Create())
            {
                context.SentEvaluationNotifications.Update(sentEvaluationNotification);
                context.SaveChanges();

                return sentEvaluationNotification;
            }
        }

        public void Delete(int? gameId, int actorId, int evaluationId)
        {
            using (var context = ContextFactory.Create())
            {
                var sentEvaluationNotification = context.SentEvaluationNotifications
                    .Find(context, gameId, actorId, evaluationId);

                context.SentEvaluationNotifications.Remove(sentEvaluationNotification);
                context.SaveChanges();
            }
        }
    }
}
