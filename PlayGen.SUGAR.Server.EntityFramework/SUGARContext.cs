using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;
using PlayGen.SUGAR.Server.Model.Interfaces;

namespace PlayGen.SUGAR.Server.EntityFramework
{
	/// <summary>
	/// Entity Framework Database Configuration
	/// </summary>
	public class SUGARContext : DbContext
	{
		private readonly bool _isReadOnly;
		private readonly DbExceptionHandler _exceptionHandler = new DbExceptionHandler();

		public SUGARContext(DbContextOptions<SUGARContext> options, bool isReadOnly = false) : base(options)
		{
			_isReadOnly = isReadOnly;
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

		public DbSet<ActorRelationship> Relationships { get; set; }
		public DbSet<ActorRelationshipRequest> RelationshipRequests { get; set; }

		public DbSet<Leaderboard> Leaderboards { get; set; }
		public DbSet<Claim> Claims { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<RoleClaim> RoleClaims { get; set; }
		public DbSet<ActorRole> ActorRoles { get; set; }
		public DbSet<ActorClaim> ActorClaims { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var implementedConfigTypes = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => !t.IsAbstract
					&& !t.IsGenericTypeDefinition
					&& t.GetTypeInfo().ImplementedInterfaces.Any(i =>
						i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));
			
			foreach (var configType in implementedConfigTypes)
			{
				dynamic config = Activator.CreateInstance(configType);
				modelBuilder.ApplyConfiguration(config);
			}
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess = true)
		{
			try
			{
				if (_isReadOnly)
				{
					throw new ReadOnlyContextException();
				}

                UpdateModificationHistory();
				return base.SaveChanges(acceptAllChangesOnSuccess);
            }
			catch (Exception exception)
			{
				throw _exceptionHandler.Process(exception);
			}
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
				if (history != null)
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
}