using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using Xunit;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.UnitTests;

namespace PlayGen.SUGAR.GameData.UnitTests
{
    [CollectionDefinition("Game Data Fixture Collection")]
    public class GameDataFixtureCollection : ICollectionFixture<GameDataFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class GameDataFixture : IDisposable
    {
        public const int UserCount = 100;
        public const int GameCount = 100;
        public const int GroupCount = 10;
        public const int FriendCount = 10;
        public const int DataCount = 100000;

        private readonly ProjectFixture _projectFixture;

        private readonly UserController _userController = ControllerLocator.UserController;
        private readonly GroupController _groupController = ControllerLocator.GroupController;
        private readonly GameController _gameController = ControllerLocator.GameController;
        private readonly UserRelationshipController _userRelationshipController = ControllerLocator.UserRelationshipController;
        private readonly GroupRelationshipController _groupRelationshipController = ControllerLocator.GroupRelationshipController;
        private readonly GameDataController _gameDataController = ControllerLocator.GameDataController;

        public GameDataFixture()
        {
            _projectFixture = new ProjectFixture();
			PopulateData();
		}

        public void Dispose()
        {
            _projectFixture.Dispose();
        }

        private void PopulateData()
		{
			List<Game> games = new List<Game>();
			List<User> users = new List<User>();
			List<Group> groups = new List<Group>();
			var dataValues = GenerateDataValues();
			for (int i = 0; i < UserCount; i++)
			{
				users.Add(CreateUser((i + 1).ToString()));
				
			}

			for (int i = 0; i < GameCount; i++)
			{
				games.Add(CreateGame((i + 1).ToString()));
			}

			for (int i = 0; i < GroupCount; i++)
			{
				groups.Add(CreateGroup((i + 1).ToString()));
			}

			for (int i = 0; i < users.Count; i++)
			{
				for (int j = 1; j <= FriendCount; j++)
				{
					int friendId = i + j;
					if (i + j >= users.Count)
					{
						friendId -= users.Count;
					}
					CreateFriendship(users[i].Id, users[friendId].Id);
				}
				CreateMembership(users[i].Id, groups[i / GroupCount].Id);
			}

			Random random = new Random();
			List<Data.Model.GameData> gameDatas = new List<Data.Model.GameData>();
			for (int j = 0; j < DataCount; j++)
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
				Name = "User_" + name,
			};
			_userController.Create(user);

			return user;
		}

		private Group CreateGroup(string name)
		{
			var group = new Group
			{
				Name = "Group_" + name,
			};
			_groupController.Create(group);

			return group;
		}

		private Game CreateGame(string name)
		{
			var game = new Game
			{
				Name = "Game_" + name,
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
    }
}
