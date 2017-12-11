using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class ActorRelationshipConfig : IEntityTypeConfiguration<ActorRelationship>
    {
	    public void Configure(EntityTypeBuilder<ActorRelationship> builder)
	    {
		    builder.HasOne(u => u.Requestor)
			    .WithMany(u => u.Requestors)
			    .HasForeignKey(u => u.RequestorId)
			    .IsRequired();

		    builder.HasOne(u => u.Acceptor)
			    .WithMany(u => u.Acceptors)
			    .HasForeignKey(u => u.AcceptorId)
			    .IsRequired();

		    builder.HasKey(k => new { k.RequestorId, k.AcceptorId });
	    }
    }
}
