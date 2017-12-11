using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class EvaluationDataConfig : IEntityTypeConfiguration<EvaluationData>
    {
		public void Configure(EntityTypeBuilder<EvaluationData> builder)
		{
			builder.HasIndex(g => new { g.Key, g.GameId, g.Category, CreatingActorId = g.ActorId, g.EvaluationDataType })
				.HasName("IX_EvaluationData_Key_Game_Category_Creator_Type");

			builder.HasIndex(g => new { g.GameId, g.Category, CreatingActorId = g.ActorId, g.EvaluationDataType })
				.HasName("IX_EvaluationData_Game_Category_Creator_Type");

			builder.HasIndex(g => new { g.GameId, g.Category, g.MatchId, g.EvaluationDataType })
				.HasName("IX_EvaluationData_Game_Category_Match_Type");
		}
	}
}
