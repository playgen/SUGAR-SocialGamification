using System.Threading;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using PlayGen.SUGAR.Server.EntityFramework.Extensions;

namespace PlayGen.SUGAR.Server.EntityFramework
{
	public class SUGARContextFactory
	{
		public readonly string ConnectionString;
		private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		private static bool _didCheckNew;

		public SUGARContextFactory(string connectionString = null)
		{
			ConnectionString = connectionString;
		}

		public SUGARContext Create()
		{
			var optionsBuilder = new DbContextOptionsBuilder<SUGARContext>();
			optionsBuilder.UseMySQL(ConnectionString);

			var context = new SUGARContext(optionsBuilder.Options);

			Lock.EnterUpgradeableReadLock();

			try
			{
				if (!_didCheckNew)
				{
					Lock.EnterWriteLock();

					try
					{
						// Another process could have written to this variable while we have been waiting to aquire the lock
						// hence the second check once we have aquired the write lock
						if (!_didCheckNew)
						{
							var newlyCreated = context.Database.EnsureCreated();
							if (newlyCreated)
							{
								context.Seed();
							}

							_didCheckNew = true;
						}
					}
					finally
					{
						Lock.ExitWriteLock();
					}
				}
			}
			finally
			{
				Lock.ExitUpgradeableReadLock();
			}

			return context;
		}
	}
}
