using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class ResourceClientTests : ClientTestBase
	{
		[Fact]
		public void CanCreate()
		{
			var key = "Resource_CanCreate";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Quantity = 100
			};

			var response = Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
		public void CannotCreateWithoutGameId()
		{
			var key = "Resource_CanCreateWithoutGameId";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = key,
				Quantity = 100
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest));
		}

		[Fact]
		public void CannotCreateWithoutActorId()
		{
			var key = "Resource_CannotCreateWithoutActorId";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var resourceRequest = new ResourceAddRequest
			{
				GameId = game.Id,
				Key = key,
				Quantity = 100
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest));
		}

		[Fact]
		public void CannotCreateWithoutQuantity()
		{
			var key = "Resource_CannotCreateWithoutQuantity";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest));
		}

		[Fact]
		public void CannotCreateWithoutKey()
		{
			var key = "Resource_CannotCreateWithoutKey";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Quantity = 100
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest));
		}

		[Fact]
		public void CanUpdateExisting()
		{
			var key = "Resource_CanUpdateExisting";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = key,
				Quantity = 100,
				GameId = game.Id
			};

			var createdResource = Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);
			var createdQuantity = createdResource.Quantity;
			var updatedQuantity = createdQuantity + 9000;

			var resourceRequestUpdated = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = createdResource.Key,
				Quantity = updatedQuantity,
				GameId = game.Id
			};

			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequestUpdated);

			var updatedResource = Fixture.SUGARClient.Resource.Get(createdResource.GameId, createdResource.ActorId,
				new[] {createdResource.Key}).Single();

			Assert.Equal(createdQuantity + updatedQuantity, updatedResource.Quantity);
		}

		[Fact]
		public void CanTransferCreateResourceFromUserToUser()
		{
			var key = "Resource_CanTransferCreateResourceFromUserToUser";
			var recipient = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var fromResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = Platform.GlobalGameId,
				Key = key,
				Quantity = 300
			});

			var originalQuantity = fromResource.Quantity;
			var transferQuantity = originalQuantity/3;

			var transferResponse = Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				GameId = Platform.GlobalGameId,
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity
			});

			Assert.Equal(originalQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(transferQuantity, transferResponse.ToResource.Quantity);
			Assert.Equal(recipient.Id, transferResponse.ToResource.ActorId);
			Assert.Equal(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.Equal(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Fact]
		public void CanTransferUpdateResourceFromUserToUser()
		{
			var key = "Resource_CanTransferUpdateResourceFromUserToUser";
			var recipient = CreateUser(key);
			var toResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = recipient.Id,
				GameId = Platform.GlobalGameId,
				Key = key,
				Quantity = 150
			});

			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var fromResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = Platform.GlobalGameId,
				Key = key,
				Quantity = 300
			});

			var originalFrmoQuantity = fromResource.Quantity;
			var originalToQuantity = toResource.Quantity;
			var transferQuantity = originalFrmoQuantity / 3;

			var transferResponse = Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				GameId = Platform.GlobalGameId,
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity
			});

			Assert.Equal(originalFrmoQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(originalToQuantity + transferQuantity, transferResponse.ToResource.Quantity);
			Assert.Equal(recipient.Id, transferResponse.ToResource.ActorId);
			Assert.Equal(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.Equal(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Fact]
		public void CannotTransferNonExistingResource()
		{
			var key = "Resource_CannotTransferNonExistingResource";
			var recipient = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = key,
				Quantity = 100
			}));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(-2000)]
		public void CannotTransferWithLessThan1Quantity(long transferQuantity)
		{
			var key = "Resource_CannotTransferWithLessThan1Quantity";
			var recipient = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = Platform.GlobalGameId,
				Key = (key + transferQuantity).Replace('-', '_'),
				Quantity = 300
			});		

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				GameId = Platform.GlobalGameId,
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = key,
				Quantity = transferQuantity
			}));
		}

		[Fact]
		public void CannotTransferWithOutOfRangeQuantity()
		{
			var key = "Resource_CannotTransferWithOutOfRangeQuantity";
			var recipient = CreateUser(key);
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var fromResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = Platform.GlobalGameId,
				Key = key,
				Quantity = 100
			});

			var transferQuantity = fromResource.Quantity*2;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				GameId = Platform.GlobalGameId,
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = key,
				Quantity = transferQuantity
			}));
		}

		[Fact]
		public void CanGetResource()
		{
			var key = "Resource_CanGetResource";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Quantity = 100
			};

			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = Fixture.SUGARClient.Resource.Get(game.Id, loggedInAccount.User.Id, new[] { key });

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
		public void CannotGetResourceWithoutActorId()
		{
			var key = "Resource_CannotGetResourceWithoutActorId";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.Get(game.Id, Platform.GlobalActorId, new[] { key }));
		}

		[Fact]
		public void CanGetGlobalResource()
		{
			var key = "Resource_CanGetResourceWithoutGameId";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = Platform.GlobalGameId,
				Key = key,
				Quantity = 100
			};

			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = Fixture.SUGARClient.Resource.Get(Platform.GlobalGameId, loggedInAccount.User.Id, new[] { key });

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
		public void CanGetResourceByMultipleKeys()
		{
			var key = "Resource_CanGetResourceByMultipleKeys";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var resourceRequestOne = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "1",
				Quantity = 100
			};

			var resourceRequestTwo = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "2",
				Quantity = 100
			};

			var resourceRequestThree = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "3",
				Quantity = 100
			};

			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequestOne);
			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequestTwo);
			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequestThree);

			var get = Fixture.SUGARClient.Resource.Get(game.Id, loggedInAccount.User.Id, new[] { resourceRequestOne.Key, resourceRequestTwo.Key, resourceRequestThree.Key });

			Assert.Equal(3, get.Count());
			foreach (var r in get)
			{
				Assert.Equal(100, r.Quantity);
			}
		}

		[Fact]
		public void UserCanTransferFromGroupToSelf()
		{
			// Arrange
			var key = "Resource_UserCanTransferFromGroupToSelf";
			var group = CreateGroup(key + "_Group");

			var groupInitialResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = group.Id,
				GameId = Platform.GlobalGameId,
				Key = key,
				Quantity = 150
			});

			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			Fixture.SUGARClient.GroupMember.CreateMemberRequest(new RelationshipRequest
			{
				AcceptorId = group.Id,
				RequestorId = loggedInAccount.User.Id,
				AutoAccept = true
			});

			var userInitialResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = Platform.GlobalGameId,
				Key = key,
				Quantity = 300
			});

			var originalFromQuantity = groupInitialResource.Quantity;
			var originalToQuantity = userInitialResource.Quantity;
			var transferQuantity = originalFromQuantity / 3;

			// Act
			var transferResponse = Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				GameId = Platform.GlobalGameId,
				SenderActorId = group.Id,
				RecipientActorId = loggedInAccount.User.Id,
				Key = groupInitialResource.Key,
				Quantity = transferQuantity
			});

			// Assert
			Assert.Equal(originalFromQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(originalToQuantity + transferQuantity, transferResponse.ToResource.Quantity);
			Assert.Equal(loggedInAccount.User.Id, transferResponse.ToResource.ActorId);
			Assert.Equal(groupInitialResource.GameId, transferResponse.FromResource.GameId);
			Assert.Equal(groupInitialResource.GameId, transferResponse.ToResource.GameId);
		}

        #region Helpers
		private GroupResponse CreateGroup(string key)
		{
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_Creator");

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			return Fixture.SUGARClient.Group.Create(groupRequest);
		}

		private UserResponse CreateUser(string key)
		{
			var friendAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_User");
			return friendAccount.User;
		}
		#endregion

		public ResourceClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}