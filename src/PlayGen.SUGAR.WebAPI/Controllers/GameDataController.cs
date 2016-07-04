using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Contracts.Controllers;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;
using PlayGen.SUGAR.WebAPI.Exceptions;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates UserData specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorization]
	public class GameDataController : Controller
	{
		private readonly Data.EntityFramework.Controllers.GameDataController _gameDataController;

		public GameDataController(Data.EntityFramework.Controllers.GameDataController gameDataController)
		{
			_gameDataController = gameDataController;
		}

		/// <summary>
		/// Find a list of all GameData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// 
		/// Example Usage: GET api/gamedata?actorId=1&amp;gameId=1&amp;key=key1&amp;key=key2
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="GameDataResponse"/> which match the search criteria.</returns>
		[HttpGet]
		[ResponseType(typeof(IEnumerable<GameDataResponse>))]
		public IActionResult Get(int? actorId, int? gameId, string[] key)
		{
			var data = _gameDataController.Get(actorId, gameId, key);
			var dataContract = data.ToContractList();
			return new ObjectResult(dataContract);
		}

		/// <summary>
		/// Create a new GameData record.
		/// 
		/// Example Usage: POST api/gamedata
		/// </summary>
		/// <param name="newData"><see cref="GameDataRequest"/> object that holds the details of the new GameData.</param>
		/// <returns>A <see cref="GameDataResponse"/> containing the new GameData details.</returns>
		[HttpPost]
		[ResponseType(typeof(GameDataResponse))]
		[ArgumentsNotNull]
		public IActionResult Add([FromBody]GameDataRequest newData)
		{
			var data = newData.ToUserModel();
			_gameDataController.Create(data);
			var dataContract = data.ToContract();
			return new ObjectResult(dataContract);
		}
	}
}
