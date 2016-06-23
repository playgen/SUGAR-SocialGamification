using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel;
using PlayGen.SGA.DataModel.Interfaces;
using MySql.Data.Entity;
using PlayGen.SGA.DataAccess.ExtensionMethods;

namespace PlayGen.SGA.DataAccess
{
    /// <summary>
    /// Entity Framework Database Configuration
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class SGAContext : DbContext
    {
        public SGAContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SGAContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SGAContext>());
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupAchievement> GroupAchievements { get; set; }
        public DbSet<GroupData> GroupDatas { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<UserData> UserDatas { get; set; }

        public DbSet<UserToUserRelationshipRequest> UserToUserRelationshipRequests { get; set; }
        public DbSet<UserToUserRelationship> UserToUserRelationships { get; set; }
        public DbSet<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }
        public DbSet<UserToGroupRelationship> UserToGroupRelationships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Setup foreign key relationships in the database tables
            modelBuilder.Entity<UserToUserRelationship>().HasRequired(u => u.Requestor).WithMany(u => u.Requestors).HasForeignKey(u => u.RequestorId);
            modelBuilder.Entity<UserToUserRelationship>().HasRequired(u => u.Acceptor).WithMany(u => u.Acceptors).HasForeignKey(u => u.AcceptorId);
            modelBuilder.Entity<UserToUserRelationshipRequest>().HasRequired(u => u.Requestor).WithMany(u => u.RequestRequestors).HasForeignKey(u => u.RequestorId);
            modelBuilder.Entity<UserToUserRelationshipRequest>().HasRequired(u => u.Acceptor).WithMany(u => u.RequestAcceptors).HasForeignKey(u => u.AcceptorId);

            // Setup unique fields
            modelBuilder.Entity<Game>().Property(g => g.Name).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Name).IsUnique();
            modelBuilder.Entity<Group>().Property(g => g.Name).IsUnique();
            modelBuilder.Entity<Account>().Property(a => a.Name).IsUnique();

            // Serialize specific objects as Json objects instead of creating a new table
            modelBuilder.ComplexType<AchievementCriteriaCollection>()
                .Property(p => p.Serialised)
                .HasColumnName("CompletionCriteria");
            modelBuilder.ComplexType<AchievementCriteriaCollection>().Property(a => a.Serialised).HasMaxLength(1024);

            // Change all string fields to have a max length of 64 chars
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(64));
        }

        public override int SaveChanges()
        {
            // User reflection to detect classes that implement the IModificationHistory interface
            // and set their DateCreated and DateModified DateTime fields accordingly.
            var histories = this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added ||
                                                                 e.State == EntityState.Modified))
                .Select(e => e.Entity as IModificationHistory);

            foreach (var history in histories)
            {
                history.DateModified = DateTime.Now;

                if (history.DateCreated == default(DateTime))
                {
                    history.DateCreated = DateTime.Now;;
                }
            }

            return base.SaveChanges();
        }
    }
}