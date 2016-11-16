using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class GameController
    {
        private readonly Data.EntityFramework.Controllers.GameController _gameDbController;
        private readonly Data.EntityFramework.Controllers.ActorRoleController _actorRoleController;
        private readonly Data.EntityFramework.Controllers.RoleController _roleController;

        public GameController(Data.EntityFramework.Controllers.GameController gameDbController,
                    Data.EntityFramework.Controllers.ActorRoleController actorRoleController,
                    Data.EntityFramework.Controllers.RoleController roleController)
        {
            _gameDbController = gameDbController;
            _actorRoleController = actorRoleController;
            _roleController = roleController;
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
            var role = _roleController.Get(ClaimScope.Game.ToString());
            if (role != null)
            {
                _actorRoleController.Create(new ActorRole { ActorId = creatorId, RoleId = role.Id, EntityId = newGame.Id });
                var admins = _actorRoleController.GetRoleActors(role.Id, 0);
                foreach (var admin in admins)
                {
                    _actorRoleController.Create(new ActorRole { ActorId = admin.Id, RoleId = role.Id, EntityId = newGame.Id });
                }
            }
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
