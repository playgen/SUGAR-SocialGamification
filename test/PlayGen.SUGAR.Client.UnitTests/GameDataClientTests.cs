using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class GameDataTests : ClientTestsBase
	{
		[Test]
		public void CanCreate()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanCreate",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = SUGARClient.GameData.Add(SaveDataRequest);

			Assert.AreEqual(SaveDataRequest.ActorId, response.ActorId);
			Assert.AreEqual(SaveDataRequest.GameId, response.GameId);
			Assert.AreEqual(SaveDataRequest.Key, response.Key);
			Assert.AreEqual(SaveDataRequest.Value, response.Value);
			Assert.AreEqual(SaveDataRequest.SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanCreateWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = SUGARClient.GameData.Add(SaveDataRequest);

			Assert.AreEqual(SaveDataRequest.ActorId, response.ActorId);
			Assert.AreEqual(SaveDataRequest.GameId, response.GameId);
			Assert.AreEqual(SaveDataRequest.Key, response.Key);
			Assert.AreEqual(SaveDataRequest.Value, response.Value);
			Assert.AreEqual(SaveDataRequest.SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanCreateWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				GameId = game.Id,
				Key = "CanCreateWithoutActorId",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = SUGARClient.GameData.Add(SaveDataRequest);

			Assert.AreEqual(SaveDataRequest.ActorId, response.ActorId);
			Assert.AreEqual(SaveDataRequest.GameId, response.GameId);
			Assert.AreEqual(SaveDataRequest.Key, response.Key);
			Assert.AreEqual(SaveDataRequest.Value, response.Value);
			Assert.AreEqual(SaveDataRequest.SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CannotCreateWithoutKey()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(SaveDataRequest));
		}

		[Test]
		public void CannotCreateWithoutValue()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithoutKey",
				SaveDataType = SaveDataType.String,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(SaveDataRequest));
		}

		[Test]
		public void CannotCreateWithMismatchedData()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithMismatchedData",
				Value = "Test Value",
				SaveDataType = SaveDataType.Float,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Long;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Boolean;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Float;
			SaveDataRequest.Value = "True";

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Long;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Boolean;
			SaveDataRequest.Value = "2";

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(SaveDataRequest));
		}

		[Test]
		public void CanGetGameData()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "CanGetGameData");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "CanGetGameData");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameData",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = SUGARClient.GameData.Add(SaveDataRequest);

			var get = SUGARClient.GameData.Get(user.Id, game.Id, new [] { "CanGetGameData" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(get.First().ActorId, response.ActorId);
			Assert.AreEqual(get.First().GameId, response.GameId);
			Assert.AreEqual(get.First().Key, response.Key);
			Assert.AreEqual(get.First().Value, response.Value);
			Assert.AreEqual(get.First().SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanGetGameDataWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var SaveDataRequest = new SaveDataRequest
			{
				GameId = game.Id,
				Key = "CanGetGameDataWithoutActorId",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = SUGARClient.GameData.Add(SaveDataRequest);

			var get = SUGARClient.GameData.Get(null, game.Id, new string[] { "CanGetGameDataWithoutActorId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(get.First().ActorId, response.ActorId);
			Assert.AreEqual(get.First().GameId, response.GameId);
			Assert.AreEqual(get.First().Key, response.Key);
			Assert.AreEqual(get.First().Value, response.Value);
			Assert.AreEqual(get.First().SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanGetGameDataWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Get");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				Key = "CanGetGameDataWithoutGameId",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = SUGARClient.GameData.Add(SaveDataRequest);

			var get = SUGARClient.GameData.Get(user.Id, null, new string[] { "CanGetGameDataWithoutGameId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(get.First().ActorId, response.ActorId);
			Assert.AreEqual(get.First().GameId, response.GameId);
			Assert.AreEqual(get.First().Key, response.Key);
			Assert.AreEqual(get.First().Value, response.Value);
			Assert.AreEqual(get.First().SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanGetGameDataByMultipleKeys()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Get");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var SaveDataRequestOne = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys1",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var SaveDataRequestTwo = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys2",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var SaveDataRequestThree = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys3",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var responseOne = SUGARClient.GameData.Add(SaveDataRequestOne);
			var responseTwo = SUGARClient.GameData.Add(SaveDataRequestTwo);
			var responseThree = SUGARClient.GameData.Add(SaveDataRequestThree);

			var get = SUGARClient.GameData.Get(user.Id, game.Id, new string[] { "CanGetGameDatByMultipleKeys1", "CanGetGameDatByMultipleKeys2", "CanGetGameDatByMultipleKeys3" });

			Assert.AreEqual(3, get.Count());
			foreach (var g in get)
			{
				Assert.AreEqual("Test Value", g.Value);
			}
		}
	}
}