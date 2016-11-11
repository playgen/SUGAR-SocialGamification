using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class GameController
    {
        private readonly Data.EntityFramework.Controllers.GameController _gameDbController;

        public GameController(Data.EntityFramework.Controllers.GameController gameDbController)
        {
            _gameDbController = gameDbController;
        }

        public IEnumerable<Game> Get()
        {
            var games = _gameDbController.Get();
            return games;
        }

        public Game Get(int id)
        {
            var game = _gameDbController.Get(id);
            return game;
        }

        public IEnumerable<Game> Search(string name)
        {
            var games = _gameDbController.Search(name);
            return games;
        }
        
        public Game Create(Game newGame)
        {
            newGame = _gameDbController.Create(newGame);
            return newGame;
        }
         
        public void Update(Game game)
        {
            _gameDbController.Update(game);
        }

        public void Delete(int id)
        {
            _gameDbController.Delete(id);
        }
    }
}
