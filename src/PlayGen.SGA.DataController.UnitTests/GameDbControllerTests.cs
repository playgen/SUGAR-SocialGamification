using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class GameDbControllerTests : TestDbController
    {
        #region Configuration
        private readonly GameDbController _gameDbController;

        public GameDbControllerTests()
        {
            _gameDbController = new GameDbController(_nameOrConnectionString);
        }
        #endregion


        #region Tests
        [Fact]
        public void CreateAndGetGame()
        {
            string gameName = "CreateGame";

            CreateGame(gameName);
            
            var games = _gameDbController.Get(new string[] {gameName});

            int matches = games.Count(g => g.Name == gameName);

            Assert.Equal(matches, 1);
        }

        [Fact]
        public void CreateDuplicateGame()
        {
            string gameName = "CreateDuplicateGame";

            CreateGame(gameName);
            
            bool hadDuplicateException = false;

            try
            {
                CreateGame(gameName);
            }
            catch (DuplicateRecordException)
            {
                hadDuplicateException = true;
            }

            Assert.True(hadDuplicateException);
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

            CreateGame("GetMultipleGames_DontGetThis");

            var games = _gameDbController.Get(gameNames);

            var matchingGames = games.Select(g => gameNames.Contains(g.Name));
            
            Assert.Equal(matchingGames.Count(), gameNames.Length);
        }

        [Fact]
        public void GetNonExistingGames()
        {
            var games = _gameDbController.Get(new string[] {"GetNonExsitingGames"});

            Assert.Empty(games);
        }

        [Fact]
        public void DeleteExistingGame()
        {
            string gameName = "DeleteExistingGame";

            var game = CreateGame(gameName);

            var games = _gameDbController.Get(new string[] { gameName });
            Assert.Equal(games.Count(), 1);
            Assert.Equal(games.ElementAt(0).Name, gameName);

            _gameDbController.Delete(new []{game.Id});
            games = _gameDbController.Get(new string[] { gameName });

            Assert.Empty(games);
        }

        [Fact]
        public void DeleteNonExistingGame()
        {
            bool hadExeption = false;

            try
            {
                _gameDbController.Delete(new int[] {-1});
            }
            catch (Exception)
            {
                hadExeption = true;
            }

            Assert.False(hadExeption);
        }
        #endregion

        #region Helpers
        private Game CreateGame(string name)
        {
            var newGame = new Game
            {
                Name = name,
            };

            return _gameDbController.Create(newGame);
        }
        #endregion
    }
}
