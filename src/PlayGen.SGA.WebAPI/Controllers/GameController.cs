using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.WebAPI.ExtensionMethods;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private GameDbController _gameDbController;
        
        public GameController(GameDbController gameDbController)
        {
            _gameDbController = gameDbController;
        }

        // POST api/game/battleminions
        [HttpPost("{name}")]
        public int Create(string name)
        {
            var game = _gameDbController.Create(name);
            return game.Id;
        }

        // GET api/game/battleminions
        [HttpGet("{name}")]
        public Contracts.Game Get(string name)
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