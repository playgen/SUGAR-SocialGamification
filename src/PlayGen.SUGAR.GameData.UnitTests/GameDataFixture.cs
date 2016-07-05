using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework;

namespace PlayGen.SUGAR.GameData.UnitTests
{
	public class GameDataFixture : IDisposable
	{
		private readonly UserController _userController;
		private readonly GameController _gameController;
		private readonly GameDataController _gameDataController;

		public GameDataFixture()
		{
			Dispose();
			_userController = new UserController(TestController.NameOrConnectionString);
			_gameController = new GameController(TestController.NameOrConnectionString);
			_gameDataController = new GameDataController(TestController.NameOrConnectionString);
			PopulateData();
		}

		private void PopulateData()
		{
			List<Game> games = new List<Game>();
			List<User> users = new List<User>();
			var dataValues = GenerateDataValues();
			for (int i = 0; i < 100; i++)
			{
				users.Add(CreateUser((i + 1).ToString()));
				games.Add(CreateGame((i + 1).ToString()));
			}

			Random random = new Random();

			for (int j = 0; j < 400; j++)
			{
				List<Data.Model.GameData> gameDatas = new List<Data.Model.GameData>();
				for (int k = 0; k < 400; k++)
				{
					gameDatas.Add(CreateData(games[random.Next(0, games.Count)], users[random.Next(0, users.Count)], dataValues[random.Next(0, dataValues.Count)]));
				}
				_gameDataController.Create(gameDatas.ToArray());
			}

			
		}

		private Data.Model.GameData CreateData(Game game, User user, DataParam data)
		{
			var gameData = new Data.Model.GameData
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = data.DataType.ToString(),
				Value = data.Value,
				DataType = data.DataType,
			};

			return gameData;
		}

		private User CreateUser(string name)
		{
			var user = new User
			{
				Name = name,
			};
			_userController.Create(user);

			return user;
		}

		private Game CreateGame(string name)
		{
			var game = new Game
			{
				Name = name,
			};
			_gameController.Create(game);

			return game;
		}

		private List<DataParam> GenerateDataValues()
		{
			Random random = new Random();
			List<DataParam> dataParams = new List<DataParam>();

			for (int i = 0; i < 500; i++)
			{
				dataParams.Add(new DataParam
				{
					Value = (random.NextDouble() * 1000).ToString("f5"),
					DataType = GameDataType.Float,
				});
				dataParams.Add(new DataParam
				{
					Value = random.Next(0, 1000).ToString(),
					DataType = GameDataType.Long,
				});
				dataParams.Add(new DataParam
				{
					Value = random.Next(0, 1000).ToString(),
					DataType = GameDataType.String,
				});
				dataParams.Add(new DataParam
				{
					Value = (random.Next(0, 2) == 1 ? true : false).ToString(),
					DataType = GameDataType.Boolean,
				});
			}

			return dataParams;
		}

		struct DataParam
		{
			public string Value { get; set; }

			public GameDataType DataType;
		}

		public void Dispose()
		{
			using (var context = new SGAContext(TestController.NameOrConnectionString))
			{
				if (context.Database.Connection.Database == TestController.DbName)
				{
					context.Database.Delete();
				}
			}
		}
	}
}
