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

        // POST api/game/battleminions
        [HttpPost]
        public int Create([FromBody]Game newGame)
        {
            var game = _gameDbController.Create(newGame.ToModel());
            return game.Id;
        }

        // GET api/game/battleminions
        [HttpGet]
        public IEnumerable<Game> Get(string[] name)
        {
            var game = _gameDbController.Get(name);
            return game.ToContract();
        }

        // DELETE api/game/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _gameDbController.Delete(id);
        }
    }
}