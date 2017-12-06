using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class ActorRoleConfig : IEntityTypeConfiguration<ActorRole>
    {
		public void Configure(EntityTypeBuilder<ActorRole> builder)
		{
			builder.HasIndex(a => new { a.ActorId, a.EntityId, a.RoleId })
				.IsUnique();
		}
	}
}
