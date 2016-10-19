using PlayGen.SUGAR.Data.EntityFramework;

namespace PlayGen.SUGAR.GameData.UnitTests
{
	public abstract class TestController
	{
		public const string DbName = "sugardatatest";

		private static string _nameOrConnectionString = null;

		private static bool _deletedDatabase = false;

		public static int UserCount = 100;

		public static int GameCount = 100;

		public static int GroupCount = 10;

		public static int FriendCount = 10;

		public static int DataCount = 100000;

		public static string NameOrConnectionString
		{
			get
			{
				if (_nameOrConnectionString == null)
				{
					_nameOrConnectionString = "Server=(localdb)\\mssqllocaldb;" +
											  $"Database={DbName};" +
											  "Uid=root;" +
											  "Pwd=t0pSECr3t;" +
											  "Trusted_Connection=True;" +
											  "MultipleActiveResultSets=true";
				}

				return _nameOrConnectionString;
			}
		}
	}
}