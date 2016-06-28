using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Contracts.Controllers;
using PlayGen.SUGAR.WebAPI.ExtensionMethods;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Exceptions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates GroupData specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class GroupDataController : Controller
	{
		private readonly Data.EntityFramework.Controllers.GroupDataController _groupDataController;

		public GroupDataController(Data.EntityFramework.Controllers.GroupDataController groupDataController)
		{
			_groupDataController = groupDataController;
		}

		/// <summary>
		/// Get a list of all GroupData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// 
		/// Example Usage: GET api/groupsavedata?actorId=1&amp;gameId=1&amp;key=key1&amp;key=key2
		/// </summary>
		/// <param name="actorId">ID of a Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="SaveDataResponse"/> which match the search criteria.</returns>
		[HttpGet]
		[ResponseType(typeof(IEnumerable<SaveDataResponse>))]
		public IActionResult Get(int actorId, int gameId, string[] key)
		{
			var data = _groupDataController.Get(actorId, gameId, key);
			var dataContract = data.ToContract();
			return Ok(dataContract);
		}

		/// <summary>
		/// Create a new GroupData record.
		/// 
		/// Example Usage: POST api/groupsavedata
		/// </summary>
		/// <param name="newData"><see cref="SaveDataRequest"/> object that holds the details of the new GroupData.</param>
		/// <returns>A <see cref="SaveDataResponse"/> containing the new GroupData details.</returns>
		[HttpPost]
		[ResponseType(typeof(SaveDataResponse))]
		public IActionResult Add([FromBody]SaveDataRequest newData)
		{
			if (newData == null)
			{
				throw new NullObjectException("Invalid object passed");
			}
			var data = newData.ToGroupModel();
			_groupDataController.Create(data);
			var dataContract = data.ToContract();
			return Ok(dataContract);
		}
	}
}
