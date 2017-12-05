using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class AchievementConfig : IEntityTypeConfiguration<Achievement>
    {
		public void Configure(EntityTypeBuilder<Achievement> builder)
		{
			builder.ToTable("Achievements");

			builder.Property(p => p.Description)
				.HasMaxLength(256);
		}
	}
}
