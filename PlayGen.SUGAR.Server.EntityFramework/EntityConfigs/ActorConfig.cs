using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class ActorConfig : IEntityTypeConfiguration<Actor>
    {
		public void Configure(EntityTypeBuilder<Actor> builder)
		{
			builder.HasDiscriminator<string>("Discriminator")
				.HasValue<Group>(ActorType.Group.ToString())
				.HasValue<User>(ActorType.User.ToString());
		}
	}
}
