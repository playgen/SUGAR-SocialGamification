using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class GameController
	{
		public static event Action<int> GameDeletedEvent;

		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.GameController _gameDbController;
		private readonly ActorClaimController _actorClaimController;
		private readonly ActorRoleController _actorRoleController;

		public GameController(
			ILogger<GameController> logger,
			EntityFramework.Controllers.GameController gameDbController,
			ActorClaimController actorClaimController,
			ActorRoleController actorRoleController)
		{
			_logger = logger;
			_gameDbController = gameDbController;
			_actorClaimController = actorClaimController;
			_actorRoleController = actorRoleController;
		}

		public List<Game> Get()
		{
			var games = _gameDbController.Get();

			_logger.LogInformation($"Got: {games?.Count} Games");

			return games;
		}

		public List<Game> GetByPermissions(int actorId)
		{
			var games = Get();
			var permissions = _actorClaimController.GetActorClaimsByScope(actorId, ClaimScope.Game).Select(p => p.EntityId).ToList();
			games = games.Where(g => permissions.Contains(g.Id)).ToList();

			_logger.LogInformation($"Got: {games.Count} Games, for ActorId: {actorId}");

			return games;
		}

		public Game Get(int id)
		{
			var game = _gameDbController.Get(id);

			_logger.LogInformation($"Got: Game: {game?.Id}, for Id: {id}");

			return game;
		}

		public List<Game> Search(string name)
		{
			var games = _gameDbController.Search(name);

			_logger.LogInformation($"Got: {games?.Count} Games, for Name: {name}");

			return games;
		}

		public Game Create(Game newGame, int creatorId)
		{
			newGame = _gameDbController.Create(newGame);
			_actorRoleController.Create(ClaimScope.Game, creatorId, newGame.Id);

			_logger.LogInformation($"Created: Game: {newGame.Id}, for CreatorId: {creatorId}");

			return newGame;
		}

		public void Update(Game game)
		{
			_gameDbController.Update(game);

			_logger.LogInformation($"{game?.Id}");
		}

		public void Delete(int id)
		{
			GameDeletedEvent?.Invoke(id);

			_gameDbController.Delete(id);

			_logger.LogInformation($"{id}");
		}
	}
}
