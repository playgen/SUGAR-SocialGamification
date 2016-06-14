using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataAccess
{
    [DbConfigurationType((typeof(CodeConfig)))]
    public class SGAContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupAchievement> GroupAchievements { get; set; }
        public DbSet<GroupData> GroupDatas { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<UserData> UserData { get; set; }
        public DbSet<UserToUserRelationshipRequest> UserToUserRelationshipRequests { get; set; }
        public DbSet<UserToUserRelationship> UserToUserRelationships { get; set; }
        public DbSet<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }
        public DbSet<UserToGroupRelationship> UserToGroupRelationship { get; set; }

        public SGAContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SGAContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SGAContext>());
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