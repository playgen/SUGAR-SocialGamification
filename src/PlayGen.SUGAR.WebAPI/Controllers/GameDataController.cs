using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates GameData specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	public class GameDataController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Data.EntityFramework.Controllers.GameDataController _gameDataCoreController;

		public GameDataController(Data.EntityFramework.Controllers.GameDataController gameDataCoreController,
					IAuthorizationService authorizationService)
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
		/// <returns>A list of <see cref="SaveDataResponse"/> which match the search criteria.</returns>
		[HttpGet]
		//[ResponseType(typeof(IEnumerable<SaveDataResponse>))]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.GameData)]
		public IActionResult Get(int? actorId, int? gameId, string[] key)
		{
			if (_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
			{
				var data = _gameDataCoreController.Get(gameId, actorId, key);
				var dataContract = data.ToContractList();
				return new ObjectResult(dataContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Create a new GameData record.
		/// 
		/// Example Usage: POST api/gamedata
		/// </summary>
		/// <param name="newData"><see cref="SaveDataRequest"/> object that holds the details of the new GameData.</param>
		/// <returns>A <see cref="SaveDataResponse"/> containing the new GameData details.</returns>
		[HttpPost]
		//[ResponseType(typeof(SaveDataResponse))]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.GameData)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Create, AuthorizationOperation.GameData)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.GameData)]
		public IActionResult Add([FromBody]SaveDataRequest newData)
		{
			if (_authorizationService.AuthorizeAsync(User, newData.ActorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, newData.ActorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, newData.GameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
			{
				var data = newData.ToGameDataModel();
				_gameDataCoreController.Create(data);
				var dataContract = data.ToContract();
				return new ObjectResult(dataContract);
			}
			return Forbid();
		}
	}
}