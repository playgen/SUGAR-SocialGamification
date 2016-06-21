using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates Game specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class GameController : Controller, IGameController
    {
        private readonly GameDbController _gameDbController;
        
        public GameController(GameDbController gameDbController)
        {
            _gameDbController = gameDbController;
        }

        /// <summary>
        /// GetByGame a list of all Games.
        /// 
        /// Example Usage: GET api/game/all
        /// </summary>
        /// <returns>A list of <see cref="GameResponse"/> that hold Game details.</returns>
        [HttpGet("all")]
        public IEnumerable<GameResponse> Get()
        {
            var game = _gameDbController.Get();
            return game.ToContract();
        }

        /// <summary>
        /// GetByGame a list of Games that match <param name="name"/> provided.
        /// 
        /// Example Usage: GET api/game?name=game1&name=game2
        /// </summary>
        /// <param name="name">Array of Game names</param>
        /// <returns>A list of <see cref="GameResponse"/> which match the search criteria.</returns>
        [HttpGet]
        public IEnumerable<GameResponse> Get(string[] name)
        {
            var game = _gameDbController.Get(name);
            return game.ToContract();
        }

        /// <summary>
        /// Create a new Game.
        /// Requires the <see cref="GameRequest.Name"/> to be unique.
        /// 
        /// Example Usage: POST api/game
        /// </summary>
        /// <param name="newGame"><see cref="GameRequest"/> object that contains the details of the new Game.</param>
        /// <returns>A <see cref="GameResponse"/> containing the new Game details.</returns>
        [HttpPost]
        public GameResponse Create([FromBody]GameRequest newGame)
        {
            var game = _gameDbController.Create(newGame.ToModel());
            return game.ToContract();
        }

        /// <summary>
        /// Delete Games with the IDs provided.
        /// 
        /// Example Usage: DELETE api/game?id=1&id=2
        /// </summary>
        /// <param name="id">Array of Game IDs.</param>
        [HttpDelete]
        public void Delete(int[] id)
        {
            _gameDbController.Delete(id);
        }
    }
}