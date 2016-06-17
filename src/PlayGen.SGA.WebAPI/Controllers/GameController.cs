using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller, IGameController
    {
        private readonly GameDbController _gameDbController;
        
        public GameController(GameDbController gameDbController)
        {
            _gameDbController = gameDbController;
        }

        // GET api/game/all
        [HttpGet("all")]
        public IEnumerable<GameResponse> Get()
        {
            var game = _gameDbController.Get();
            return game.ToContract();
        }

        // GET api/game?name=game1&name=game2
        [HttpGet]
        public IEnumerable<GameResponse> Get(string[] name)
        {
            var game = _gameDbController.Get(name);
            return game.ToContract();
        }

        // POST api/game
        [HttpPost]
        public GameResponse Create([FromBody]GameRequest newGame)
        {
            var game = _gameDbController.Create(newGame.ToModel());
            return game.ToContract();
        }

        // DELETE api/game?id=1&id=2
        [HttpDelete]
        public void Delete(int[] id)
        {
            _gameDbController.Delete(id);
        }
    }
}