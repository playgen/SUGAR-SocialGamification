using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class GameDbControllerTests : DbController
    {
        private readonly GameDbController _gameDbController;

        public GameDbControllerTests()
        {
            _gameDbController = new GameDbController(_nameOrConnectionString);
        }

        [Fact]
        public void CreateGame()
        {
            string gameName = "TestCreateGame";

            var newGame = new Game
            {
                Name = gameName,
            };

            _gameDbController.Create(newGame);

            var games = _gameDbController.Get(new string[] {gameName});

            int matches = games.Count(g => g.Name == newGame.Name);

            Assert.Equal(matches, 1);
        }

        [Fact]
        public void CreateDuplicateGame()
        {
            string gameName = "TestCreateDuplicateGame";

            var newGame = new Game
            {
                Name = gameName,
            };

            _gameDbController.Create(newGame);

            bool hadDuplicateException = false;

            try
            {
                _gameDbController.Create(newGame);
            }
            catch (DuplicateRecordException)
            {
                hadDuplicateException = true;
            }

            Assert.True(hadDuplicateException);
        }
    }
}
