using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.Entity;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.Model.Interfaces;
using PlayGen.SUGAR.Data.EntityFramework.ExtensionMethods;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayGen.SUGAR.Data.EntityFramework
{
	/// <summary>
	/// Entity Framework Database Configuration
	/// </summary>
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class SGAContext : DbContext
	{
		private readonly bool _isSaveDisabled;

		public SGAContext(string nameOrConnectionString, bool disableSave = false) : base(nameOrConnectionString)
		{
			_isSaveDisabled = disableSave;
			Database.SetInitializer(new CreateDatabaseIfNotExists<SGAContext>());
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SGAContext>());
		}

		public DbSet<Account> Accounts { get; set; }

		public DbSet<Game> Games { get; set; }

		public DbSet<Achievement> Achievements { get; set; }
		public DbSet<Skill> Skills { get; set; }

		public DbSet<Leaderboard> Leaderboards { get; set; }

		public DbSet<Actor> Actors { get; set; }

		public DbSet<User> Users { get; set; }
		public DbSet<Group> Groups { get; set; }


		public DbSet<GameData> GameData { get; set; }

		public DbSet<UserToUserRelationshipRequest> UserToUserRelationshipRequests { get; set; }
		public DbSet<UserToUserRelationship> UserToUserRelationships { get; set; }
		public DbSet<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }
		public DbSet<UserToGroupRelationship> UserToGroupRelationships { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable("Users");
			modelBuilder.Entity<Group>().ToTable("Groups");

			modelBuilder.Entity<GameData>()
				.ToTable("GameData");

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

			// Setup composite primary keys
			modelBuilder.Entity<Achievement>()
				.HasKey(a => new { a.Token, a.GameId });
			modelBuilder.Entity<Skill>()
				.HasKey(a => new { a.Token, a.GameId });
			modelBuilder.Entity<Leaderboard>()
				.HasKey(a => new { a.Token, a.GameId });

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

			// multiple indexes for a single property must be added in the same fluent call
			modelBuilder.Entity<GameData>()
				.Property(gd => gd.Key)
				.IsIndexed("IX_GameData_Game_Actor_Key", 0)
				.IsIndexed("IX_GameData_Game_Actor_Key_Type", 0);
			modelBuilder.Entity<GameData>()
				.Property(gd => gd.GameId)
				.IsIndexed("IX_GameData_Game_Actor_Key", 1)
				.IsIndexed("IX_GameData_Game_Actor_Key_Type", 1);
			modelBuilder.Entity<GameData>()
				.Property(gd => gd.ActorId)
				.IsIndexed("IX_GameData_Game_Actor_Key", 2)
				.IsIndexed("IX_GameData_Game_Actor_Key_Type", 2);
			modelBuilder.Entity<GameData>()
				.Property(gd => gd.DataType)
				.IsIndexed("IX_GameData_Game_Actor_Key_Type", 4);

			// Serialize specific objects as Json objects instead of creating a new table
			modelBuilder.ComplexType<AchievementCriteriaCollection>()
				.Property(p => p.Serialised)
				.HasColumnName("CompletionCriteria")
				.HasMaxLength(1024);
			modelBuilder.ComplexType<RewardCollection>()
				.Property(p => p.Serialised)
				.HasColumnName("RewardCollection")
				.HasMaxLength(1024);

			modelBuilder.Entity<GameData>()
				.Property(g => g.DateCreated)
				.HasPrecision(3);
			modelBuilder.Entity<GameData>()
				.Property(g => g.DateModified)
				.HasPrecision(3);

			// Change all string fields to have a max length of 64 chars
			modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(64));

			modelBuilder.Entity<Achievement>()
				.Property(p => p.Description)
				.HasMaxLength(256);

			// Setup composite primary keys
			modelBuilder.Entity<UserToUserRelationshipRequest>().HasKey(k => new { k.RequestorId, k.AcceptorId });
			modelBuilder.Entity<UserToUserRelationship>().HasKey(k => new { k.RequestorId, k.AcceptorId });
			modelBuilder.Entity<UserToGroupRelationshipRequest>().HasKey(k => new { k.RequestorId, k.AcceptorId });
			modelBuilder.Entity<UserToGroupRelationship>().HasKey(k => new {k.RequestorId, k.AcceptorId});
		}

		public override int SaveChanges()
		{
			// User reflection to detect classes that implement the IModificationHistory interface
			// and set their DateCreated and DateModified DateTime fields accordingly.
			var histories = this.ChangeTracker.Entries()
				.Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added || e.State == EntityState.Modified))
				.Select(e => e.Entity as IModificationHistory);

			foreach (var history in histories)
			{
				history.DateModified = DateTime.Now;

				if (history.DateCreated == default(DateTime))
				{
					history.DateCreated = DateTime.Now;
				}
			}

			if (_isSaveDisabled)
			{
				return 0; 
			}
			else
			{
				return base.SaveChanges();
			}
		}
	}
}