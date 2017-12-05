using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class RoleClaimConfig : IEntityTypeConfiguration<RoleClaim>
    {
		public void Configure(EntityTypeBuilder<RoleClaim> builder)
		{
			builder.HasKey(k => new { k.RoleId, k.ClaimId });
		}
	}
}
