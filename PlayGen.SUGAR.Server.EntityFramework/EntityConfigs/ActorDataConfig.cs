using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class ActorDataConfig : IEntityTypeConfiguration<ActorData>
    {
		public void Configure(EntityTypeBuilder<ActorData> builder)
		{
			builder.HasIndex(g => new { g.Key, g.GameId, g.ActorId, DataType = g.EvaluationDataType })
				.IsUnique();
		}
	}
}
