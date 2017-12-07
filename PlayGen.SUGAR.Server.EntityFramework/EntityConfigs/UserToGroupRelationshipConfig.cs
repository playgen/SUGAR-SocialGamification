using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class UserToGroupRelationshipConfig : IEntityTypeConfiguration<UserToGroupRelationship>
    {
		public void Configure(EntityTypeBuilder<UserToGroupRelationship> builder)
		{
			builder.HasOne(u => u.Requestor)
				.WithMany(u => u.UserToGroupRelationships)
				.HasForeignKey(u => u.RequestorId)
				.IsRequired();

			builder.HasOne(u => u.Acceptor)
				.WithMany(u => u.UserToGroupRelationships)
				.HasForeignKey(u => u.AcceptorId)
				.IsRequired();

			builder.HasKey(k => new { k.RequestorId, k.AcceptorId });
		}
	}
}
