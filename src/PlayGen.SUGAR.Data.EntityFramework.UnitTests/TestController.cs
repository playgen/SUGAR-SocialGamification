using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using System;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class TestController : IDisposable
	{
		private readonly string _dbName;
		private readonly string _connectionString;

		private GameDataController _gameDataController;
		public GameDataController GameDataController => _gameDataController ?? _gameDataController = new GameDataController(_connectionString);;

		public TestController(string dbName = "sgaunittests")
		{
			_dbName = dbName;
			_connectionString = GetNameOrConnectionString(_dbName);

			DeleteDatabase();
		}

		private string GetNameOrConnectionString(string dbName)
		{
			return "Server=localhost;" +
					"Port=3306;" +
					$"Database={DbName};" +
					"Uid=root;" +
					"Pwd=t0pSECr3t;" +
					"Convert Zero Datetime=true;" +
					"Allow Zero Datetime=true";
		}

		private void DeleteDatabase()
		{
			using (var context = new SGAContext(_connectionString))
			{
				if (context.Database.Connection.Database == _dbName)
				{
					context.Database.Delete();
				}
				else
				{
					throw new Exception($"Database with name: {_dbName} doesn't exist.");
				}
			}
		}
		
		public void Dispose()
		{
			DeleteDatabase();
		}
	}
}