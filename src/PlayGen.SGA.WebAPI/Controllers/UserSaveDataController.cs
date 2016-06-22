using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.WebAPI.Exceptions;

namespace PlayGen.SGA.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates UserData specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class UserSaveDataController : Controller, IUserSaveDataController
    {
        private UserSaveDataDbController _userSaveDataDbController;

        public UserSaveDataController(UserSaveDataDbController userSaveDataDbController)
        {
            _userSaveDataDbController = userSaveDataDbController;
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
            var data = _userSaveDataDbController.Get(actorId, gameId, key);
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
            _userSaveDataDbController.Create(data);
            var dataContract = data.ToContract();
            return dataContract;
        }
    }
}
