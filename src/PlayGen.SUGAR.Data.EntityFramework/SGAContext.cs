using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.Entity;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.Model.Interfaces;
using PlayGen.SUGAR.Data.EntityFramework.ExtensionMethods;

namespace PlayGen.SUGAR.Data.EntityFramework
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
		public DbSet<GroupData> GroupData { get; set; }

		public DbSet<User> Users { get; set; }
		public DbSet<UserAchievement> UserAchievements { get; set; }
		public DbSet<UserData> UserData { get; set; }

		public DbSet<UserToUserRelationshipRequest> UserToUserRelationshipRequests { get; set; }
		public DbSet<UserToUserRelationship> UserToUserRelationships { get; set; }
		public DbSet<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }
		public DbSet<UserToGroupRelationship> UserToGroupRelationships { get; set; }

		public IQueryable<GameData> GetGameData<TData>()
			where TData : GameData
		{
			var dataType = typeof(TData);

			if (dataType == typeof(UserData))
			{
				return UserData.Cast<GameData>();
			}
			else if (dataType == typeof(GroupData))
			{
				return GroupData.Cast<GameData>();
			}
			throw new NotImplementedException($"Type {typeof(TData)} not supported.");
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// Setup foreign key relationships in the database tables
			modelBuilder.Entity<UserToUserRelationship>()
				.HasRequired(u => u.Requestor)
				.WithMany(u => u.Requestors)
				.HasForeignKey(u => u.RequestorId);
			modelBuilder.Entity<UserToUserRelationship>()
				.HasRequired(u => u.Acceptor)
				.WithMany(u => u.Acceptors)
				.HasForeignKey(u => u.AcceptorId);
			modelBuilder.Entity<UserToUserRelationshipRequest>()
				.HasRequired(u => u.Requestor)
				.WithMany(u => u.RequestRequestors)
				.HasForeignKey(u => u.RequestorId);
			modelBuilder.Entity<UserToUserRelationshipRequest>()
				.HasRequired(u => u.Acceptor)
				.WithMany(u => u.RequestAcceptors)
				.HasForeignKey(u => u.AcceptorId);

			// Setup unique fields
			modelBuilder.Entity<Game>()
				.Property(g => g.Name)
				.IsUnique();
			modelBuilder.Entity<User>()
				.Property(u => u.Name)
				.IsUnique();
			modelBuilder.Entity<Group>()
				.Property(g => g.Name)
				.IsUnique();
			modelBuilder.Entity<Account>()
				.Property(a => a.Name)
				.IsUnique();

			modelBuilder.Entity<GroupData>()
				.Property(gd => gd.GameId)
				.IsIndexed("IX_GroupData_Game_Group_Key");
			modelBuilder.Entity<GroupData>()
				.Property(gd => gd.GroupId)
				.IsIndexed("IX_GroupData_Game_Group_Key");
			modelBuilder.Entity<GroupData>()
				.Property(gd => gd.Key)
				.IsIndexed("IX_GroupData_Game_Group_Key");

			modelBuilder.Entity<GroupData>()
				.Property(gd => gd.GameId)
				.IsIndexed("IX_GroupData_Game_Group_Key_Type");
			modelBuilder.Entity<GroupData>()
				.Property(gd => gd.GroupId)
				.IsIndexed("IX_GroupData_Game_Group_Key_Type");
			modelBuilder.Entity<GroupData>()
				.Property(gd => gd.Key)
				.IsIndexed("IX_GroupData_Game_Group_Key_Type");
			modelBuilder.Entity<GroupData>()
				.Property(gd => gd.DataType)
				.IsIndexed("IX_GroupData_Game_Group_Key_Type");

			modelBuilder.Entity<UserData>()
				.Property(gd => gd.GameId)
				.IsIndexed("IX_UserData_Game_User_Key");
			modelBuilder.Entity<UserData>()
				.Property(gd => gd.UserId)
				.IsIndexed("IX_UserData_Game_User_Key");
			modelBuilder.Entity<UserData>()
				.Property(gd => gd.Key)
				.IsIndexed("IX_UserData_Game_User_Key");

			modelBuilder.Entity<UserData>()
				.Property(gd => gd.GameId)
				.IsIndexed("IX_UserData_Game_User_Key_Type");
			modelBuilder.Entity<UserData>()
				.Property(gd => gd.UserId)
				.IsIndexed("IX_UserData_Game_User_Key_Type");
			modelBuilder.Entity<UserData>()
				.Property(gd => gd.Key)
				.IsIndexed("IX_UserData_Game_User_Key_Type");
			modelBuilder.Entity<UserData>()
				.Property(gd => gd.DataType)
				.IsIndexed("IX_UserData_Game_User_Key_Type");

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