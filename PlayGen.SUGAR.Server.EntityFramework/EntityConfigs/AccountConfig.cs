using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
		public void Configure(EntityTypeBuilder<Account> builder)
		{
			builder.HasIndex(a => new { a.Name, a.AccountSourceId })
				.IsUnique();
		}
	}
}
