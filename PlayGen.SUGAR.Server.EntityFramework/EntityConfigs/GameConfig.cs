using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class GameConfig : IEntityTypeConfiguration<Game>
    {
		public void Configure(EntityTypeBuilder<Game> builder)
		{
			builder.HasIndex(g => g.Name)
				.IsUnique();
		}
	}
}
