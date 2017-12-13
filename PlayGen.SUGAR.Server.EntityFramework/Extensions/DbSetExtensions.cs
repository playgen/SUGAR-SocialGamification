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
				.ThenInclude(r => r.Requestor)
				.Include(s => s.Requestors)
				.ThenInclude(r => r.Acceptor)
				.Include(s => s.RequestRequestors)
				.ThenInclude(r => r.Requestor)
				.Include(s => s.RequestRequestors)
				.ThenInclude(r => r.Acceptor)
				.Include(s => s.Acceptors)
				.ThenInclude(r => r.Requestor)
				.Include(s => s.Acceptors)
				.ThenInclude(r => r.Acceptor)
				.Include(s => s.RequestAcceptors)
				.ThenInclude(r => r.Requestor)
				.Include(s => s.RequestAcceptors)
				.ThenInclude(r => r.Acceptor);
		}

		public static IQueryable<User> IncludeAll(this DbSet<User> dbSet)
		{
			return dbSet
				.Include(s => s.Requestors)
				.ThenInclude(r => r.Requestor)
				.Include(s => s.Requestors)
				.ThenInclude(r => r.Acceptor)
				.Include(s => s.RequestRequestors)
				.ThenInclude(r => r.Requestor)
				.Include(s => s.RequestRequestors)
				.ThenInclude(r => r.Acceptor)
				.Include(s => s.Acceptors)
				.ThenInclude(r => r.Requestor)
				.Include(s => s.Acceptors)
				.ThenInclude(r => r.Acceptor)
				.Include(s => s.RequestAcceptors)
				.ThenInclude(r => r.Requestor)
				.Include(s => s.RequestAcceptors)
				.ThenInclude(r => r.Acceptor);
		}

		public static IQueryable<Match> IncludeAll(this DbSet<Match> dbSet)
		{
			return dbSet
				.Include(s => s.Game)
				.Include(s => s.Creator);
		}
	}
}