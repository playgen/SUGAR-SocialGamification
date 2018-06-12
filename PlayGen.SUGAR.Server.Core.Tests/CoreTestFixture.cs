using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.EntityFramework.Tests;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
    public class CoreTestFixture
    {
		// Must be divisible by GroupCount and (FriendCount + 1)
        public const int UserCount = 1000;
        public const int GameCount = 10;
        public const int GroupCount = 10;
        public const int FriendCount = 9;
        public const int DataCount = 100000;

        private readonly List<Game> _games = new List<Game>(DataCount);
        private readonly List<User> _users = new List<User>(UserCount);
        private readonly List<Group> _groups = new List<Group>(GroupCount);
        
        private readonly Random _random = new Random(123);

        private readonly UserController _userController = ControllerLocator.UserController;
        private readonly GroupController _groupController = ControllerLocator.GroupController;
        private readonly GameController _gameController = ControllerLocator.GameController;
	    private readonly RelationshipController _relationshipController = ControllerLocator.RelationshipController;
        private readonly GameDataController _gameDataController = ControllerLocator.GameDataController;

        public IReadOnlyList<Game> Games => _games;
        public IReadOnlyList<User> Users => _users;
        public IReadOnlyList<Group> Groups => _groups;

        public CoreTestFixture()
        {
			ClearDatabaseFixture.Clear();
            PopulateData();
        }
		
        private void PopulateData()
        {
            _games.Clear();
            _users.Clear();
            _groups.Clear();

            var dataValues = GenerateDataValues();
            for (var i = 0; i < UserCount; i++)
            {
                _users.Add(CreateUser((i + 1).ToString()));
            }

            for (var i = 0; i < GameCount; i++)
            {
                _games.Add(CreateGame((i + 1).ToString()));
            }

            for (var i = 0; i < GroupCount; i++)
            {
                _groups.Add(CreateGroup((i + 1).ToString()));
            }

            for (var userIndex = 0; userIndex < _users.Count; userIndex++)
            {
	            var windowSize = FriendCount + 1;
	            var indexInWindow = userIndex % windowSize;
	            var friendsToAddCount = windowSize - (indexInWindow + 1);

	            for (var friendIndexOffset = 1; friendIndexOffset <= friendsToAddCount; friendIndexOffset++)
	            {
		            var friendIndex = userIndex + friendIndexOffset;

		            CreateFriendship(_users[userIndex].Id, _users[friendIndex].Id);
                }
				
                CreateMembership(_users[userIndex].Id, _groups[userIndex % GroupCount].Id);
            }

            var datas = new List<EvaluationData>();
            for (var j = 0; j < DataCount; j++)
            {
                datas.Add(CreateData(_games[_random.Next(0, _games.Count)], _users[_random.Next(0, _users.Count)], dataValues[_random.Next(0, dataValues.Count)]));
            }
            _gameDataController.Add(datas.ToArray());
        }

        private EvaluationData CreateData(Game game, User user, DataParam data)
        {
            var gameData = new EvaluationData
            {
                ActorId = user.Id,
                GameId = game.Id,
                Key = data.DataType.ToString(),
                Value = data.Value,
                EvaluationDataType = data.DataType,
            };

            return gameData;
        }

        private User CreateUser(string name)
        {
            var user = new User
            {
                Name = $"User_{name}",
            };
            _userController.Create(user);

            return user;
        }

        private Group CreateGroup(string name)
        {
            var group = new Group
            {
                Name = $"Group_{name}",
            };
            //todo use actual user id instead of 0
            _groupController.Create(group, 1);

            return group;
        }

        private Game CreateGame(string name)
        {
            var game = new Game
            {
                Name = $"Game_{name}",
            };
            //todo use actual user id instead of 0
            _gameController.Create(game, 1);

            return game;
        }

        private void CreateFriendship(int requestor, int acceptor)
        {
            var relationship = new ActorRelationship
			{
                RequestorId = requestor,
                AcceptorId = acceptor,
            };
	        _relationshipController.CreateRequest(relationship, true);
        }

        private void CreateMembership(int requestor, int acceptor)
        {
            var relationship = new ActorRelationship
            {
                RequestorId = requestor,
                AcceptorId = acceptor,
            };
	        _relationshipController.CreateRequest(relationship, true);
        }

        private List<DataParam> GenerateDataValues()
        {
            var dataParams = new List<DataParam>();

            for (var i = 0; i < 2500; i++)
            {
                dataParams.Add(new DataParam
                {
                    Value = (_random.NextDouble() * 1000).ToString("f5"),
                    DataType = EvaluationDataType.Float,
                });
                dataParams.Add(new DataParam
                {
                    Value = _random.Next(0, 1000).ToString(),
                    DataType = EvaluationDataType.Long,
                });
                dataParams.Add(new DataParam
                {
                    Value = _random.Next(0, 1000).ToString(),
                    DataType = EvaluationDataType.String,
                });
                dataParams.Add(new DataParam
                {
                    Value = (_random.Next(0, 2) == 1 ? true : false).ToString(),
                    DataType = EvaluationDataType.Boolean,
                });
            }

            return dataParams;
        }

        private struct DataParam
        {
            public string Value { get; set; }

            public EvaluationDataType DataType;
        }
    }

	[CollectionDefinition(nameof(CoreTestFixtureCollection))]
	public class CoreTestFixtureCollection : ICollectionFixture<CoreTestFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
