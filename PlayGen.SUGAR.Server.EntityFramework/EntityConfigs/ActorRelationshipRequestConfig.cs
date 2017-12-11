using System;
using System.Collections.Generic;
using System.Text;
using PlayGen.SUGAR.Server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class ActorRelationshipRequestConfig : IEntityTypeConfiguration<ActorRelationshipRequest>
    {
	    public void Configure(EntityTypeBuilder<ActorRelationshipRequest> builder)
	    {
		    builder.HasOne(u => u.Requestor)
			    .WithMany(u => u.RequestAcceptors)
			    .HasForeignKey(u => u.RequestorId)
			    .IsRequired();

		    builder.HasOne(u => u.Acceptor)
			    .WithMany(u => u.RequestRequestors)
			    .HasForeignKey(u => u.AcceptorId)
			    .IsRequired();

		    builder.HasKey(k => new { k.RequestorId, k.AcceptorId });
	    }
    }
}
