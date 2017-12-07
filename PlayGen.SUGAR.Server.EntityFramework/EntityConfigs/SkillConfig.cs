using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class SkillConfig : IEntityTypeConfiguration<Skill>
    {
		public void Configure(EntityTypeBuilder<Skill> builder)
		{
			builder.ToTable("Skills");

			builder.Property(p => p.Description)
				.HasMaxLength(256);
		}
	}
}
