using PlayGen.SGA.DataController;
using PlayGen.SGA.DataController.UnitTests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.QueryBuilder.UnitTests
{
    public class SQLSaveDataQueryBuilderTests
    {
        #region Configuration
        private readonly UserDbController _userDbController;
        private readonly GameDbController _gameDbController;
        private readonly UserSaveDataDbController _userSaveDataDbController;

        public SQLSaveDataQueryBuilderTests()
        {
            InitializeEnvironment();

            _userDbController = new UserDbController(TestDbController.NameOrConnectionString);
            _gameDbController = new GameDbController(TestDbController.NameOrConnectionString);
            _userSaveDataDbController = new UserSaveDataDbController(TestDbController.NameOrConnectionString);
        }

        private void InitializeEnvironment()
        {
            TestDbController.DeleteDatabase();
        }
        #endregion

        #region Tests
        [Fact]
        public void SingleStringQuery()
        {
            // TODO 
            // sql select
            // sql with params for table
            // sql value filter type
            // sql sum 
            // sql latest
            // sql from databaseQueryParams
            PopulateData();
            string query = "SELECT value FROM userdatas WHERE id = 1";
            string result = _userSaveDataDbController.Query(query);
            Console.WriteLine(result);
        }
        #endregion

        #region Helpers
        private void PopulateData()
        {
            var user = CreateUser("SQLSaveDataQueryBuilderTests_user");
            var game = CreateGame("SQLSaveDataQueryBuilderTests_game");

            var dataValues = GenerateDataValues();

            foreach (var kvp in dataValues)
            {
                CreateData(user, game, kvp.Key, kvp.Value.DataType, kvp.Value.Values);
            }
        }
        
        private void CreateData(User user, Game game, string key, DataType type, params object[] values)
        {
            foreach (var value in values)
            {
                var saveData = new UserData
                {
                    UserId = user.Id,
                    User = user,
                    GameId = game.Id,
                    Game = game,

                    Key = key,
                    Value = value.ToString(),
                    DataType = type,
                };

                _userSaveDataDbController.Create(saveData);
            }
        }

        private User CreateUser(string name)
        {
            var user = new User
            {
                Name = name,
            };
            _userDbController.Create(user);

            return user;
        }

        private Game CreateGame(string name)
        {
            var game = new Game
            {
                Name = name,
            };
            _gameDbController.Create(game);

            return game;
        }

        private Dictionary<string, DataParams> GenerateDataValues()
        {
            Random random = new Random();

            return new Dictionary<string, DataParams>
            {
                {
                    "floats", new DataParams
                    {
                        Values = Enumerable
                            .Repeat(0, 10)
                            .Select(i => (random.NextDouble()*1000) - 100)
                            .ToArray().Cast<object>().ToArray(),

                        DataType = DataType.Float,
                    }
                },
                {

                    "longs", new DataParams
                    {
                        Values = Enumerable
                            .Repeat(0, 10)
                            .Select(i => random.Next(-10, 100))
                            .ToArray().Cast<object>().ToArray(),

                        DataType = DataType.Long,
                    }
                },
                {
                    "strings", new DataParams
                    {
                        Values = CultureInfo.GetCultures(CultureTypes.AllCultures)
                            .OrderBy(x => random.Next())
                            .Take(10)
                            .Select(c => c.EnglishName)
                            .ToArray().Cast<object>().ToArray(),

                        DataType = DataType.Long,
                    }
                },
                {
                    "bools", new DataParams
                    {
                        Values = Enumerable
                            .Repeat(0, 10)
                            .Select(i => random.Next(0, 2) == 1)
                            .ToArray().Cast<object>().ToArray(),

                        DataType = DataType.Long,
                    }
                },
            };
        }

        struct DataParams
        {
            public object[] Values { get; set; }

            public DataType DataType;
        }
        #endregion
    }
}
