using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.Model;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates GameData specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class GameDataController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.GameDataController _gameDataCoreController;

		public GameDataController(Core.Controllers.GameDataController gameDataCoreController, IAuthorizationService authorizationService)
		{
			_gameDataCoreController = gameDataCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Find a list of all GameData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// 
		/// Example Usage: GET api/gamedata?actorId=1&amp;gameId=1&amp;key=key1&amp;key=key2
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="EvaluationDataResponse"/> which match the search criteria.</returns>
		[HttpGet]
		[Authorization(ClaimScope.Group, AuthorizationAction.Get, AuthorizationEntity.GameData)]
		[Authorization(ClaimScope.User, AuthorizationAction.Get, AuthorizationEntity.GameData)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.GameData)]
		public async Task<IActionResult> Get(int? actorId, int? gameId, string[] key)
		{
			if (gameId.HasValue && actorId.HasValue)
			{
				if ((await _authorizationService.AuthorizeAsync(User, actorId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, actorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
				{
					var data = _gameDataCoreController.Get(gameId.Value, actorId.Value, key);
					var dataContract = data.ToContractList();
					return new ObjectResult(dataContract);
				}
			}
			return Forbid();
		}

		/// <summary>
		/// Find a list of all Actors that have data saved for the game <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/gamedata/gameactors/1
		/// </summary>
		/// <param name="id">ID of a Game.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("gameactors/{id:int}")]
		public IActionResult GetGameActors(int id)
		{
			var data = _gameDataCoreController.GetGameActors(id);
			var dataContract = data.ToActorContractList();
			return new ObjectResult(dataContract);
		}

		/// <summary>
		/// Find a list of all GameData keys for the <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/gamedata/gamekeys/1
		/// </summary>
		/// <param name="id">ID of a Game.</param>
		/// <returns>A list of GameData keys and their EvaluationDataType that has data saved for the provided game ID</returns>
		[HttpGet("gamekeys/{id:int}")]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.GameData)]
		public async Task<IActionResult> GetGameKeys(int id)
		{
			if ((await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var data = _gameDataCoreController.GetGameKeys(id);
				return new ObjectResult(data);
			}
			return Forbid();
		}

		/// <summary>
		/// Find GameData that matches the <param name="actorId"/>, <param name="gameId"/>, <param name="key"/>, <param name="dataType"/> and <param name="sortType"/> provided.
		/// 
		/// Example Usage: GET api/gamedata/leaderboardType/1/1/datakey/long/highest
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <param name="dataType">Data type of value</param>
		/// <param name="sortType"></param>
		/// <returns></returns>
		[HttpGet("leaderboardType/{actorId:int}/{gameId:int}/{key}/{dataType}/{sortType}")]
		[Authorization(ClaimScope.Group, AuthorizationAction.Get, AuthorizationEntity.GameData)]
		[Authorization(ClaimScope.User, AuthorizationAction.Get, AuthorizationEntity.GameData)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.GameData)]
		public async Task<IActionResult> GetByLeaderboardType([FromRoute]int actorId, [FromRoute]int gameId, [FromRoute]string key, [FromRoute]EvaluationDataType dataType, [FromRoute]LeaderboardType sortType)
		{
			if ((await _authorizationService.AuthorizeAsync(User, actorId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, actorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var data = _gameDataCoreController.Get(gameId, actorId, key, dataType, sortType);
				var dataContract = data.ToContract();
				return new ObjectResult(dataContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Create a new GameData record.
		/// 
		/// Example Usage: POST api/gamedata
		/// </summary>
		/// <param name="newData"><see cref="EvaluationDataRequest"/> object that holds the details of the new GameData.</param>
		/// <returns>A <see cref="EvaluationDataResponse"/> containing the new GameData details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationAction.Create, AuthorizationEntity.GameData)]
		[Authorization(ClaimScope.User, AuthorizationAction.Create, AuthorizationEntity.GameData)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Create, AuthorizationEntity.GameData)]
		public async Task<IActionResult> Add([FromBody]EvaluationDataRequest newData)
		{
			if ((await _authorizationService.AuthorizeAsync(User, newData.CreatingActorId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, newData.CreatingActorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, newData.GameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var data = newData.ToGameDataModel();
				_gameDataCoreController.Add(data);
				var dataContract = data.ToContract();
				return new ObjectResult(dataContract);
			}
			return Forbid();
		}

		// todo create method for adding batches of gamedata
	}
}