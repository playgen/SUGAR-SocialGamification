using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class UserToUserRelationshipRequestConfig : IEntityTypeConfiguration<UserToUserRelationshipRequest>
    {
		public void Configure(EntityTypeBuilder<UserToUserRelationshipRequest> builder)
		{
			builder.HasOne(u => u.Requestor)
				.WithMany(u => u.RequestRequestors)
				.HasForeignKey(u => u.RequestorId)
				.IsRequired();

			builder.HasOne(u => u.Acceptor)
				.WithMany(u => u.RequestAcceptors)
				.HasForeignKey(u => u.AcceptorId)
				.IsRequired();

			builder.HasKey(k => new { k.RequestorId, k.AcceptorId });
		}
	}
}
