using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GameControllerTests : IClassFixture<TestEnvironment>
	{
		#region Configuration
		private readonly GameController _gameDbController;

		public GameControllerTests(TestEnvironment testEnvironment)
		{
			_gameDbController = testEnvironment.GameController;
		}
		#endregion


		#region Tests
		[Fact]
		public void CreateAndGetGame()
		{
			string gameName = "CreateGame";

			CreateGame(gameName);
			
			var games = _gameDbController.Search(gameName);

			int matches = games.Count(g => g.Name == gameName);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateDuplicateGame()
		{
			string gameName = "CreateDuplicateGame";

			CreateGame(gameName);
			
			Assert.Throws<DuplicateRecordException>(() => CreateGame(gameName));
		}
		
		[Fact]
		public void GetMultipleGames()
		{
			string[] gameNames = new[]
			{
				"GetMultipleGames1",
				"GetMultipleGames2",
				"GetMultipleGames3",
				"GetMultipleGames4",
			};

			foreach (var gameName in gameNames)
			{
				CreateGame(gameName);
			}

			CreateGame("GetMultiple_Games_DontGetThis");

			var games = _gameDbController.Search("GetMultipleGames");

			var matchingGames = games.Select(g => gameNames.Contains(g.Name));
			
			Assert.Equal(gameNames.Length, matchingGames.Count());
		}

		[Fact]
		public void GetNonExistingGame()
		{
			var games = _gameDbController.Search("GetNonExistingGame");

			Assert.Empty(games);
		}

		[Fact]
		public void GetGameById()
		{
			Game newGame = CreateGame("GetGameById");

			int id = newGame.Id;

			var game = _gameDbController.Search(id);

			Assert.NotNull(game);
			Assert.Equal(newGame.Name, game.Name);
		}

		[Fact]
		public void GetNonExistingGameById()
		{
			var game = _gameDbController.Search(-1);

			Assert.Null(game);
		}

		[Fact]
		public void UpdateGame()
		{
			string gameName = "UpdateExistingGame";

			Game newGame = CreateGame(gameName);

			var games = _gameDbController.Search(gameName);

			int matches = games.Count(g => g.Name == gameName);

			Assert.Equal(1, matches);

			var updateGame = new Game
			{
				Id = newGame.Id,
				Name = "UpdateExistingGameProof"
			};

			_gameDbController.Update(updateGame);

			var updatedGame = _gameDbController.Search(newGame.Id);

			Assert.Equal("UpdateExistingGameProof", updatedGame.Name);
		}

		[Fact]
		public void UpdateGameToDuplicateName()
		{
			string gameName = "UpdateGameToDuplicateName";

			Game newGame = CreateGame(gameName);

			Game newGameDuplicate = CreateGame(gameName + " Two");

			var updateGame = new Game
			{
				Id = newGameDuplicate.Id,
				Name = newGame.Name
			};

			Assert.Throws<DuplicateRecordException>(() => _gameDbController.Update(updateGame));
		}

		[Fact]
		public void UpdateNonExistingGame()
		{
			var game = new Game
			{
				Id = -1,
				Name = "UpdateNonExistingGame"
			};

			Assert.Throws<MissingRecordException>(() => _gameDbController.Update(game));
		}

		[Fact]
		public void DeleteExistingGame()
		{
			string gameName = "DeleteExistingGame";

			var game = CreateGame(gameName);

			var games = _gameDbController.Search(gameName);
			Assert.Equal(games.Count(), 1);
			Assert.Equal(games.ElementAt(0).Name, gameName);

			_gameDbController.Delete(game.Id);
			games = _gameDbController.Search(gameName);

			Assert.Empty(games);
		}

		[Fact]
		public void DeleteNonExistingGame()
		{
			_gameDbController.Delete(-1);
		}
		#endregion

		#region Helpers
		private Game CreateGame(string name)
		{
			var game = new Game
			{
				Name = name,
			};

			_gameDbController.Create(game);

			return game;
		}
		#endregion
	}
}
