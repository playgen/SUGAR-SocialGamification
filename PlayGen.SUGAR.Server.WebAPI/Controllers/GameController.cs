using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common.Authorization;
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
	public class GameController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.GameController _gameCoreController;

		public GameController(Core.Controllers.GameController gameCoreController, IAuthorizationService authorizationService)
		{
			_gameCoreController = gameCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Get a list of all Games.
		/// </summary>
		/// <returns>A list of <see cref="GameResponse"/> that holds Game details.</returns>
		[HttpGet("list")]
		public IActionResult Get()
		{
			var games = _gameCoreController.Get();
			var gameContract = games.ToContractList();
			return new ObjectResult(gameContract);
		}

		/// <summary>
		/// Get a list of all Games the signed in User has control over.
		/// </summary>
		/// <returns>A list of <see cref="GameResponse"/> that holds Game details.</returns>
		[HttpGet("controlled")]
		public IActionResult GetControlled()
		{
			var games = _gameCoreController.GetByPermissions(int.Parse(User.Identity.Name));
			var gameContract = games.ToContractList();
			return new ObjectResult(gameContract);
		}

		/// <summary>
		/// Get a list of Games whose name contains the name provided.
		/// </summary>
		/// <param name="name">Game name</param>
		/// <returns>A list of <see cref="GameResponse"/> which match the search criteria.</returns>
		[HttpGet("find/{name}")]
		public IActionResult Get([FromRoute]string name)
		{
			var games = _gameCoreController.Search(name);
			var gameContract = games.ToContractList();
			return new ObjectResult(gameContract);
		}

		/// <summary>
		/// Get Game that matches the id provided.
		/// </summary>
		/// <param name="id">Game id</param>
		/// <returns><see cref="GameResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}", Name = "GetByGameId")]
		public IActionResult GetById([FromRoute]int id)
		{  
			var game = _gameCoreController.Get(id);
			var gameContract = game.ToContract();
			return new ObjectResult(gameContract);
		}

		/// <summary>
		/// Create a new Game.
		/// Requires the <see cref="GameRequest.Name"/> to be unique.
		/// </summary>
		/// <param name="newGame"><see cref="GameRequest"/> object that contains the details of the new Game.</param>
		/// <returns>A <see cref="GameResponse"/> containing the new Game details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationAction.Create, AuthorizationEntity.Game)]
		[Authorization(ClaimScope.User, AuthorizationAction.Create, AuthorizationEntity.Game)]
		public async Task<IActionResult> Create([FromBody]GameRequest newGame)
		{
			if ((await _authorizationService.AuthorizeAsync(User, Platform.AllId, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, int.Parse(User.Identity.Name), HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
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
		/// </summary>
		/// <param name="id">Id of the existing Game.</param>
		/// <param name="game"><see cref="GameRequest"/> object that holds the details of the Game.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Game, AuthorizationAction.Update, AuthorizationEntity.Game)]
		// todo refactor game request into GameUpdateRequest (which requires the Id) and GameCreateRequest (which has no required Id field) - and remove the Id param from the definition below
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] GameRequest game)
		{
			if ((await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
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
		/// </summary>
		/// <param name="id">Game ID.</param>
		[HttpDelete("{id:int}")]
		[Authorization(ClaimScope.Game, AuthorizationAction.Delete, AuthorizationEntity.Game)]
		public async Task<IActionResult> Delete([FromRoute]int id)
		{
			if ((await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				_gameCoreController.Delete(id);
				return Ok();
			}
			return Forbid();
		}
	}
}