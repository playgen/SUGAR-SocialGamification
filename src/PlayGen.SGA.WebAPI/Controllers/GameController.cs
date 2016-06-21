using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.WebAPI.Exceptions;

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
        /// Get a list of all Games.
        /// 
        /// Example Usage: GET api/game/all
        /// </summary>
        /// <returns>A list of <see cref="GameResponse"/> that hold Game details.</returns>
        [HttpGet("all")]
        public IEnumerable<GameResponse> Get()
        {
            var game = _gameDbController.Get();
            var gameContract = game.ToContract();
            return gameContract;
        }

        /// <summary>
        /// Get a list of Games that match <param name="name"/> provided.
        /// 
        /// Example Usage: GET api/game?name=game1amp;name=game2
        /// </summary>
        /// <param name="name">Array of Game names</param>
        /// <returns>A list of <see cref="GameResponse"/> which match the search criteria.</returns>
        [HttpGet]
        public IEnumerable<GameResponse> Get(string[] name)
        {
            var game = _gameDbController.Get(name);
            var gameContract = game.ToContract();
            return gameContract;
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
            if (newGame == null) {
                throw new NullObjectException("Invalid object passed");
            }
            var game = newGame.ToModel();
            _gameDbController.Create(game);
            var gameContract = game.ToContract();
            return gameContract;
        }

        /// <summary>
        /// Delete Games with the IDs provided.
        /// 
        /// Example Usage: DELETE api/game?id=1amp;id=2
        /// </summary>
        /// <param name="id">Array of Game IDs.</param>
        [HttpDelete]
        public void Delete(int[] id)
        {
            _gameDbController.Delete(id);
        }
    }
}