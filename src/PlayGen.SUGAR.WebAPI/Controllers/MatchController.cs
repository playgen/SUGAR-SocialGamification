using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.ServerAuthentication.Extensions;
using PlayGen.SUGAR.WebAPI.Attributes;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web controller that facillitates Match specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class MatchController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.MatchController _matchCoreController;

		public MatchController(Core.Controllers.MatchController matchCoreController,
			IAuthorizationService authorizationService)
		{
			_matchCoreController = matchCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Method for admins to creates a match for a game they are not logged into.
		/// 
		/// Example Usage: GET api/match/create/1
		/// </summary>
		/// <param name="gameId"></param>
		/// <returns>The newly created <see cref="MatchResponse"/></returns>
		[HttpGet("create/{gameId:int}")]
		[Authorization(ClaimScope.Global, AuthorizationOperation.Create, AuthorizationOperation.Match)]
		public IActionResult Create(int gameId)
		{
			var userId = HttpContext.Request.Headers.GetUserId();

			if (_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
			{
				var match = _matchCoreController.Create(gameId, userId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Method to create a match for a game a user is currently logged into
		/// 
		/// Example Usage: GET api/match/create
		/// </summary>
		/// <returns>The newly created <see cref="MatchResponse"/></returns>
		[HttpGet("create")]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		public IActionResult Create()
		{
			var userId = HttpContext.Request.Headers.GetUserId();
			var gameId = HttpContext.Request.Headers.GetUserId();

			if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
			{
				var match = _matchCoreController.Create(gameId, userId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Method to create a match for a game a user is currently logged into which is also started automatically.
		/// 
		/// Example Usage: GET api/match/create
		/// </summary>
		/// <returns>The newly created <see cref="MatchResponse"/></returns>
		[HttpGet("createandstart")]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Create, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.Match)]
		public IActionResult CreateAndStart()
		{
			var gameId = HttpContext.Request.Headers.GetGameId();
			var userId = HttpContext.Request.Headers.GetUserId();

			if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result || 
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
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
		/// 
		/// Example Usage: GET api/match/1/start
		/// </summary>
		/// <param name="matchId"></param>
		/// <returns><see cref="MatchResponse"/></returns>
		[HttpGet("{matchId:int}/start")]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		public IActionResult Start(int matchId)
		{
			var gameId = HttpContext.Request.Headers.GetGameId();
			var userId = HttpContext.Request.Headers.GetUserId();

			if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
			{
				var match = _matchCoreController.Start(matchId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Ends a match for the game that the user is currently logged in for.
		/// 
		/// Example Usage: GET api/match/1/end
		/// </summary>
		/// <param name="matchId"></param>
		/// <returns><see cref="MatchResponse"/></returns>
		[HttpGet("{matchId:int}/end")]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		public IActionResult End([FromRoute]int matchId)
		{
			var gameId = HttpContext.Request.Headers.GetGameId();
			var userId = HttpContext.Request.Headers.GetUserId();

			if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
			{
				var match = _matchCoreController.End(matchId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Method for admins to end a match for a game they are not logged into.
		/// 
		/// Example Usage: GET api/match/1/end
		/// </summary>
		/// <param name="matchId"></param>
		/// <returns><see cref="MatchResponse"/></returns>
		[HttpGet("{gameId:int}/{matchId:int}/end")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationOperation.Update, AuthorizationOperation.Match)]
		public IActionResult End([FromRoute]int gameId, [FromRoute]int matchId)
		{
			var userId = HttpContext.Request.Headers.GetUserId();

			if (_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
			{
				var match = _matchCoreController.End(matchId);
				var contract = match.ToContract();
				return new ObjectResult(contract);
			}
			return Forbid();
		}

		/// <summary>
		/// Get a list of matches filtered by a time range.
		/// 
		/// Example Usage: GET api/match/2016-12-20T15:40:30/2016-12-20T16:50:40
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>A list of <see cref="MatchResponse"/></returns>
		[HttpGet("{start:datetime}/{end:datetime}")]
		public IActionResult GetByTime(DateTime? start, DateTime? end)
		{
			var matches = _matchCoreController.GetByTime(start, end);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches for a specific game.
		/// 
		/// Example Usage: GET api/match/game/1
		/// </summary>
		/// <param name="gameId"></param>
		/// <returns>A list of <see cref="MatchResponse"/></returns>
		[HttpGet("game/{gameId:int}")]
		[ArgumentsNotNull]
		public IActionResult GetByGame(int gameId)
		{
			var matches = _matchCoreController.GetByGame(gameId);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches for a specific game, filtered by a time range.
		/// 
		/// Example Usage: GET api/match/game/1/2016-12-20T15:40:30/2016-12-20T16:50:40
		/// </summary>
		/// <param name="gameId"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>A list of <see cref="MatchResponse"/></returns>
		[HttpGet("game/{gameId:int}/{start:datetime}/{end:datetime}")]
		public IActionResult GetByGame(int gameId, DateTime? start, DateTime? end)
		{
			var matches = _matchCoreController.GetByGame(gameId, start, end);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches that were created by a specific actor.
		/// 
		/// Example Usage: GET api/match/creator/1
		/// </summary>
		/// <param name="creatorId"></param>
		/// <returns>A list of <see cref="MatchResponse"/></returns>
		[HttpGet("creator/{creatorId:int}")]
		[ArgumentsNotNull]
		public IActionResult GetByCreator(int creatorId)
		{
			var matches = _matchCoreController.GetByCreator(creatorId);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches that were created by a specific actor, filtered by a time range.
		/// 
		/// Example Usage: GET api/match/creator/1/2016-12-20T15:40:30/2016-12-20T16:50:40
		/// </summary>
		/// <param name="creatorId"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>A list of <see cref="MatchResponse"/></returns>
		[HttpGet("creator/{creatorId:int}/{start:datetime}/{end:datetime}")]
		public IActionResult GetByCreator(int creatorId, DateTime? start, DateTime? end)
		{
			var matches = _matchCoreController.GetByCreator(creatorId, start, end);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches for a specific game created by a specific actor.
		/// 
		/// Example Usage: GET api/match/game/1/creator/1
		/// </summary>
		/// <param name="gameId"></param>
		/// <param name="creatorId"></param>
		/// <returns>A list of <see cref="MatchResponse"/></returns>
		[HttpGet("game/{gameId:int}/creator/{creatorId:int}")]
		[ArgumentsNotNull]
		public IActionResult GetByGameAndCreator(int gameId, int creatorId)
		{
			var matches = _matchCoreController.GetByGameAndCreator(gameId, creatorId);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Get a list of matches for a specific game created by a specific user, filtered by a time range.
		/// 
		/// Example Usage: GET api/match/game/1/creator/1/2016-12-20T15:40:30/2016-12-20T16:50:40
		/// </summary>
		/// <param name="gameId"></param>
		/// <param name="creatorId"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>A list of <see cref="MatchResponse"/></returns>
		[HttpGet("game/{gameId:int}/creator/{creatorId:int}/{start:datetime}/{end:datetime}")]
		public IActionResult GetByGameAndCreator(int gameId, int creatorId, DateTime? start, DateTime? end)
		{
			var matches = _matchCoreController.GetByGameAndCreator(gameId, creatorId, start, end);
			return new ObjectResult(matches.ToContractList());
		}

		/// <summary>
		/// Find a list of all MatchData that match the input parameters.
		/// 
		/// Example Usage: GET api/match/1/data
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
		/// 
		/// Example Usage: POST api/match/1/data
		/// </summary>
		/// <param name="newData"><see cref="EvaluationDataRequest"/> object that holds the details of the new Match Data.</param>
		/// <returns>A <see cref="EvaluationDataResponse"/> containing the new Match Data details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Create, AuthorizationOperation.Match)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.Match)]
		public IActionResult Add([FromBody]EvaluationDataRequest newData)
		{
			if (_authorizationService.AuthorizeAsync(User, newData.CreatingActorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, newData.CreatingActorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, newData.GameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
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
