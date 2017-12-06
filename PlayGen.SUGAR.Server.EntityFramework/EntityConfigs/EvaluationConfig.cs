using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class EvaluationConfig : IEntityTypeConfiguration<Evaluation>
    {
		public void Configure(EntityTypeBuilder<Evaluation> builder)
		{
			builder.ToTable("Evaluations");

			builder.HasIndex(e => new { e.Token, e.GameId, e.ActorType })
				.IsUnique();

			builder.HasDiscriminator<string>("Discriminator")
				.HasValue<Achievement>(EvaluationType.Achievement.ToString())
				.HasValue<Skill>(EvaluationType.Skill.ToString());
		}
	}
}
