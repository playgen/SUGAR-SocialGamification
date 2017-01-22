using System;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;

namespace PlayGen.SUGAR.Data.EntityFramework
{
	/// <summary>
	/// Entity Framework Database Configuration
	/// </summary>
	public class SUGARContext : DbContext
	{
		private readonly bool _isSaveDisabled;

		internal SUGARContext(DbContextOptions<SUGARContext> options, bool disableSave = false) : base(options)
		{
			_isSaveDisabled = disableSave;
		}
		
		public DbSet<Account> Accounts { get; set; }
		public DbSet<AccountSource> AccountSources { get; set; }

		public DbSet<Game> Games { get; set; }
		
		public DbSet<Evaluation> Evaluations { get; set; }
		public DbSet<SentEvaluationNotification> SentEvaluationNotifications { get; set; }
		public DbSet<Achievement> Achievements { get; set; }
		public DbSet<Skill> Skills { get; set; }

		public DbSet<Actor> Actors { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Group> Groups { get; set; }

		public DbSet<Match> Matches { get; set; }

		public DbSet<EvaluationData> EvaluationData { get; set; }
		public DbSet<ActorData> ActorData { get; set; }

		public DbSet<UserToUserRelationshipRequest> UserToUserRelationshipRequests { get; set; }
		public DbSet<UserToUserRelationship> UserToUserRelationships { get; set; }
		public DbSet<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }
		public DbSet<UserToGroupRelationship> UserToGroupRelationships { get; set; }

		public DbSet<Leaderboard> Leaderboards { get; set; }
		public DbSet<Claim> Claims { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<RoleClaim> RoleClaims { get; set; }
		public DbSet<ActorRole> ActorRoles { get; set; }
		public DbSet<ActorClaim> ActorClaims { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ConfigureTableNames();
			modelBuilder.ConfigureHierarchy();
			modelBuilder.ConfigureCompositePrimaryKeys();
			modelBuilder.ConfigureIndexes();
			modelBuilder.ConfigureForeignKeys();
			modelBuilder.ConfigureProperties();
		}

		public override int SaveChanges()
		{
			UpdateModificationHistory();

			return _isSaveDisabled 
				? 0
				: base.SaveChanges();
		}

		/// <summary>
		/// User reflection to detect classes that implement the IModificationHistory interface
		/// and set their DateCreated and DateModified DateTime fields accordingly.
		/// </summary>
		private void UpdateModificationHistory()
		{
			var histories = ChangeTracker.Entries()
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
		}
	}
}