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
	    public void CanGetExistingResourceByKey()
	    {
			var newResource = CreateGameData("CanGetExistingResourceByKey");

			var gotResources = _resourceController.Get(keys: new []{newResource.Key});

			Assert.True(gotResources.Count(r => IsMatch(r, newResource)) == 1);
		}

		[Fact]
		public void CanGetExistingResourceActorId()
		{
			var newResource = CreateGameData("CanGetExistingResourceActorId", createNewUser: true);

			var gotResources = _resourceController.Get(actorId: newResource.ActorId);

			Assert.True(gotResources.Count(r => IsMatch(r, newResource)) == 1);
		}

		[Fact]
		public void CanGetExistingResourceGameId()
		{
			var newResource = CreateGameData("CanGetExistingResourceGameId", createNewGame: true);

			var gotResources = _resourceController.Get(gameId: newResource.GameId);

			Assert.True(gotResources.Count(r => IsMatch(r, newResource)) == 1);
		}

		[Fact]
		public void CantGetNonexistantResource()
		{
			
		}
		#endregion

		#region Helpers

		private bool IsMatch(GameData lhs, GameData rhs)
		{
			return lhs.ActorId == rhs.ActorId
			       && lhs.GameId == rhs.GameId
			       && lhs.Category == rhs.Category
			       && lhs.DataType == rhs.DataType
				   && lhs.Key == rhs.Key
			       && lhs.Value == rhs.Value;
		}

		#region Helpers
		private GameData CreateGameData(string key, int? gameId = null, int? actorId = null,
			bool createNewGame = false, bool createNewUser = false)
		{
			if (createNewGame)
			{
				Game game = new Game
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
		#endregion
		#endregion
	}
}
