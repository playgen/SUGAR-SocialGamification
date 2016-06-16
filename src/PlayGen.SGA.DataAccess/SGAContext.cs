using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel;
using PlayGen.SGA.DataModel.Interfaces;
using MySql.Data.Entity;

namespace PlayGen.SGA.DataAccess
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class SGAContext : DbContext
    {
        public SGAContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<SGAContext>());
            Database.SetInitializer(new CreateDatabaseIfNotExists<SGAContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SGAContext>());
        }

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
            modelBuilder.Entity<UserToUserRelationship>().HasRequired(u => u.Requestor).WithMany(u => u.Requestors).HasForeignKey(u => u.RequestorId).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserToUserRelationship>().HasRequired(u => u.Acceptor).WithMany(u => u.Acceptors).HasForeignKey(u => u.AcceptorId).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserToUserRelationshipRequest>().HasRequired(u => u.Requestor).WithMany(u => u.RequestRequestors).HasForeignKey(u => u.RequestorId).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserToUserRelationshipRequest>().HasRequired(u => u.Acceptor).WithMany(u => u.RequestAcceptors).HasForeignKey(u => u.AcceptorId).WillCascadeOnDelete(false);
        }

        /*
        public override int SaveChanges()
        {
            foreach(var history in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added ||
                    e.State == EntityState.Modified))
                .Select())
        }*/
    }
}