using System;
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GameDataTest : ClientTestBase
	{
		[Fact]
		public void CanCreate()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GameDataTest)}_Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanCreate",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			Assert.Equal(EvaluationDataRequest.CreatingActorId, response.CreatingActorId);
			Assert.Equal(EvaluationDataRequest.GameId, response.GameId);
			Assert.Equal(EvaluationDataRequest.Key, response.Key);
			Assert.Equal(EvaluationDataRequest.Value, response.Value);
			Assert.Equal(EvaluationDataRequest.EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CanCreateWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GameDataTest)}_Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			Assert.Equal(EvaluationDataRequest.CreatingActorId, response.CreatingActorId);
			Assert.Equal(EvaluationDataRequest.GameId, response.GameId);
			Assert.Equal(EvaluationDataRequest.Key, response.Key);
			Assert.Equal(EvaluationDataRequest.Value, response.Value);
			Assert.Equal(EvaluationDataRequest.EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CanCreateWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(GameDataTest)}_Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				GameId = game.Id,
				Key = "CanCreateWithoutActorId",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			Assert.Equal(EvaluationDataRequest.CreatingActorId, response.CreatingActorId);
			Assert.Equal(EvaluationDataRequest.GameId, response.GameId);
			Assert.Equal(EvaluationDataRequest.Key, response.Key);
			Assert.Equal(EvaluationDataRequest.Value, response.Value);
			Assert.Equal(EvaluationDataRequest.EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CannotCreateWithoutKey()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GameDataTest)}_Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(GameDataTest)}_Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithoutValue()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GameDataTest)}_Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(GameDataTest)}_Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithoutKey",
				EvaluationDataType = EvaluationDataType.String,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithMismatchedData()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GameDataTest)}_Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(GameDataTest)}_Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithMismatchedData",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.Float,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Long;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Boolean;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Float;
			EvaluationDataRequest.Value = "True";

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Long;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Boolean;
			EvaluationDataRequest.Value = "2";

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));
		}

		[Fact]
		public void CanGetGameData()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GameDataTest)}_CanGetGameData");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(GameDataTest)}_CanGetGameData");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameData",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			var get = SUGARClient.GameData.Get(user.Id, game.Id, new [] { "CanGetGameData" });

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().CreatingActorId, response.CreatingActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CanGetGameDataWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(GameDataTest)}_Get");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				GameId = game.Id,
				Key = "CanGetGameDataWithoutActorId",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			var get = SUGARClient.GameData.Get(null, game.Id, new string[] { "CanGetGameDataWithoutActorId" });

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().CreatingActorId, response.CreatingActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CanGetGameDataWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GameDataTest)}_Get");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				Key = "CanGetGameDataWithoutGameId",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			var get = SUGARClient.GameData.Get(user.Id, null, new string[] { "CanGetGameDataWithoutGameId" });

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
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(GameDataTest)}_Get");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(GameDataTest)}_Get");

			var EvaluationDataRequestOne = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys1",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			var EvaluationDataRequestTwo = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys2",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			var EvaluationDataRequestThree = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys3",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String,
			};

			var responseOne = SUGARClient.GameData.Add(EvaluationDataRequestOne);
			var responseTwo = SUGARClient.GameData.Add(EvaluationDataRequestTwo);
			var responseThree = SUGARClient.GameData.Add(EvaluationDataRequestThree);

			var get = SUGARClient.GameData.Get(user.Id, game.Id, new string[] { "CanGetGameDatByMultipleKeys1", "CanGetGameDatByMultipleKeys2", "CanGetGameDatByMultipleKeys3" });

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
		public void CanCreateWithValidKey(string key)
		{
			// Arrange
			var controller = SUGARClient.GameData;

			// Act
			var data = controller.Add(new EvaluationDataRequest
			{
				Key = key,

				Value = "TestValue",

				EvaluationDataType = EvaluationDataType.String
			});

			// Assert
			Assert.NotNull(data);
		}

		[Theory]
		[InlineData("")]
		[InlineData("$")]
		[InlineData("dj_+das")]
		public void CantCreateWithInValidKey(string key)
		{
			// Arrange
			var controller = SUGARClient.GameData;

			// Act Assert
			Assert.Throws<ArgumentException>(() => controller.Add(new EvaluationDataRequest
			{
				Key = key,

				Value = "TestValue",

				EvaluationDataType = EvaluationDataType.String
			}));
		}
	}
}