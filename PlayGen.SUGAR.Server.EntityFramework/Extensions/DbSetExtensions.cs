using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Extensions
{
	/// <summary>
	/// Functionality missing from EF.Core that was available in EF 6
	/// </summary>
	public static class DbSetExtensions
	{
		public static IQueryable<Account> IncludeAll(this DbSet<Account> dbSet)
		{
			return dbSet
				.Include(s => s.User);
		}

		public static IQueryable<Evaluation> IncludeAll(this DbSet<Evaluation> dbSet)
		{
			return dbSet
				.Include(s => s.EvaluationCriterias)
				.Include(s => s.Rewards);
		}

		public static IQueryable<Group> IncludeAll(this DbSet<Group> dbSet)
		{
			return dbSet
				.Include(s => s.Requestors)
				.Include(s => s.RequestRequestors)
				.Include(s => s.Acceptors)
				.Include(s => s.RequestAcceptors);
		}

		public static IQueryable<User> IncludeAll(this DbSet<User> dbSet)
		{
			return dbSet
				.Include(s => s.Requestors)
				.Include(s => s.RequestRequestors)
				.Include(s => s.Acceptors)
				.Include(s => s.RequestAcceptors);
		}

		public static IQueryable<Match> IncludeAll(this DbSet<Match> dbSet)
		{
			return dbSet
				.Include(s => s.Game)
				.Include(s => s.Creator);
		}
	}
}