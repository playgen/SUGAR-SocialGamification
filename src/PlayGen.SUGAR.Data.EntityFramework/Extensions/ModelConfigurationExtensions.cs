using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;
using EvaluationCriteria = PlayGen.SUGAR.Data.Model.EvaluationCriteria;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
    internal static class ModelConfigurationExtensions
    {
        internal static void ConfigureTableNames(this ModelBuilder builder)
        {
            // Table names
            builder.Entity<User>()
                .ToTable("Users");

            builder.Entity<Group>()
                .ToTable("Groups");

            builder.Entity<Achievement>()
                .ToTable("Achievements");

            builder.Entity<Skill>()
                .ToTable("Skills");

            builder.Entity<Evaluation>()
                .ToTable("Evaluations");

            builder.Entity<EvaluationCriteria>()
                .ToTable("EvaluationCriterias");

            builder.Entity<Model.Reward>()
                .ToTable("Rewards");

            builder.Entity<SentEvaluationNotification>()
                .ToTable("SentEvaluationNotifications");
        }

        internal static void ConfigureForeignKeys(this ModelBuilder builder)
        {
            // Setup foreign key relationships in the database tables
            builder.Entity<UserToUserRelationship>()
                .HasOne(u => u.Requestor)
                .WithMany(u => u.Requestors)
                .HasForeignKey(u => u.RequestorId)
                .IsRequired();

            builder.Entity<UserToUserRelationship>()
                .HasOne(u => u.Acceptor)
                .WithMany(u => u.Acceptors)
                .HasForeignKey(u => u.AcceptorId)
                .IsRequired();

            builder.Entity<UserToUserRelationshipRequest>()
                .HasOne(u => u.Requestor)
                .WithMany(u => u.RequestRequestors)
                .HasForeignKey(u => u.RequestorId)
                .IsRequired();

            builder.Entity<UserToUserRelationshipRequest>()
                .HasOne(u => u.Acceptor)
                .WithMany(u => u.RequestAcceptors)
                .HasForeignKey(u => u.AcceptorId)
                .IsRequired();
        }

        internal static void ConfigureCompositePrimaryKeys(this ModelBuilder builder)
        {
            builder.Entity<Leaderboard>()
                .HasKey(a => new { a.Token, a.GameId });

            builder.Entity<UserToUserRelationshipRequest>()
                .HasKey(k => new { k.RequestorId, k.AcceptorId });

            builder.Entity<UserToUserRelationship>()
                .HasKey(k => new { k.RequestorId, k.AcceptorId });

            builder.Entity<UserToGroupRelationshipRequest>()
                .HasKey(k => new { k.RequestorId, k.AcceptorId });

            builder.Entity<UserToGroupRelationship>()
                .HasKey(k => new { k.RequestorId, k.AcceptorId });

            builder.Entity<RoleClaim>()
                .HasKey(k => new { k.RoleId, k.ClaimId });

            builder.Entity<SentEvaluationNotification>()
                .HasKey(k => new {k.GameId, k.ActorId, k.EvaluationId});
        }

        internal static void ConfigureIndexes(this ModelBuilder builder)
        {
            // Multiple columns
            builder.Entity<GameData>()
                .HasIndex(g => new { g.Key, g.GameId, g.Category, g.ActorId, DataType = g.SaveDataType });

			builder.Entity<ActorData>()
				.HasIndex(g => new { g.Key, g.GameId, g.ActorId, DataType = g.SaveDataType })
				.IsUnique();

			builder.Entity<Evaluation>()
                .HasIndex(e => new { e.Token, e.GameId, e.ActorType });

            builder.Entity<ActorRole>()
                .HasIndex(a => new { a.ActorId, a.EntityId, a.RoleId })
                .IsUnique();

			builder.Entity<ActorClaim>()
				.HasIndex(a => new { a.ActorId, a.EntityId, a.ClaimId })
				.IsUnique();

			// Unique
			builder.Entity<Game>()
                .HasIndex(g => g.Name)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            builder.Entity<Group>()
                .HasIndex(g => g.Name)
                .IsUnique();

            builder.Entity<Account>()
                .HasIndex(a => new { a.Name, a.AccountSourceId})
                .IsUnique();

            builder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();
        }

        internal static void ConfigureHierarchy(this ModelBuilder builder)
        {
            builder.Entity<Actor>()
               .HasDiscriminator<ActorType>("Discriminator")
               .HasValue<Group>(ActorType.Group)
               .HasValue<User>(ActorType.User);

            builder.Entity<Evaluation>()
               .HasDiscriminator<EvaluationType>("Discriminator")
               .HasValue<Achievement>(EvaluationType.Achievement)
               .HasValue<Skill>(EvaluationType.Skill);
		}

        internal static void ConfigureProperties(this ModelBuilder builder)
        {
            // Change all string fields to have a max length of 64 chars
            // todo find api to achieve below:
            //modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(64));

            builder.Entity<Achievement>()
                .Property(p => p.Description)
                .HasMaxLength(256);

            builder.Entity<Skill>()
                .Property(p => p.Description)
                .HasMaxLength(256);

            // Set precision of data
            //modelBuilder.Entity<GameData>()
            //	.Property(g => g.DateCreated)
            //	.HasPrecision(3);
            //modelBuilder.Entity<GameData>()
            //	.Property(g => g.DateModified)
            //	.HasPrecision(3);
            //modelBuilder.Entity<ActorData>()
            //    .Property(g => g.DateCreated)
            //    .HasPrecision(3);
            //modelBuilder.Entity<ActorData>()
            //    .Property(g => g.DateModified)
            //    .HasPrecision(3);
        }
    }
}
