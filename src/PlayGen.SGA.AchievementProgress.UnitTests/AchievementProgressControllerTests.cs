using PlayGen.SGA.DataController;
using PlayGen.SGA.DataController.UnitTests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.DataModel;
using Xunit;
using DataType = PlayGen.SGA.DataModel.DataType;

namespace PlayGen.SGA.AchievementProgress.UnitTests
{
    public class AchievementProgressControllerTests
    {
        /*
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
        public void SumLongs()
        {
            User user;
            Game game;
            var generatedData = PopulateData("SumLongs", out game, out user);

            var dbResult = _userSaveDataDbController.SumLongs(game.Id, user.Id, "longs");

            var summedValue = generatedData["longs"].Values.Sum(v => Convert.ToInt64(v));

            Assert.Equal(summedValue, dbResult);
        }

        [Fact]
        public void SumFloats()
        {
            User user;
            Game game;
            var generatedData = PopulateData("SumFloats", out game, out user);

            var dbResult = _userSaveDataDbController.SumFloats(game.Id, user.Id, "floats");

            var summedValue = generatedData["floats"].Values.Sum(v => Convert.ToSingle(v));

            Assert.Equal(summedValue, dbResult);
        }

        [Fact]
        public void LatestString()
        {
            User user;
            Game game;
            var generatedData = PopulateData("TryGetLatestString", out game, out user);

            string dbResult;
            bool gotResult = _userSaveDataDbController.TryGetLatestString(game.Id, user.Id, "strings", out dbResult);

            var lastValue = (string)generatedData["strings"].Values[generatedData["strings"].Values.Length - 1];

            Assert.True(gotResult);
            Assert.Equal(lastValue, dbResult);
        }

        [Fact]
        public void LatestBool()
        {
            User user;
            Game game;
            var generatedData = PopulateData("TryGetLatestBool", out game, out user);

            bool dbResult;
            bool gotResult = _userSaveDataDbController.TryGetLatestBool(game.Id, user.Id, "bools", out dbResult);

            var lastValue = (bool)generatedData["bools"].Values[generatedData["bools"].Values.Length - 1];

            Assert.True(gotResult);
            Assert.Equal(lastValue, dbResult);
        }

        [Fact]
        public void SumMissingLongs()
        {
            var dbResult = _userSaveDataDbController.SumLongs(1, 1, "SumMissingLongs");

            Assert.Equal(0, dbResult);
        }

        [Fact]
        public void SumMissingFloats()
        {
            var dbResult = _userSaveDataDbController.SumFloats(1, 1, "SumMissingFloats");

            Assert.Equal(0, dbResult);
        }

        [Fact]
        public void LatestMissingStrings()
        {
            string dbResult;
            bool gotResult = _userSaveDataDbController.TryGetLatestString(1, 1, "LatestMissingStrings", out dbResult);

            Assert.False(gotResult);
        }

        [Fact]
        public void LatestMissingBools()
        {
            bool dbResult;
            bool gotResult = _userSaveDataDbController.TryGetLatestBool(1, 1, "LatestMissingBools", out dbResult);

            Assert.False(gotResult);
        }
        #endregion

        #region Helpers
        private Dictionary<string, DataParams> PopulateData(string name, out Game game, out User user)
        {
            user = CreateUser(name);
            game = CreateGame(name);

            var dataValues = GenerateDataValues();

            foreach (var kvp in dataValues)
            {
                CreateData(game, user, kvp.Key, kvp.Value.DataType, kvp.Value.Values);
            }

            return dataValues;
        }
        
        private void CreateData(Game game, User user, string key, DataType type, params object[] values)
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

                // Because the tests for these objects rely on their timestamps being different, 
                // their entry into the database needs to be temporally separated.
                // Could rather try sort these values in a linq expression in the test evaluation as the 
                // ticks may vary but this method seems close to what a user would do.
                if (type == DataType.Boolean || type == DataType.String)
                {
                    Thread.Sleep(1000);
                }

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

                        DataType = DataType.String,
                    }
                },
                {
                    "bools", new DataParams
                    {
                        Values = Enumerable
                            .Repeat(0, 10)
                            .Select(i => random.Next(0, 2) == 1)
                            .ToArray().Cast<object>().ToArray(),

                        DataType = DataType.Boolean,
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
        */
    }
}
