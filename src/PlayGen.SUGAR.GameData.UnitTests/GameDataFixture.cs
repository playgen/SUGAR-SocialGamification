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
		private readonly GroupController _groupController;
		private readonly GameController _gameController;
		private readonly UserRelationshipController _userRelationshipController;
		private readonly GroupRelationshipController _groupRelationshipController;
		private readonly GameDataController _gameDataController;

		public GameDataFixture()
		{
			DeleteExisting();
			_userController = new UserController(TestController.NameOrConnectionString);
			_groupController = new GroupController(TestController.NameOrConnectionString);
			_userRelationshipController = new UserRelationshipController(TestController.NameOrConnectionString);
			_groupRelationshipController = new GroupRelationshipController(TestController.NameOrConnectionString);
			_gameController = new GameController(TestController.NameOrConnectionString);
			_gameDataController = new GameDataController(TestController.NameOrConnectionString);
			PopulateData();
		}

		private void PopulateData()
		{
			List<Game> games = new List<Game>();
			List<User> users = new List<User>();
			List<Group> groups = new List<Group>();
			var dataValues = GenerateDataValues();
			for (int i = 0; i < TestController.UserCount; i++)
			{
				users.Add(CreateUser((i + 1).ToString()));
				
			}

			for (int i = 0; i < TestController.GameCount; i++)
			{
				games.Add(CreateGame((i + 1).ToString()));
			}

			for (int i = 0; i < TestController.GroupCount; i++)
			{
				groups.Add(CreateGroup((i + 1).ToString()));
			}

			for (int i = 0; i < users.Count; i++)
			{
				for (int j = 1; j <= TestController.FriendCount; j++)
				{
					int friendId = i + j;
					if (i + j >= users.Count)
					{
						friendId -= users.Count;
					}
					CreateFriendship(users[i].Id, users[friendId].Id);
				}
				CreateMembership(users[i].Id, groups[i / TestController.GroupCount].Id);
			}

			Random random = new Random();
			List<Data.Model.GameData> gameDatas = new List<Data.Model.GameData>();
			for (int j = 0; j < TestController.DataCount; j++)
			{
				gameDatas.Add(CreateData(games[random.Next(0, games.Count)], users[random.Next(0, users.Count)], dataValues[random.Next(0, dataValues.Count)]));
			}
			_gameDataController.Create(gameDatas.ToArray());
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

		private Group CreateGroup(string name)
		{
			var group = new Group
			{
				Name = name,
			};
			_groupController.Create(group);

			return group;
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

		private void CreateFriendship(int requestor, int acceptor)
		{
			var relationship = new UserToUserRelationship
			{
				RequestorId = requestor,
				AcceptorId = acceptor,
			};
			_userRelationshipController.Create(relationship, true);
		}

		private void CreateMembership(int requestor, int acceptor)
		{
			var relationship = new UserToGroupRelationship
			{
				RequestorId = requestor,
				AcceptorId = acceptor,
			};
			_groupRelationshipController.Create(relationship, true);
		}

		private List<DataParam> GenerateDataValues()
		{
			Random random = new Random();
			List<DataParam> dataParams = new List<DataParam>();

			for (int i = 0; i < 2500; i++)
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
			
		}

		private void DeleteExisting()
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
