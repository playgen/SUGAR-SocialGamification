using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class GameController
    {
        private readonly Data.EntityFramework.Controllers.GameController _gameDbController;
        private readonly ActorRoleController _actorRoleController;

        public GameController(Data.EntityFramework.Controllers.GameController gameDbController,
                    ActorRoleController actorRoleController)
        {
            _gameDbController = gameDbController;
            _actorRoleController = actorRoleController;
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
        
        public Game Create(Game newGame, int creatorId)
        {
            newGame = _gameDbController.Create(newGame);
            _actorRoleController.Create(ClaimScope.Game.ToString(), creatorId, newGame.Id);
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
