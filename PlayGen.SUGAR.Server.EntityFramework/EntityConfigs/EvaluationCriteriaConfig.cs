using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class EvaluationCriteriaConfig : IEntityTypeConfiguration<EvaluationCriteria>
    {
		public void Configure(EntityTypeBuilder<EvaluationCriteria> builder)
		{
			builder.ToTable("EvaluationCriterias");
		}
	}
}
