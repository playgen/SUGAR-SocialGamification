using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Permissions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.WebAPI.Attributes;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
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

		public GameDataController(Core.Controllers.GameDataController gameDataCoreController,
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
		/// <returns>A list of <see cref="EvaluationDataResponse"/> which match the search criteria.</returns>
		[HttpGet]
		//[ResponseType(typeof(IEnumerable<EvaluationDataResponse>))]
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
		/// Find a list of all Actors that have data saved for the game <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/gamedata/gameactors/1
		/// </summary>
		/// <param name="id">ID of a Game.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("gameactors/{id:int}")]
        //[ResponseType(typeof(IEnumerable<string>))]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.GameData)]
        public IActionResult GetGameActors(int? id)
        {
            if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var data = _gameDataCoreController.GetGameActors(id);
                var dataContract = data.ToActorContractList();
                return new ObjectResult(data);
            }
            return Forbid();
        }

        /// <summary>
		/// Find a list of all GameData keys for the <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/gamedata/gamekeys/1
		/// </summary>
		/// <param name="id">ID of a Game.</param>
		/// <returns>A list of GameData keys and their EvaluationDataType that has data saved for the provided game ID</returns>
		[HttpGet("gamekeys/{id:int}")]
        //[ResponseType(typeof(IEnumerable<string>))]
        public IActionResult GetGameKeys(int? id)
        {
            var data = _gameDataCoreController.GetGameKeys(id);
            return new ObjectResult(data);
        }

        /// <summary>
		/// Find a list of all GameData keys for the <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/gamedata/actor/1
		/// </summary>
		/// <param name="id">ID of a Game.</param>
		/// <returns>A list of <see cref="EvaluationDataResponse"/> that has data saved for the provided actor ID</returns>
		[HttpGet("actor/{id:int}")]
        //[ResponseType(typeof(IEnumerable<string>))]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Get, AuthorizationOperation.GameData)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Get, AuthorizationOperation.GameData)]
        public IActionResult GetActorData(int? id)
        {
            if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                 _authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result)
            {
                var data = _gameDataCoreController.GetActorData(id);
                var dataContract = data.ToContractList();
                return new ObjectResult(data);
            }
            return Forbid();
        }

        /// <summary>
        /// Finds a list of GameData with the highest <param name="dataType"/> for each <param name="key"/> provided that matches the <param name="actorId"/> and <param name="gameId"/>.
        /// 
        /// Example Usage: GET api/gamedata/highest?actorId=1&amp;gameId=1&amp;key=key1&amp;key=key2&amp;dataType=1
        /// </summary>
        /// <param name="actorId">ID of a User/Group.</param>
        /// <param name="gameId">ID of a Game.</param>
        /// <param name="key">Array of Key names.</param>
        /// <param name="dataType">Data type of value</param>
        /// <returns></returns>
        [HttpGet("highest")]
		//[ResponseType(typeof(IEnumerable<EvaluationDataResponse>))]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Get, AuthorizationOperation.GameData)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Get, AuthorizationOperation.GameData)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Get, AuthorizationOperation.GameData)]
		public IActionResult GetHighest(int? actorId, int? gameId, string[] key, EvaluationDataType dataType)
		{
			if (_authorizationService.AuthorizeAsync(User, actorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				 _authorizationService.AuthorizeAsync(User, actorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
				 _authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
			{
				var dataList = new List<EvaluationData>();
				switch (dataType)
				{
					case EvaluationDataType.Float:
						foreach (var dataKey in key)
						{
							var gameData = _gameDataCoreController.GetEvaluationDataByHighestFloat(gameId, actorId, dataKey);
							if (gameData != null)
							{
								dataList.Add(gameData);
							}
						}
						break;
					case EvaluationDataType.Long:
						foreach (var dataKey in key)
						{
							var gameData = _gameDataCoreController.GetEvaluationDataByHighestLong(gameId, actorId, dataKey);
							if (gameData != null)
							{
								dataList.Add(gameData);
							}
						}
						break;

				}
				var dataContract = dataList.ToContractList();
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
		//[ResponseType(typeof(EvaluationDataResponse))]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.GameData)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Create, AuthorizationOperation.GameData)]
		[Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.GameData)]
		public IActionResult Add([FromBody]EvaluationDataRequest newData)
		{
			if (_authorizationService.AuthorizeAsync(User, newData.CreatingActorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, newData.CreatingActorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, newData.GameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
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