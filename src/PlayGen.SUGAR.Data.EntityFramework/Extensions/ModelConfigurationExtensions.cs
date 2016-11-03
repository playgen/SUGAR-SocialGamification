using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework
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
            
            // Need to be abbreviated otherwise foreign key constraint names are too long.
            builder.Entity<Achievement>()
                .ToTable("Achvmnt");

            builder.Entity<CompletionCriteria>()
                .ToTable("CompCri");
        }

        internal static void ConfigureForeignKeys(this ModelBuilder builder)
        {
            // Setup foreign key relationships in the database tables
            builder.Entity<UserToUserRelationship>()
                .HasOne(u => u.Requestor)
                //.HasRequired(u => u.Requestor)
                .WithMany(u => u.Requestors)
                .HasForeignKey(u => u.RequestorId);

            builder.Entity<UserToUserRelationship>()
                .HasOne(u => u.Acceptor)
                //.HasRequired(u => u.Requestor)
                .WithMany(u => u.Acceptors)
                .HasForeignKey(u => u.AcceptorId);

            builder.Entity<UserToUserRelationshipRequest>()
                .HasOne(u => u.Requestor)
                //.HasRequired(u => u.Requestor)
                .WithMany(u => u.RequestRequestors)
                .HasForeignKey(u => u.RequestorId);

            builder.Entity<UserToUserRelationshipRequest>()
                .HasOne(u => u.Acceptor)
                //.HasRequired(u => u.Requestor)
                .WithMany(u => u.RequestAcceptors)
                .HasForeignKey(u => u.AcceptorId);
        }

        internal static void ConfigureCompositePrimaryKeys(this ModelBuilder builder)
        {
            builder.Entity<Achievement>()
                .HasKey(a => new { a.Token, a.GameId });

            builder.Entity<Skill>()
                .HasKey(a => new { a.Token, a.GameId });

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
        }

        internal static void ConfigureIndexes(this ModelBuilder builder)
        {
            // Multiple columns
            builder.Entity<GameData>()
                .HasIndex(g => new { g.Key, g.GameId, g.Category, g.ActorId, g.DataType });
        
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
                .HasIndex(a => a.Name)
                .IsUnique();
        }

        internal static void ConfigureHierarchy(this ModelBuilder builder)
        {
            builder.Entity<Actor>()
               .HasDiscriminator<string>("discriminator")
               .HasValue<Group>("group")
               .HasValue<User>("user");
        }

        internal static void ConfigureProperties(this ModelBuilder builder)
        {
            // Change all string fields to have a max length of 64 chars
            // todo find api to achieve below:
            //modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(64));

            builder.Entity<Achievement>()
                .Property(p => p.Description)
                .HasMaxLength(256);

            // Set precision of data
            //modelBuilder.Entity<GameData>()
            //	.Property(g => g.DateCreated)
            //	.HasPrecision(3);
            //modelBuilder.Entity<GameData>()
            //	.Property(g => g.DateModified)
            //	.HasPrecision(3);
        }
    }
}
