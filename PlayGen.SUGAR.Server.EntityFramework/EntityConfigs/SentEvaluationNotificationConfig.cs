using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class SentEvaluationNotificationConfig : IEntityTypeConfiguration<SentEvaluationNotification>
    {
		public void Configure(EntityTypeBuilder<SentEvaluationNotification> builder)
		{
			builder.ToTable("SentEvaluationNotifications");

			builder.HasKey(k => new { k.GameId, k.ActorId, k.EvaluationId });
		}
	}
}
