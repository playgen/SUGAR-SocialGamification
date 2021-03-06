﻿using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
	public class AchievementProgressControllerTests : CoreTestBase
	{
		/*
		#region Configuration
		private readonly UserDbController _userDbController;
		private readonly GameDbController _gameDbController;
		private readonly UserDataDbController _userEvaluationDataDbController;

		public SQLEvaluationDataQueryBuilderTests()
		{
			InitializeEnvironment();

			_userDbController = new UserDbController(TestDbController.NameOrConnectionString);
			_gameDbController = new GameDbController(TestDbController.NameOrConnectionString);
			_userEvaluationDataDbController = new UserDataDbController(TestDbController.NameOrConnectionString);
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
			var generatedData = PopulateData("SumLongs", out var game, out var user);

			var dbResult = _userEvaluationDataDbController.SumLongs(game.Id, user.Id, "longs");

			var summedValue = generatedData["longs"].Values.Sum(v => Convert.ToInt64(v));

			Assert.Equal(summedValue, dbResult);
		}

		[Fact]
		public void SumFloats()
		{
			var generatedData = PopulateData("SumFloats", out var game, out var user);

			var dbResult = _userEvaluationDataDbController.SumFloats(game.Id, user.Id, "floats");

			var summedValue = generatedData["floats"].Values.Sum(v => Convert.ToSingle(v));

			Assert.Equal(summedValue, dbResult);
		}

		[Fact]
		public void LatestString()
		{
			var generatedData = PopulateData("TryGetLatestString", out var game, out var user);

			bool gotResult = _userEvaluationDataDbController.TryGetLatestString(game.Id, user.Id, "strings", out var dbResult);

			var lastValue = (string)generatedData["strings"].Values[generatedData["strings"].Values.Length - 1];

			Assert.True(gotResult);
			Assert.Equal(lastValue, dbResult);
		}

		[Fact]
		public void LatestBool()
		{
			var generatedData = PopulateData("TryGetLatestBool", out var game, out var user);

			bool gotResult = _userEvaluationDataDbController.TryGetLatestBool(game.Id, user.Id, "bools", out var dbResult);

			var lastValue = (bool)generatedData["bools"].Values[generatedData["bools"].Values.Length - 1];

			Assert.True(gotResult);
			Assert.Equal(lastValue, dbResult);
		}

		[Fact]
		public void SumMissingLongs()
		{
			var dbResult = _userEvaluationDataDbController.SumLongs(1, 1, "SumMissingLongs");

			Assert.Equal(0, dbResult);
		}

		[Fact]
		public void SumMissingFloats()
		{
			var dbResult = _userEvaluationDataDbController.SumFloats(1, 1, "SumMissingFloats");

			Assert.Equal(0, dbResult);
		}

		[Fact]
		public void LatestMissingStrings()
		{
			bool gotResult = _userEvaluationDataDbController.TryGetLatestString(1, 1, "LatestMissingStrings", out var dbResult);

			Assert.False(gotResult);
		}

		[Fact]
		public void LatestMissingBools()
		{
			bool gotResult = _userEvaluationDataDbController.TryGetLatestBool(1, 1, "LatestMissingBools", out var dbResult);

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
				CreateData(game, user, kvp.Key, kvp.Value.EvaluationDataType, kvp.Value.Values);
			}

			return dataValues;
		}
		
		private void CreateData(Game game, User user, string key, EvaluationDataType valueType, params object[] values)
		{
			foreach (var value in values)
			{
				var evaluationData = new UserData
				{
					UserId = user.Id,
					User = user,
					GameId = game.Id,
					Game = game,

					Key = key,
					Value = value.ToString(),
					EvaluationDataType = valueType,
				};

				// Because the tests for these objects rely on their timestamps being different, 
				// their entry into the database needs to be temporally separated.
				// Could rather try sort these values in a linq expression in the test evaluation as the 
				// ticks may vary but this method seems close to what a user would do.
				if (valueType == EvaluationDataType.Boolean || valueType == EvaluationDataType.String)
				{
					Thread.Sleep(1000);
				}

				_userEvaluationDataDbController.Create(evaluationData);
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

						EvaluationDataType = EvaluationDataType.Float,
					}
				},
				{

					"longs", new DataParams
					{
						Values = Enumerable
							.Repeat(0, 10)
							.Select(i => random.Next(-10, 100))
							.ToArray().Cast<object>().ToArray(),

						EvaluationDataType = EvaluationDataType.Long,
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

						EvaluationDataType = EvaluationDataType.String,
					}
				},
				{
					"bools", new DataParams
					{
						Values = Enumerable
							.Repeat(0, 10)
							.Select(i => random.Next(0, 2) == 1)
							.ToArray().Cast<object>().ToArray(),

						EvaluationDataType = EvaluationDataType.Boolean,
					}
				},
			};
		}

		struct DataParams
		{
			public object[] Values { get; set; }

			public EvaluationDataType EvaluationDataType;
		}
		#endregion*/
	}
}
