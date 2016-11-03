using System;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;

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
		[Fact]
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

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
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

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
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

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
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

			Assert.Equal(createdQuantity + updatedQuantity, updatedResource.Quantity);
			Assert.Equal(createdResource.Id, updatedResource.Id);
		}

		[Fact]
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

			Assert.Equal(originalQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(transferQuantity, transferResponse.ToResource.Quantity);
			Assert.Equal(toUser.Id, transferResponse.ToResource.ActorId);
			Assert.Equal(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.Equal(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Fact]
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

			Assert.Equal(originalFrmoQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(originalToQuantity + transferQuantity, transferResponse.ToResource.Quantity);
			Assert.Equal(toUser.Id, transferResponse.ToResource.ActorId);
			Assert.Equal(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.Equal(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Fact]
		public void CannotTransferNonExistingResource()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			Assert.Throws<ClientException>(() => _resourceClient.Transfer(new ResourceTransferRequest
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

			Assert.Throws<ClientException>(() => _resourceClient.Transfer(new ResourceTransferRequest
			{
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				GameId = fromResource.GameId,
				Quantity = transferQuantity
			}));
		}

		[Fact]
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

			Assert.Throws<ClientException>(() => _resourceClient.Transfer(new ResourceTransferRequest
			{
				GameId = fromResource.GameId,
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity,
			}));
		}

		[Fact]
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

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
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

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
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

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
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

			Assert.Equal(3, get.Count());
			foreach (var r in get)
			{
				Assert.Equal(100, r.Quantity);
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