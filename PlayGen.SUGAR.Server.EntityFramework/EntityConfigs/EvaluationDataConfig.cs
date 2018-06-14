using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class EvaluationDataConfig : IEntityTypeConfiguration<EvaluationData>
    {
		public void Configure(EntityTypeBuilder<EvaluationData> builder)
		{
			builder.HasIndex(g => g.Category);

			builder.HasIndex(g => g.ActorId);

			builder.HasIndex(g => g.Key);
        }
	}
}
