using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Contracts.Controllers;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Exceptions;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Game specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class GameController : Controller
	{
		private readonly Data.EntityFramework.Controllers.GameController _gameDbController;
		
		public GameController(Data.EntityFramework.Controllers.GameController gameDbController)
		{
			_gameDbController = gameDbController;
		}

		/// <summary>
		/// Find a list of all Games.
		/// 
		/// Example Usage: GET api/game/all
		/// </summary>
		/// <returns>A list of <see cref="GameResponse"/> that hold Game details.</returns>
		[HttpGet("list")]
		public IEnumerable<GameResponse> Get()
		{
			var game = _gameDbController.Get();
			var gameContract = game.ToContractList();
			return gameContract;
		}

		/// <summary>
		/// Find a list of Games that match <param name="name"/> provided.
		/// 
		/// Example Usage: GET api/game?name=game1&amp;name=game2
		/// </summary>
		/// <param name="name">Array of Game names</param>
		/// <returns>A list of <see cref="GameResponse"/> which match the search criteria.</returns>
		[HttpGet("find/{name}")]
		public IEnumerable<GameResponse> Find([FromRoute]string name)
		{
			var game = _gameDbController.Find(name);
			var gameContract = game.ToContractList();
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
		[HttpPost("create")]
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
		/// Example Usage: DELETE api/game?id=1&amp;id=2
		/// </summary>
		/// <param name="id">Array of Game IDs.</param>
		[HttpDelete("{id:int}")]
		public void Delete([FromRoute]int id)
		{
			_gameDbController.Delete(new [] {id});
		}
	}
}