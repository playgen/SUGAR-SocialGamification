using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authentication.Extensions;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web controller that facillitates Match specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	public class MatchController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.MatchController _matchCoreController;

		public MatchController(Core.Controllers.MatchController matchCoreController, IAuthorizationService authorizationService)
		{
			_matchCoreController = matchCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Method for admins to creates a match for a game they are not logged into.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>The newly created <see cref="MatchResponse"/></returns>
		[HttpGet("create/{gameId:int}")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Create, AuthorizationEntity.Match)]
		public async Task<IActionResult> Create(int gameId)
		{
			if ((await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				var userId = HttpContext.Request.Headers.GetUserId();
				var match = _matchCoreController.Create(gameId, userId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Method to create a match for a game a user is currently logged into
		/// </summary>
		/// <returns>The newly created <see cref="MatchResponse"/></returns>
		[HttpGet("create")]
		[Authorization(ClaimScope.Group, AuthorizationAction.Update, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.User, AuthorizationAction.Update, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Update, AuthorizationEntity.Match)]
		public async Task<IActionResult> Create()
		{
			var userId = HttpContext.Request.Headers.GetUserId();
			var gameId = HttpContext.Request.Headers.GetGameId();
			if ((await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var match = _matchCoreController.Create(gameId, userId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Method to create a match for a game a user is currently logged into which is also started automatically.
		/// </summary>
		/// <returns>The newly created <see cref="MatchResponse"/></returns>
		[HttpGet("createandstart")]
		[Authorization(ClaimScope.Group, AuthorizationAction.Create, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.User, AuthorizationAction.Create, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Create, AuthorizationEntity.Match)]
		public async Task<IActionResult> CreateAndStart()
		{
			var gameId = HttpContext.Request.Headers.GetGameId();
			var userId = HttpContext.Request.Headers.GetUserId();
			if ((await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var match = _matchCoreController.Create(gameId, userId);
				match = _matchCoreController.Start(match.Id);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Start a match for the game the user is currently logged into.
		/// </summary>
		/// <param name="matchId">Match ID</param>
		/// <returns><see cref="MatchResponse"/>The current state of the started match</returns>
		[HttpGet("{matchId:int}/start")]
		[Authorization(ClaimScope.Group, AuthorizationAction.Update, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.User, AuthorizationAction.Update, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Update, AuthorizationEntity.Match)]
		public async Task<IActionResult> Start(int matchId)
		{
			var gameId = HttpContext.Request.Headers.GetGameId();
			var userId = HttpContext.Request.Headers.GetUserId();
			if ((await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var match = _matchCoreController.Start(matchId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Ends a match for the game that the user is currently logged in for.
		/// </summary>
		/// <param name="matchId">Match ID</param>
		/// <returns><see cref="MatchResponse"/>The current state of the ended match</returns>
		[HttpGet("{matchId:int}/end")]
		[Authorization(ClaimScope.Group, AuthorizationAction.Update, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.User, AuthorizationAction.Update, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Update, AuthorizationEntity.Match)]
		public async Task<IActionResult> End([FromRoute]int matchId)
		{
			var gameId = HttpContext.Request.Headers.GetGameId();
			var userId = HttpContext.Request.Headers.GetUserId();
			if ((await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, userId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var match = _matchCoreController.End(matchId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Method for admins to end a match for a game they are not logged into.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <param name="matchId">Match ID</param>
		/// <returns><see cref="MatchResponse"/>The current state of the ended match</returns>
		[HttpGet("{gameId:int}/{matchId:int}/end")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationAction.Update, AuthorizationEntity.Match)]
		public async Task<IActionResult> End([FromRoute]int gameId, [FromRoute]int matchId)
		{
			if ((await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				var match = _matchCoreController.End(matchId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Get a list of matches filtered by a time range.
		/// </summary>
		/// <param name="start">Earliest time</param>
		/// <param name="end">Latest time</param>
		/// <returns>A list of <see cref="MatchResponse"/> from the time range provided</returns>
		[HttpGet("{start:datetime}/{end:datetime}")]
		public IActionResult GetByTime(DateTime? start, DateTime? end)
		{
			var matches = _matchCoreController.GetByTime(start, end);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches for a specific game.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>A list of <see cref="MatchResponse"/> for the provided game</returns>
		[HttpGet("game/{gameId:int}")]
		[ArgumentsNotNull]
		public IActionResult GetByGame(int gameId)
		{
			var matches = _matchCoreController.GetByGame(gameId);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches for a specific game, filtered by a time range.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <param name="start">Earliest time</param>
		/// <param name="end">Latest time</param>
		/// <returns>A list of <see cref="MatchResponse"/> filtered by the time range and game</returns>
		[HttpGet("game/{gameId:int}/{start:datetime}/{end:datetime}")]
		public IActionResult GetByGame(int gameId, DateTime? start, DateTime? end)
		{
			var matches = _matchCoreController.GetByGame(gameId, start, end);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches that were created by a specific actor.
		/// </summary>
		/// <param name="creatorId">The Actor Id of the creator</param>
		/// <returns>A list of <see cref="MatchResponse"/> created by this user</returns>
		[HttpGet("creator/{creatorId:int}")]
		[ArgumentsNotNull]
		public IActionResult GetByCreator(int creatorId)
		{
			var matches = _matchCoreController.GetByCreator(creatorId);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches that were created by a specific actor, filtered by a time range.
		/// </summary>
		/// <param name="creatorId">The Actor Id of the creator</param>
		/// <param name="start">Earliest time</param>
		/// <param name="end">Latest time</param>
		/// <returns>A list of <see cref="MatchResponse"/> filtered by the time range and creator</returns>
		[HttpGet("creator/{creatorId:int}/{start:datetime}/{end:datetime}")]
		public IActionResult GetByCreator(int creatorId, DateTime? start, DateTime? end)
		{
			var matches = _matchCoreController.GetByCreator(creatorId, start, end);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches for a specific game created by a specific actor.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <param name="creatorId">The Actor Id of the creator</param>
		/// <returns>A list of <see cref="MatchResponse"/> filtered by game and creator</returns>
		[HttpGet("game/{gameId:int}/creator/{creatorId:int}")]
		[ArgumentsNotNull]
		public IActionResult GetByGameAndCreator(int gameId, int creatorId)
		{
			var matches = _matchCoreController.GetByGameAndCreator(gameId, creatorId);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches for a specific game created by a specific user, filtered by a time range.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <param name="creatorId">The Actor Id of the creator</param>
		/// <param name="start">Earliest time</param>
		/// <param name="end">Latest time</param>
		/// <returns>A list of <see cref="MatchResponse"/> filtered by time range, game and creator</returns>
		[HttpGet("game/{gameId:int}/creator/{creatorId:int}/{start:datetime}/{end:datetime}")]
		public IActionResult GetByGameAndCreator(int gameId, int creatorId, DateTime? start, DateTime? end)
		{
			var matches = _matchCoreController.GetByGameAndCreator(gameId, creatorId, start, end);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Find a list of all MatchData that match the input parameters.
		/// </summary>
		/// <param name="matchId">Id of a User/Group.</param>
		/// <param name="keys">Array of Key names.</param>
		/// <returns>A list of <see cref="EvaluationDataResponse"/> which match the search criteria.</returns>
		[HttpGet]
		public IActionResult Get(int matchId, string[] keys)
		{
			var data = _matchCoreController.GetData(matchId, keys);
			var dataContract = data.ToContractList();
			return new ObjectResult(dataContract);
		}

		/// <summary>
		/// Create a new GameData record.
		/// </summary>
		/// <param name="newData"><see cref="EvaluationDataRequest"/> object that holds the details of the new Match Data.</param>
		/// <returns>A <see cref="EvaluationDataResponse"/> containing the new Match Data details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationAction.Create, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.User, AuthorizationAction.Create, AuthorizationEntity.Match)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Create, AuthorizationEntity.Match)]
		public async Task<IActionResult> Add([FromBody]EvaluationDataRequest newData)
		{
			if ((await _authorizationService.AuthorizeAsync(User, newData.CreatingActorId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, newData.CreatingActorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, newData.GameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var data = newData.ToMatchDataModel();
				_matchCoreController.AddData(data);
				var dataContract = data.ToContract();
				return new ObjectResult(dataContract);
			}
			return Forbid();
		}
	}
}
