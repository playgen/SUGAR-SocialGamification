using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GameDataClientTests : ClientTestBase
	{
		private static readonly string GameDataClientTestsLeaderboardKey = "GameData_CanGetGameDataByLeaderboardType";//$"{nameof(GameDataClientTests)}_Leaderboards";
		private const int GameDataClientTestsEvaluationDataCount = 25;
		private const int GameDataClientTestsEvaluationStartValue = 1;
        private const int GameDataClientTestsEvaluationDataSum = GameDataClientTestsEvaluationDataCount * (GameDataClientTestsEvaluationDataCount + 1) / 2;
        private const float GameDataClientTestsEvaluationDataFloatMultiplier = 0.1f;

        public GameDataClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
        }

        // todo make other client tests follow thhis pattern
		// rather get core controllers from the Fixture.Server to seed the data.
		// Seeding and test setup shouldn't also test the client functionalty.
		// Client funcitonality should be tested by descrete tests per funcitonality.
        protected override object SetupClass(ClientTestsFixture fixture)
        {
            // Data for:
            // CanGetGameDataByLeaderboardType
            // DoestGetInvalidDataByLeaderboardType

	        using (var scope = fixture.Server.Host.Services.CreateScope())
	        {
		        var gameDataController = scope.ServiceProvider.GetService<GameDataController>();
		        var gameController = scope.ServiceProvider.GetService<GameController>();
		        var userController = scope.ServiceProvider.GetService<UserController>();

		        var game = gameController.Search(GameDataClientTestsLeaderboardKey)[0];
		        var user = userController.Create(new User
		        {
			        Name = GameDataClientTestsLeaderboardKey
		        });
				
		        for (var i = 0; i < GameDataClientTestsEvaluationDataCount; i++)
		        {
					foreach(EvaluationDataType dataType in Enum.GetValues(typeof(EvaluationDataType)))
					{
						string value;

						switch (dataType)
						{
                            case EvaluationDataType.String:
                            case EvaluationDataType.Long:
                                value = (i + GameDataClientTestsEvaluationStartValue).ToString();
	                            break;

                            case EvaluationDataType.Float:
								value = ((i + GameDataClientTestsEvaluationStartValue) * GameDataClientTestsEvaluationDataFloatMultiplier).ToString(CultureInfo.InvariantCulture);
								break;

                            case EvaluationDataType.Boolean:
								value = (i % 2 == 0).ToString();
								break;

							default:
								throw new Exception($"Unhandled: {dataType}");
						}
						
				        gameDataController.Add(new EvaluationData
				        {
					        ActorId = user.Id,
					        GameId = game.Id,
					        Key = GameDataClientTestsLeaderboardKey,
					        Value = value,
							EvaluationDataType = dataType
				        });
                    }
                }
	        }

	        return null;
        }

        [Fact]
		public void CanCreate()
		{
			var key = "GameData_CanCreate";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = Fixture.SUGARClient.GameData.Add(evaluationDataRequest);

			Assert.Equal(evaluationDataRequest.CreatingActorId, response.CreatingActorId);
			Assert.Equal(evaluationDataRequest.GameId, response.GameId);
			Assert.Equal(evaluationDataRequest.Key, response.Key);
			Assert.Equal(evaluationDataRequest.Value, response.Value);
			Assert.Equal(evaluationDataRequest.EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CannotCreateWithoutGameId()
		{
			var key = "GameData_CanCreateWithoutGameId";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithoutActorId()
		{
			var key = "GameData_CannotCreateWithoutActorId";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithoutKey()
		{
			var key = "GameData_CannotCreateWithoutKey";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			Assert.Throws<ArgumentException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithoutValue()
		{
			var key = "GameData_CannotCreateWithoutValue";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				EvaluationDataType = EvaluationDataType.String
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithoutEvaluationDataType()
		{
			var key = "GameData_CannotCreateWithoutEvaluationDataType";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Value = "Test Value"
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithMismatchedData()
		{
			var key = "GameData_CannotCreateWithMismatchedData";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.Float
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Long;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Boolean;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Float;
			evaluationDataRequest.Value = "True";

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Long;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Boolean;
			evaluationDataRequest.Value = "2";

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CanGetGameData()
		{
			var key = "GameData_CanGetGameData";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = Fixture.SUGARClient.GameData.Add(evaluationDataRequest);

			var get = Fixture.SUGARClient.GameData.Get(loggedInAccount.User.Id, game.Id, new [] { key });

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().CreatingActorId, response.CreatingActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CannotGetGameDataWithoutActorId()
		{
			var key = "GameData_CannotGetGameDataWithoutActorId";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			Fixture.SUGARClient.GameData.Add(evaluationDataRequest);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Get(Platform.GlobalActorId, game.Id, new[] { key }));
		}

		[Fact]
		public void CanGetGlobalGameData()
		{
			var key = "GameData_CanGetGameDataWithoutGameId";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = Platform.GlobalGameId,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = Fixture.SUGARClient.GameData.Add(evaluationDataRequest);

			var get = Fixture.SUGARClient.GameData.Get(loggedInAccount.User.Id, Platform.GlobalGameId, new[] { key });

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().CreatingActorId, response.CreatingActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CanGetGameDataByMultipleKeys()
		{
			var key = "GameData_CanGetGameDataByMultipleKeys";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var evaluationDataRequestOne = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "1",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var evaluationDataRequestTwo = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "2",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var evaluationDataRequestThree = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "3",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			Fixture.SUGARClient.GameData.Add(evaluationDataRequestOne);
			Fixture.SUGARClient.GameData.Add(evaluationDataRequestTwo);
			Fixture.SUGARClient.GameData.Add(evaluationDataRequestThree);

			var get = Fixture.SUGARClient.GameData.Get(loggedInAccount.User.Id, game.Id, new[] { evaluationDataRequestOne.Key, evaluationDataRequestTwo.Key, evaluationDataRequestThree.Key });

			Assert.Equal(3, get.Count());
			foreach (var g in get)
			{
				Assert.Equal("Test Value", g.Value);
			}
		}

		[Theory]
		[InlineData("a")]
		[InlineData("My_key0")]
		[InlineData("1mykey")]
		[InlineData("_key")]
		[InlineData("_")]
		[InlineData("9291")]
		public void CanCreateWithValidKey(string dataKey)
		{
			var key = "GameData_CanCreateWithValidKey";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			// Act
			var evaluationDataRequest = new EvaluationDataRequest
			{
				Key = dataKey,
				GameId = Platform.GlobalGameId,
				Value = "TestValue",
				EvaluationDataType = EvaluationDataType.String,
				CreatingActorId = loggedInAccount.User.Id
			};

			var response = Fixture.SUGARClient.GameData.Add(evaluationDataRequest);

			// Assert
			Assert.NotNull(response);
		}

		[Theory]
		[InlineData("")]
		[InlineData("$")]
		[InlineData("dj_+das")]
		public void CantCreateWithInValidKey(string dataKey)
		{
			var key = "GameData_CantCreateWithInValidKey";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				Key = dataKey,
				Value = "TestValue",
				EvaluationDataType = EvaluationDataType.String,
				CreatingActorId = loggedInAccount.User.Id
			};

			// Act Assert
			Assert.Throws<ArgumentException>(() => Fixture.SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Theory]
		[InlineData(EvaluationDataType.String, LeaderboardType.Count, GameDataClientTestsEvaluationDataCount)]
		[InlineData(EvaluationDataType.String, LeaderboardType.Earliest, GameDataClientTestsEvaluationStartValue)]
		[InlineData(EvaluationDataType.String, LeaderboardType.Latest, GameDataClientTestsEvaluationDataCount)]
		[InlineData(EvaluationDataType.Boolean, LeaderboardType.Count, GameDataClientTestsEvaluationDataCount)]
		[InlineData(EvaluationDataType.Boolean, LeaderboardType.Earliest, "True")]
		[InlineData(EvaluationDataType.Boolean, LeaderboardType.Latest, "True")]
		[InlineData(EvaluationDataType.Long, LeaderboardType.Cumulative, GameDataClientTestsEvaluationDataSum)]
		[InlineData(EvaluationDataType.Long, LeaderboardType.Highest, GameDataClientTestsEvaluationDataCount)]
		[InlineData(EvaluationDataType.Long, LeaderboardType.Lowest, GameDataClientTestsEvaluationStartValue)]
		[InlineData(EvaluationDataType.Long, LeaderboardType.Earliest, GameDataClientTestsEvaluationStartValue)]
		[InlineData(EvaluationDataType.Long, LeaderboardType.Latest, GameDataClientTestsEvaluationDataCount)]
		[InlineData(EvaluationDataType.Float, LeaderboardType.Cumulative, GameDataClientTestsEvaluationDataSum * GameDataClientTestsEvaluationDataFloatMultiplier)]
		[InlineData(EvaluationDataType.Float, LeaderboardType.Highest, GameDataClientTestsEvaluationDataCount * GameDataClientTestsEvaluationDataFloatMultiplier)]
		[InlineData(EvaluationDataType.Float, LeaderboardType.Lowest, GameDataClientTestsEvaluationStartValue * GameDataClientTestsEvaluationDataFloatMultiplier)]
		[InlineData(EvaluationDataType.Float, LeaderboardType.Earliest, GameDataClientTestsEvaluationStartValue * GameDataClientTestsEvaluationDataFloatMultiplier)]
		[InlineData(EvaluationDataType.Float, LeaderboardType.Latest, GameDataClientTestsEvaluationDataCount * GameDataClientTestsEvaluationDataFloatMultiplier)]
		public void CanGetGameDataByLeaderboardType(EvaluationDataType dataType, LeaderboardType leaderboardType, string expectedValue)
		{
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, GameDataClientTestsLeaderboardKey);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, GameDataClientTestsLeaderboardKey);

            var get = Fixture.SUGARClient.GameData.GetByLeaderboardType(loggedInAccount.User.Id, game.Id, GameDataClientTestsLeaderboardKey, dataType, leaderboardType);
			Assert.Equal(expectedValue, get.Value);
		}

		[Theory]
		[InlineData(EvaluationDataType.String, LeaderboardType.Highest)]
		[InlineData(EvaluationDataType.String, LeaderboardType.Lowest)]
		[InlineData(EvaluationDataType.String, LeaderboardType.Cumulative)]
		[InlineData(EvaluationDataType.Boolean, LeaderboardType.Highest)]
		[InlineData(EvaluationDataType.Boolean, LeaderboardType.Lowest)]
		[InlineData(EvaluationDataType.Boolean, LeaderboardType.Cumulative)]
		[InlineData(EvaluationDataType.Long, LeaderboardType.Count)]
		[InlineData(EvaluationDataType.Float, LeaderboardType.Count)]
        public void DoestGetInvalidDataByLeaderboardType(EvaluationDataType dataType, LeaderboardType leaderboardType)
		{
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, GameDataClientTestsLeaderboardKey);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, GameDataClientTestsLeaderboardKey);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.GetByLeaderboardType(loggedInAccount.User.Id, game.Id, GameDataClientTestsLeaderboardKey, dataType, leaderboardType));
        }
    }
}