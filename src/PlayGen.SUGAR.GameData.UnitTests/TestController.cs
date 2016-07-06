using PlayGen.SUGAR.Data.EntityFramework;

namespace PlayGen.SUGAR.GameData.UnitTests
{
	public abstract class TestController
	{
		public const string DbName = "sugardatatest";

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
	}
}