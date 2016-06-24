using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Contracts.Controllers;
using PlayGen.SUGAR.WebAPI.ExtensionMethods;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Exceptions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates UserData specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class UserDataController : Controller, IUserDataController
	{
		private Data.EntityFramework.Controllers.UserDataController _userDataController;

		public UserDataController(Data.EntityFramework.Controllers.UserDataController userDataController)
		{
			_userDataController = userDataController;
		}

		/// <summary>
		/// Get a list of all UserData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// 
		/// Example Usage: GET api/usersavedata?actorId=1&amp;gameId=1&amp;key=key1&amp;key=key2
		/// </summary>
		/// <param name="actorId">ID of a User.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="SaveDataResponse"/> which match the search criteria.</returns>
		[HttpGet]
		public IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] key)
		{
			var data = _userDataController.Get(actorId, gameId, key);
			var dataContract = data.ToContract();
			return dataContract;
		}

		/// <summary>
		/// Create a new UserData record.
		/// 
		/// Example Usage: POST api/usersavedata
		/// </summary>
		/// <param name="newData"><see cref="SaveDataRequest"/> object that holds the details of the new UserData.</param>
		/// <returns>A <see cref="SaveDataResponse"/> containing the new UserData details.</returns>
		[HttpPost]
		public SaveDataResponse Add([FromBody]SaveDataRequest newData)
		{
			if (newData == null)
			{
				throw new NullObjectException("Invalid object passed");
			}
			var data = newData.ToUserModel();
			_userDataController.Create(data);
			var dataContract = data.ToContract();
			return dataContract;
		}
	}
}
