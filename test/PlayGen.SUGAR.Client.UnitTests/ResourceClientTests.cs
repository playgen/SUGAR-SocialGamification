using System;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using PlayGen.SUGAR.Contracts;
using NUnit.Framework;
using System.Linq;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class ResourceClientTests
	{
		#region Configuration
		private readonly ResourceClient _resourceClient;
		private readonly UserClient _userClient;
		private readonly GameClient _gameClient;

		public ResourceClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_resourceClient = testSugarClient.Resource;
			_userClient = testSugarClient.User;
			_gameClient = testSugarClient.Game;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "ResourceClientTests",
				Password = "ResourceClientTestsPassword",
				AutoLogin = true,
			};

			try
			{
				client.Login(accountRequest);
			}
			catch
			{
				client.Register(accountRequest);
			}
		}
		#endregion

		#region Tests
		[Test]
		public void CanCreate()
		{
			var user = GetOrCreateUser("Create");
			var game = GetOrCreateGame("Create");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanCreate",
				Quantity = 100,
			};

			var response = _resourceClient.AddOrUpdate(resourceRequest);

			Assert.AreEqual(resourceRequest.Key, response.Key);
			Assert.AreEqual(resourceRequest.Quantity, response.Quantity);
		}

		[Test]
		public void CanCreateWithoutGameId()
		{
			var user = GetOrCreateUser("Create");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				Quantity = 100,
			};

			var response = _resourceClient.AddOrUpdate(resourceRequest);

			Assert.AreEqual(resourceRequest.Key, response.Key);
			Assert.AreEqual(resourceRequest.Quantity, response.Quantity);
		}

		[Test]
		public void CanCreateWithoutActorId()
		{
			var game = GetOrCreateGame("Create");

			var resourceRequest = new ResourceAddRequest
			{
				GameId = game.Id,
				Key = "CanCreateWithoutActorId",
				Quantity = 100,
			};

			var response = _resourceClient.AddOrUpdate(resourceRequest);

			Assert.AreEqual(resourceRequest.Key, response.Key);
			Assert.AreEqual(resourceRequest.Quantity, response.Quantity);
		}

		[Test]
		public void CanUpdateExisting()
		{
			var resourceRequest = new ResourceAddRequest
			{
				Key = "CanUpdateExisting",
				Quantity = 100,
			};

			var createdResource = _resourceClient.AddOrUpdate(resourceRequest);
			var createdQuantity = createdResource.Quantity;
			var updatedQuantity = createdQuantity + 9000;

			//resourceRequest.Quantity = updatedQuantity;

			var resourceRequestUpdated = new ResourceAddRequest
			{
				Key = createdResource.Key,
				Quantity = updatedQuantity
			};

			_resourceClient.AddOrUpdate(resourceRequestUpdated);

			var updatedResource = _resourceClient.Get(createdResource.GameId, createdResource.ActorId,
				new[] {createdResource.Key}).Single();

			Assert.AreEqual(createdQuantity + updatedQuantity, updatedResource.Quantity);
			Assert.AreEqual(createdResource.Id, updatedResource.Id);
		}

		[Test]
		public void CanTransferCreateResource_FromUserToUser()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = fromUser.Id,
				Key = "CanTransferCreateResource_FromUserToUser",
				Quantity = 100,
			});

			var originalQuantity = fromResource.Quantity;
			var transferQuantity = originalQuantity/3;

			var transferResponse = _resourceClient.Transfer(new ResourceTransferRequest
			{
				GameId = fromResource.GameId,
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity
			});

			Assert.AreEqual(originalQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.AreEqual(transferQuantity, transferResponse.ToResource.Quantity);
			Assert.AreEqual(toUser.Id, transferResponse.ToResource.ActorId);
			Assert.AreEqual(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.AreEqual(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Test]
		public void CanTransferUpdateResource_FromUserToUser()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = fromUser.Id,
				Key = "CanTransferUpdateResource_FromUserToUser",
				Quantity = 100,
			});

			var toResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = fromResource.GameId,
				ActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = 50,
			});

			var originalFrmoQuantity = fromResource.Quantity;
			var originalToQuantity = toResource.Quantity;
			var transferQuantity = originalFrmoQuantity / 3;

			var transferResponse = _resourceClient.Transfer(new ResourceTransferRequest
			{
				GameId = fromResource.GameId,
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity
			});

			Assert.AreEqual(originalFrmoQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.AreEqual(originalToQuantity + transferQuantity, transferResponse.ToResource.Quantity);
			Assert.AreEqual(toUser.Id, transferResponse.ToResource.ActorId);
			Assert.AreEqual(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.AreEqual(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Test]
		public void CannotTransferNonExistingResource()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			Assert.Throws<Exception>(() => _resourceClient.Transfer(new ResourceTransferRequest
			{

				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = new Guid().ToString(),
				GameId = null,
				Quantity = 100,
			}));
		}

		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(-2000)]
		public void CannotTransfer_WithLessThan1Quantity(long transferQuantity)
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = null,
				Key = "CannotTransfer_WithLessThan1Quantity" + transferQuantity,
				Quantity = 100,
			});		

			Assert.Throws<Exception>(() => _resourceClient.Transfer(new ResourceTransferRequest
			{
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				GameId = fromResource.GameId,
				Quantity = transferQuantity
			}));
		}

		[Test]
		public void CannotTransfer_WithOutOfRangeQuantity()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = null,
				Key = "CannotTransfer_WithOutOfRangeQuantity",
				Quantity = 100,
			});

			long transferQuantity = fromResource.Quantity*2;

			Assert.Throws<Exception>(() => _resourceClient.Transfer(new ResourceTransferRequest
			{
				GameId = fromResource.GameId,
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity,
			}));
		}

		[Test]
		public void CanGetResource()
		{
			var user = GetOrCreateUser("Get");
			var game = GetOrCreateGame("Get");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetResource",
				Quantity = 100,
			};

			var response = _resourceClient.AddOrUpdate(resourceRequest);

			var get = _resourceClient.Get(game.Id, user.Id, new string[] { "CanGetResource" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(resourceRequest.Key, get.First().Key);
			Assert.AreEqual(resourceRequest.Quantity, get.First().Quantity);
		}

		[Test]
		public void CanGetResourceWithoutActorId()
		{
			var game = GetOrCreateGame("Get");

			var resourceRequest = new ResourceAddRequest
			{
				GameId = game.Id,
				Key = "CanGetResourceWithoutActorId",
				Quantity = 100,
			};

			var response = _resourceClient.AddOrUpdate(resourceRequest);

			var get = _resourceClient.Get(game.Id, null, new string[] { "CanGetResourceWithoutActorId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(resourceRequest.Key, get.First().Key);
			Assert.AreEqual(resourceRequest.Quantity, get.First().Quantity);
		}

		[Test]
		public void CanGetResourceWithoutGameId()
		{
			var user = GetOrCreateUser("Get");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				Key = "CanGetResourceWithoutGameId",
				Quantity = 100,
			};

			var response = _resourceClient.AddOrUpdate(resourceRequest);

			var get = _resourceClient.Get(null, user.Id, new string[] { "CanGetResourceWithoutGameId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(resourceRequest.Key, get.First().Key);
			Assert.AreEqual(resourceRequest.Quantity, get.First().Quantity);
		}

		[Test]
		public void CanGetResourceByMultipleKeys()
		{
			var user = GetOrCreateUser("Get");
			var game = GetOrCreateGame("Get");

			var resourceRequestOne = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetResourceByMultipleKeys1",
				Quantity = 100,
			};

			var resourceRequestTwo = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetResourceByMultipleKeys2",
				Quantity = 100,
			};

			var resourceRequestThree = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetResourceByMultipleKeys3",
				Quantity = 100,
			};

			var responseOne = _resourceClient.AddOrUpdate(resourceRequestOne);
			var responseTwo = _resourceClient.AddOrUpdate(resourceRequestTwo);
			var responseThree = _resourceClient.AddOrUpdate(resourceRequestThree);

			var get = _resourceClient.Get(game.Id, user.Id, new string[] { "CanGetResourceByMultipleKeys1", "CanGetResourceByMultipleKeys2", "CanGetResourceByMultipleKeys3" });

			Assert.AreEqual(3, get.Count());
			foreach (var r in get)
			{
				Assert.AreEqual(100, r.Quantity);
			}
		}
		#endregion

		#region Helpers
		private ActorResponse GetOrCreateUser(string suffix)
		{
			string name = "ResourceControllerTests" + suffix ?? $"_{suffix}";
			var users = _userClient.Get(name, true);
			ActorResponse user;

			if (users.Any())
			{
				user = users.Single();
			}
			else
			{
				user = _userClient.Create(new ActorRequest
				{
					Name = name
				});
			}

			return user;
		}

		private GameResponse GetOrCreateGame(string suffix)
		{
			string name = "ResourceControllerTests" + suffix ?? $"_{suffix}";
			var games = _gameClient.Get(name);
			GameResponse game;

			if (games.Any())
			{
				game = games.Single();
			}
			else
			{
				game = _gameClient.Create(new GameRequest
				{
					Name = name
				});
			}

			return game;
		}
		#endregion
	}
}