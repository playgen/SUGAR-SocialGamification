using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class ActorClaimConfig : IEntityTypeConfiguration<ActorClaim>
    {
		public void Configure(EntityTypeBuilder<ActorClaim> builder)
		{
			builder.HasIndex(a => new { a.ActorId, a.EntityId, a.ClaimId })
				.IsUnique();
		}
	}
}
