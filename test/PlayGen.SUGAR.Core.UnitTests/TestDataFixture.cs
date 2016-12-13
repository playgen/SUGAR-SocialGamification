using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Core.Controllers;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.UnitTests
{
    public class TestDataFixture : IDisposable
    {
        public const int UserCount = 100;
        public const int GameCount = 100;
        public const int GroupCount = 10;
        public const int FriendCount = 10;
        public const int DataCount = 100000;

        private static readonly List<Game> _games = new List<Game>(DataCount);
        private static readonly List<User> _users = new List<User>(UserCount);
        private static readonly List<Group> _groups = new List<Group>(GroupCount);
        
        private static readonly Random _random = new Random(123);

        private static readonly UserController _userController = ControllerLocator.UserController;
        private static readonly GroupController _groupController = ControllerLocator.GroupController;
        private static readonly GameController _gameController = ControllerLocator.GameController;
        private static readonly UserFriendController _userFriendController = ControllerLocator.UserFriendController;
        private static readonly GroupMemberController _groupMemberController = ControllerLocator.GroupMemberController;
        private static readonly SaveDataController SaveDataController = ControllerLocator.SaveDataController;

        public static IReadOnlyList<Game> Games => _games;
        public static IReadOnlyList<User> Users => _users;
        public static IReadOnlyList<Group> Groups => _groups;

        public TestDataFixture()
        {
            PopulateData();
        }

        public void Dispose()
        {
        }

        private static void PopulateData()
        {
            _games.Clear();
            _users.Clear();
            _groups.Clear();

            var dataValues = GenerateDataValues();
            for (int i = 0; i < UserCount; i++)
            {
                _users.Add(CreateUser((i + 1).ToString()));

            }

            for (int i = 0; i < GameCount; i++)
            {
                _games.Add(CreateGame((i + 1).ToString()));
            }

            for (int i = 0; i < GroupCount; i++)
            {
                _groups.Add(CreateGroup((i + 1).ToString()));
            }

            for (int i = 0; i < _users.Count; i++)
            {
                for (int j = 1; j <= FriendCount; j++)
                {
                    int friendId = i + j;
                    if (i + j >= _users.Count)
                    {
                        friendId -= _users.Count;
                    }
                    CreateFriendship(_users[i].Id, _users[friendId].Id);
                }
                CreateMembership(_users[i].Id, _groups[i / GroupCount].Id);
            }

            List<Data.Model.SaveData> gameDatas = new List<Data.Model.SaveData>();
            for (int j = 0; j < DataCount; j++)
            {
                gameDatas.Add(CreateData(_games[_random.Next(0, _games.Count)], _users[_random.Next(0, _users.Count)], dataValues[_random.Next(0, dataValues.Count)]));
            }
            SaveDataController.Add(gameDatas.ToArray());
        }

        private static SaveData CreateData(Game game, User user, DataParam data)
        {
            var gameData = new Data.Model.SaveData
            {
                ActorId = user.Id,
                GameId = game.Id,
                Key = data.DataType.ToString(),
                Value = data.Value,
                SaveDataType = data.DataType,
            };

            return gameData;
        }

        private static User CreateUser(string name)
        {
            var user = new User
            {
                Name = "User_" + name,
            };
            _userController.Create(user);

            return user;
        }

        private static Group CreateGroup(string name)
        {
            var group = new Group
            {
                Name = "Group_" + name,
            };
            //todo use actual user id instead of 0
            _groupController.Create(group, 1);

            return group;
        }

        private static Game CreateGame(string name)
        {
            var game = new Game
            {
                Name = "Game_" + name,
            };
            //todo use actual user id instead of 0
            _gameController.Create(game, 1);

            return game;
        }

        private static void CreateFriendship(int requestor, int acceptor)
        {
            var relationship = new UserToUserRelationship
            {
                RequestorId = requestor,
                AcceptorId = acceptor,
            };
            _userFriendController.CreateFriendRequest(relationship, true);
        }

        private static void CreateMembership(int requestor, int acceptor)
        {
            var relationship = new UserToGroupRelationship
            {
                RequestorId = requestor,
                AcceptorId = acceptor,
            };
            _groupMemberController.CreateMemberRequest(relationship, true);
        }

        private static List<DataParam> GenerateDataValues()
        {
            List<DataParam> dataParams = new List<DataParam>();

            for (int i = 0; i < 2500; i++)
            {
                dataParams.Add(new DataParam
                {
                    Value = (_random.NextDouble() * 1000).ToString("f5"),
                    DataType = SaveDataType.Float,
                });
                dataParams.Add(new DataParam
                {
                    Value = _random.Next(0, 1000).ToString(),
                    DataType = SaveDataType.Long,
                });
                dataParams.Add(new DataParam
                {
                    Value = _random.Next(0, 1000).ToString(),
                    DataType = SaveDataType.String,
                });
                dataParams.Add(new DataParam
                {
                    Value = (_random.Next(0, 2) == 1 ? true : false).ToString(),
                    DataType = SaveDataType.Boolean,
                });
            }

            return dataParams;
        }

        private struct DataParam
        {
            public string Value { get; set; }

            public SaveDataType DataType;
        }
    }
}
