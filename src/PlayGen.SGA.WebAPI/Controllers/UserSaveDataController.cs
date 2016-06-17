using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserSaveDataController : Controller, IUserSaveDataController
    {
        private UserSaveDataDbController _userSaveDataDbController;

        public UserSaveDataController(UserSaveDataDbController userSaveDataDbController)
        {
            _userSaveDataDbController = userSaveDataDbController;
        }

        // POST api/usersavedata
        [HttpPost]
        public SaveDataResponse Add([FromBody]SaveDataRequest newData)
        {
            var data = _userSaveDataDbController.Create(newData.ToUserModel());
            return data.ToContract();
        }

        // GET api/usersavedata?actorId=1&gameId=1&key=key1&key=key2...
        [HttpGet]
        public IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] key)
        {
            var data = _userSaveDataDbController.Get(actorId, gameId, key);
            return data.ToContract();
        }
    }
}
