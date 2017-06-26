using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	[Collection("Project Fixture Collection")]
	public class GameControllerTests
	{
		private readonly GameController _gameController = ControllerLocator.GameController;

		private Game CreateGame(string name)
		{
			var game = new Game
			{
				Name = name
			};

			_gameController.Create(game);

			return game;
		}

		[Fact]
		public void CreateAndGetGame()
		{
			var gameName = "CreateGame";

			CreateGame(gameName);

			var games = _gameController.Search(gameName);

			var matches = games.Count(g => g.Name == gameName);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateDuplicateGame()
		{
			var gameName = "CreateDuplicateGame";

			CreateGame(gameName);

			Assert.Throws<DuplicateRecordException>(() => CreateGame(gameName));
		}

		[Fact]
		public void DeleteExistingGame()
		{
			var gameName = "DeleteExistingGame";

			var game = CreateGame(gameName);

			var games = _gameController.Search(gameName);
			Assert.Equal(games.Count(), 1);
			Assert.Equal(games.ElementAt(0)
					.Name,
				gameName);

			_gameController.Delete(game.Id);
			games = _gameController.Search(gameName);

			Assert.Empty(games);
		}

		[Fact]
		public void DeleteNonExistingGame()
		{
			_gameController.Delete(-1);
		}

		[Fact]
		public void GetGameById()
		{
			var newGame = CreateGame("GetGameById");

			var id = newGame.Id;

			var game = _gameController.Get(id);

			Assert.NotNull(game);
			Assert.Equal(newGame.Name, game.Name);
		}

		[Fact]
		public void GetMultipleGames()
		{
			var gameNames = new[]
			{
				"GetMultipleGames1",
				"GetMultipleGames2",
				"GetMultipleGames3",
				"GetMultipleGames4"
			};

			foreach (var gameName in gameNames)
				CreateGame(gameName);

			CreateGame("GetMultiple_Games_DontGetThis");

			var games = _gameController.Search("GetMultipleGames");

			var matchingGames = games.Select(g => gameNames.Contains(g.Name));

			Assert.Equal(gameNames.Length, matchingGames.Count());
		}

		[Fact]
		public void GetNonExistingGame()
		{
			var games = _gameController.Search("GetNonExistingGame");

			Assert.Empty(games);
		}

		[Fact]
		public void GetNonExistingGameById()
		{
			var game = _gameController.Get(-1);

			Assert.Null(game);
		}

		[Fact]
		public void UpdateGame()
		{
			var gameName = "UpdateExistingGame";

			var newGame = CreateGame(gameName);

			var games = _gameController.Search(gameName);

			var matches = games.Count(g => g.Name == gameName);

			Assert.Equal(1, matches);

			var updateGame = new Game
			{
				Id = newGame.Id,
				Name = "UpdateExistingGameProof"
			};

			_gameController.Update(updateGame);

			var updatedGame = _gameController.Get(newGame.Id);

			Assert.Equal("UpdateExistingGameProof", updatedGame.Name);
		}

		[Fact]
		public void UpdateGameToDuplicateName()
		{
			var gameName = "UpdateGameToDuplicateName";

			var newGame = CreateGame(gameName);

			var newGameDuplicate = CreateGame(gameName + " Two");

			var updateGame = new Game
			{
				Id = newGameDuplicate.Id,
				Name = newGame.Name
			};

			Assert.Throws<DuplicateRecordException>(() => _gameController.Update(updateGame));
		}

		[Fact]
		public void UpdateNonExistingGame()
		{
			var game = new Game
			{
				Id = -1,
				Name = "UpdateNonExistingGame"
			};

			Assert.Throws<MissingRecordException>(() => _gameController.Update(game));
		}
	}
}