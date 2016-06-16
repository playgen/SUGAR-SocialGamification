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

        // POST api/usersavedata/
        [HttpPost]
        public void Add([FromBody]SaveData data)
        {
            _userSaveDataDbController.Create(data.ToUserModel());
        }

        // GET api/usersavedata/
        [HttpGet]
        public IEnumerable<SaveData> Get(int actorId, int gameId, string[] key)
        {
            var data = _userSaveDataDbController.Get(actorId, gameId, key);
            return data.ToContract();
        }
    }
}
