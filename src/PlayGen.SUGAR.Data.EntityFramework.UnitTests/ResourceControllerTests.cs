using System;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class ResourceControllerTests : IClassFixture<TestEnvironment>
    {
		#region Configuration
	    private readonly ResourceController _resourceController;
		private readonly UserController _userController;
		private readonly GameController _gameController;

	    public ResourceControllerTests(TestEnvironment testEnvironment)
	    {
		    _resourceController = testEnvironment.ResourceController;
		    _userController = testEnvironment.UserController;
		    _gameController = testEnvironment.GameController;
	    }
		#endregion

		#region Tests
	    [Fact]
	    public void CanGetResourceByKey()
	    {
			var newResource = CreateGameData("CanGetExistingResourceByKey");

			var gotResources = _resourceController.Get(keys: new []{newResource.Key});

			Assert.True(gotResources.Count(r => IsMatch(r, newResource)) == 1);
		}

		[Fact]
		public void CanGetResourceActorId()
		{
			var newResource = CreateGameData("CanGetExistingResourceActorId", createNewUser: true);

			var gotResources = _resourceController.Get(actorId: newResource.ActorId);

			Assert.True(gotResources.Count(r => IsMatch(r, newResource)) == 1);
		}

		[Fact]
		public void CanGetResourceeGameId()
		{
			var newResource = CreateGameData("CanGetExistingResourceGameId", createNewGame: true);

			var gotResources = _resourceController.Get(gameId: newResource.GameId);

			Assert.True(gotResources.Count(r => IsMatch(r, newResource)) == 1);
		}

		[Fact]
		public void CanTransferResource_FromUserToUser()
		{
			var fromUser = GetOrCreateUser();
			var toUser = GetOrCreateUser();

			var fromResource = CreateGameData("CanTransferResource_FromUserToUser", actorId:fromUser.Id);

			long originalQuantity = long.Parse(fromResource.Value);
			long transferQuantity = originalQuantity/3;

			GameData toResource;
			_resourceController.Transfer(fromResource.Id, fromResource.GameId, toUser.Id, transferQuantity, out toResource);

			Assert.Equal(originalQuantity - transferQuantity, long.Parse(fromResource.Value));
			Assert.Equal(transferQuantity, long.Parse(toResource.Value));
			Assert.Equal(toUser.Id, toResource.ActorId.Value);
			Assert.Equal(fromResource.GameId.Value, toResource.GameId.Value);
		}

		[Fact]
		public void CanTransferExistingResource_FromUserToGame()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CanTransferExistingResource_FromUserToSystem()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CanTransferExistingResource_FromGameToGame()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CanTransferExistingResource_FromGameToUser()
		{
			throw new NotImplementedException();
		}

		public void CanTransferExistingResource_FromGameToSystem()
		{
			throw new NotImplementedException();
		}

		public void CanTransferExistingResource_FromSystemToGame()
		{
			throw new NotImplementedException();
		}

		public void CantTransferResource_FromSystemToUser()
		{
			throw new NotImplementedException();
		}

		public void CantTransferResource_FromUserToSameUser()
		{
			var user = GetOrCreateUser();
			var fromResource = CreateGameData("CantTransferResource_FromUserToSameUser", actorId:user.Id);
			
			long transferQuantity = long.Parse(fromResource.Value)/3;

			GameData toResource;
			_resourceController.Transfer(fromResource.Id, fromResource.GameId, user.Id, transferQuantity, out toResource);

			throw new NotImplementedException("assert should throw exception");
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(-2000)]
		public void CantTransferResource_FromUserToUserWithLessThan1Quantity(long transferQuantity)
		{
			var fromUser = GetOrCreateUser();
			var toUser = GetOrCreateUser();

			var fromResource = CreateGameData("CantTransferResource_FromUserToUserWithInvalidQuantity", actorId: fromUser.Id);

			long originalQuantity = long.Parse(fromResource.Value);

			GameData toResource;
			_resourceController.Transfer(fromResource.Id, fromResource.GameId, toUser.Id, transferQuantity, out toResource);

			throw new NotImplementedException("assert should throw exception");
		}

		[Fact]
		public void CantTransferResource_FromUserToUserWithOutOfRangeQUantity()
		{
			var fromUser = GetOrCreateUser();
			var toUser = GetOrCreateUser();

			var fromResource = CreateGameData("CantTransferResource_FromUserToUserWithInvalidQuantity", actorId: fromUser.Id);

			long originalQuantity = long.Parse(fromResource.Value);
			long transferQuantity = originalQuantity*2;

			GameData toResource;
			_resourceController.Transfer(fromResource.Id, fromResource.GameId, toUser.Id, transferQuantity, out toResource);

			throw new NotImplementedException("assert should throw exception");
		}
		#endregion

		#region Helpers
		private GameData CreateGameData(string key, int? gameId = null, int? actorId = null,
              bool createNewGame = false, bool createNewUser = false)
		{
			if (createNewGame)
			{
				var game = new Game
				{
					Name = key
				};
				_gameController.Create(game);
				gameId = game.Id;
			}

			if (createNewUser)
			{
				var user = new User
				{
					Name = key
				};
				_userController.Create(user);
				actorId = user.Id;
			}

			var resource = new GameData
			{
				GameId = gameId,
				ActorId = actorId,
				Key = key,
				Value = "100",
				DataType = GameDataType.Long,
				Category = GameDataCategory.Resource,
			};
			_resourceController.Create(resource);
			
			return resource;
		}
 
		private bool IsMatch(GameData lhs, GameData rhs)
		{
			return lhs.ActorId == rhs.ActorId
				&& lhs.GameId == rhs.GameId
				&& lhs.Category == rhs.Category
				&& lhs.DataType == rhs.DataType
				&& lhs.Key == rhs.Key
				&& lhs.Value == rhs.Value;
		}

		private Actor GetOrCreateUser()
		{
			string name = "ResourceControllerTests";
			var users = _userController.Search(name, true);
			User user;

			if (users.Any())
			{
				user = users.Single();
			}
			else
			{
				user = new User
				{
					Name = name,
				};

				_userController.Create(user);
			}

			return user;
		}
		
		#endregion
	}
}
