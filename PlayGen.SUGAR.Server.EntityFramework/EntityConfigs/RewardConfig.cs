using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class RewardConfig : IEntityTypeConfiguration<Reward>
    {
		public void Configure(EntityTypeBuilder<Reward> builder)
		{
			builder.ToTable("Rewards");
		}
	}
}
