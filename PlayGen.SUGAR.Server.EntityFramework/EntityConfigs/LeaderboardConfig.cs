using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.EntityConfigs
{
    public class LeaderboardConfig : IEntityTypeConfiguration<Leaderboard>
    {
		public void Configure(EntityTypeBuilder<Leaderboard> builder)
		{
			builder.HasKey(a => new { a.Token, a.GameId });
		}
	}
}
