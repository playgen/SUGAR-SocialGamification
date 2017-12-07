using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class UserToGroupRelationshipRequestConfig : IEntityTypeConfiguration<UserToGroupRelationshipRequest>
    {
		public void Configure(EntityTypeBuilder<UserToGroupRelationshipRequest> builder)
		{
			builder.HasOne(u => u.Requestor)
				.WithMany(u => u.UserToGroupRelationshipRequests)
				.HasForeignKey(u => u.RequestorId)
				.IsRequired();

			builder.HasOne(u => u.Acceptor)
				.WithMany(u => u.UserToGroupRelationshipRequests)
				.HasForeignKey(u => u.AcceptorId)
				.IsRequired();

			builder.HasKey(k => new { k.RequestorId, k.AcceptorId });
		}
	}
}
