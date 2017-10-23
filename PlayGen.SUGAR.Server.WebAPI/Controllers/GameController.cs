using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common.Permissions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Game specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class GameController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.GameController _gameCoreController;

		public GameController(Core.Controllers.GameController gameCoreController,
					IAuthorizationService authorizationService)
		{
			_gameCoreController = gameCoreController;
			_authorizationService = authorizationService;
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
			var games = _gameCoreController.GetByPermissions(int.Parse(User.Identity.Name));
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
			var games = _gameCoreController.Search(name);
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
		public IActionResult GetById([FromRoute]int id)
		{  
			var game = _gameCoreController.Get(id);
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
		[Authorization(ClaimScope.Global, AuthorizationOperation.Create, AuthorizationOperation.Game)]
		public IActionResult Create([FromBody]GameRequest newGame)
		{
			if (_authorizationService.AuthorizeAsync(User, -1, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
			{
				var game = newGame.ToModel();
				_gameCoreController.Create(game, int.Parse(User.Identity.Name));
				var gameContract = game.ToContract();
				return new ObjectResult(gameContract);
			}
			return Forbid();
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
		[Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Game)]
		// todo refactor game request into GameUpdateRequest (which requires the Id) and GameCreateRequest (which has no required Id field) - and remove the Id param from the definition below
		public IActionResult Update([FromRoute] int id, [FromBody] GameRequest game)
		{
			if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
			{
				var gameModel = game.ToModel();
				gameModel.Id = id;
				_gameCoreController.Update(gameModel);
				return Ok();
			}
			return Forbid();
		}

		/// <summary>
		/// Delete Game with the ID provided.
		/// 
		/// Example Usage: DELETE api/game/1
		/// </summary>
		/// <param name="id">Game ID.</param>
		[HttpDelete("{id:int}")]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Delete, AuthorizationOperation.Game)]
		public IActionResult Delete([FromRoute]int id)
		{
			if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
			{
				_gameCoreController.Delete(id);
				return Ok();
			}
			return Forbid();
		}
	}
}