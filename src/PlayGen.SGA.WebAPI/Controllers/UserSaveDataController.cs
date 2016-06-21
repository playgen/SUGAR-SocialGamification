using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;

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
        /// GetByGame a list of all UserData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
        /// 
        /// Example Usage: GET api/usersavedata?actorId=1&gameId=1&key=key1&key=key2
        /// </summary>
        /// <param name="actorId">ID of a User.</param>
        /// <param name="gameId">ID of a Game.</param>
        /// <param name="key">Array of Key names.</param>
        /// <returns>A list of <see cref="SaveDataResponse"/> which match the search criteria.</returns>
        [HttpGet]
        public IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] key)
        {
            var data = _userSaveDataDbController.Get(actorId, gameId, key);
            return data.ToContract();
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
            var data = _userSaveDataDbController.Create(newData.ToUserModel());
            return data.ToContract();
        }
    }
}
