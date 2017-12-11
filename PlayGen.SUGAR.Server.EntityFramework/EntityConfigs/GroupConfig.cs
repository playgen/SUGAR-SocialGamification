using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class GroupConfig : IEntityTypeConfiguration<Group>
    {
		public void Configure(EntityTypeBuilder<Group> builder)
		{
			builder.ToTable("Groups");

			builder.HasIndex(g => g.Name)
				.IsUnique();
		}
	}
}
