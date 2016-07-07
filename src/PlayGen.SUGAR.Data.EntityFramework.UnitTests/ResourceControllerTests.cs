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
			var newResource = CreateResource("CanGetExistingResourceByKey");

			var gotResources = _resourceController.Get(keys: new []{newResource.Key});

			Assert.True(gotResources.Count(r => IsMatch(r, newResource)) == 1);
		}

		[Fact]
		public void CanGetResourceActorId()
		{
			var newResource = CreateResource("CanGetExistingResourceActorId", createNewUser: true);

			var gotResources = _resourceController.Get(actorId: newResource.ActorId);

			Assert.True(gotResources.Count(r => IsMatch(r, newResource)) == 1);
		}

		[Fact]
		public void CanGetResourceeGameId()
		{
			var newResource = CreateResource("CanGetExistingResourceGameId", createNewGame: true);

			var gotResources = _resourceController.Get(gameId: newResource.GameId);

			Assert.True(gotResources.Count(r => IsMatch(r, newResource)) == 1);
		}

		[Fact]
		public void CanUpdateResource()
		{
			var newResource = CreateResource("CanUpdateResource");

			string originalValue = newResource.Value;
			string newValue = originalValue + "999";
			newResource.Value = newValue;

			_resourceController.Update(newResource);

			var resources = _resourceController.Get(newResource.GameId, newResource.ActorId, new string[] {newResource.Key});
			newResource = resources.Single();

			Assert.Equal(newValue, newResource.Value);
		}

		[Fact]
		public void CanTransferCreate_FromUserToUser()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = CreateResource("CanTransfer_FromUserToUser", actorId:fromUser.Id);

			long originalQuantity = long.Parse(fromResource.Value);
			long transferQuantity = originalQuantity/3;

			var toResource = _resourceController.Transfer(fromResource.Id, fromResource.GameId, toUser.Id, transferQuantity, out fromResource);

			Assert.Equal(originalQuantity - transferQuantity, long.Parse(fromResource.Value));
			Assert.Equal(transferQuantity, long.Parse(toResource.Value));
			Assert.Equal(toUser.Id, toResource.ActorId);
			Assert.Equal(fromResource.GameId, toResource.GameId);
		}

		[Fact]
		public void CanTransferUpdate_FromUserToUser()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = CreateResource("CanTransfer_FromUserToUser", actorId: fromUser.Id);
			var existingToResource = CreateResource(fromResource.Key, actorId: toUser.Id);

			long originalQuantity = long.Parse(fromResource.Value);
			long transferQuantity = originalQuantity / 3;

			var processedToResource = _resourceController.Transfer(fromResource.Id, fromResource.GameId, toUser.Id, transferQuantity, out fromResource);

			Assert.Equal(originalQuantity - transferQuantity, long.Parse(fromResource.Value));
			Assert.Equal(originalQuantity + transferQuantity, long.Parse(processedToResource.Value));
			Assert.Equal(existingToResource.Id, processedToResource.Id);
		}
		#endregion

		#region Helpers
		private GameData CreateResource(string key, int? gameId = null, int? actorId = null,
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

		private Actor GetOrCreateUser(string suffix = null)
		{
			string name = "ResourceControllerTests" + suffix ?? $"_{suffix}";
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
