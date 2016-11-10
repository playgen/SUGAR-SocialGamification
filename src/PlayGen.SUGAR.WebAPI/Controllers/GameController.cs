using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Game specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorization]
	public class GameController : Controller
	{
		private readonly Data.EntityFramework.Controllers.GameController _gameDbController;
		
		public GameController(Data.EntityFramework.Controllers.GameController gameDbController)
		{
			_gameDbController = gameDbController;
		}

		/// <summary>
		/// Get a list of all Games.
		/// 
		/// Example Usage: GET api/game/list
		/// </summary>
		/// <returns>A list of <see cref="GameResponse"/> that hold Game details.</returns>
		[HttpGet("list")]
		//[ResponseType(typeof(IEnumerable<GameResponse>))]
		public IActionResult Get()
		{
			var games = _gameDbController.Get();
			var gameContract = games.ToContractList();
			return new ObjectResult(gameContract);
		}

		/// <summary>
		/// Get a list of Games that match <param name="name"/> provided.
		/// 
		/// Example Usage: GET api/game/find/game1
		/// </summary>
		/// <param name="name">Game name</param>
		/// <returns>A list of <see cref="GameResponse"/> which match the search criteria.</returns>
		[HttpGet("find/{name}")]
		//[ResponseType(typeof(IEnumerable<GameResponse>))]
		public IActionResult Get([FromRoute]string name)
		{
			var games = _gameDbController.Search(name);
			var gameContract = games.ToContractList();
			return new ObjectResult(gameContract);
		}

		/// <summary>
		/// Get Game that matches <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/game/findbyid/1
		/// </summary>
		/// <param name="id">Game id</param>
		/// <returns><see cref="GameResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}", Name = "GetByGameId")]
		//[ResponseType(typeof(GameResponse))]
		public IActionResult Get([FromRoute]int id)
		{
			var game = _gameDbController.Search(id);
			var gameContract = game.ToContract();
			return new ObjectResult(gameContract);
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
		//[ResponseType(typeof(GameResponse))]
		[ArgumentsNotNull]
		public IActionResult Create([FromBody]GameRequest newGame)
		{
			var game = newGame.ToModel();
			_gameDbController.Create(game);
			var gameContract = game.ToContract();
			return new ObjectResult(gameContract);
		}

		/// <summary>
		/// Update an existing Game.
		/// 
		/// Example Usage: PUT api/game/update/1
		/// </summary>
		/// <param name="id">Id of the existing Game.</param>
		/// <param name="game"><see cref="GameRequest"/> object that holds the details of the Game.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		public void Update([FromRoute] int id, [FromBody] GameRequest game)
		{
			var gameModel = game.ToModel();
			gameModel.Id = id;
			_gameDbController.Update(gameModel);
		}

		/// <summary>
		/// Delete Game with the ID provided.
		/// 
		/// Example Usage: DELETE api/game/1
		/// </summary>
		/// <param name="id">Game ID.</param>
		[HttpDelete("{id:int}")]
		public void Delete([FromRoute]int id)
		{
			_gameDbController.Delete(id);
		}
	}
}