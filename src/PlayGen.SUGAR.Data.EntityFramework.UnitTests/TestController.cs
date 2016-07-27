namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public abstract class TestController
	{
		public const string DbName = "sugarunittests";

		private static string _nameOrConnectionString = null;

		private static bool _deletedDatabase = false;

		public static string NameOrConnectionString
		{
			get
			{
				if (_nameOrConnectionString == null)
				{
					_nameOrConnectionString = "Server=localhost;" +
											  "Port=3306;" +
											  $"Database={DbName};" +
											  "Uid=root;" +
											  "Pwd=t0pSECr3t;" +
											  "Convert Zero Datetime=true;" +
											  "Allow Zero Datetime=true";
				}

				return _nameOrConnectionString;
			}
		}

		public static void DeleteDatabase()
		{
			if (!_deletedDatabase)
			{
				using (var context = new SUGARContext(NameOrConnectionString))
				{
					if (context.Database.Connection.Database == DbName)
					{
						context.Database.Delete();
						_deletedDatabase = true;
					}
				}
			}
		}

		protected TestController()
		{
			DeleteDatabase();
		}
	}
}