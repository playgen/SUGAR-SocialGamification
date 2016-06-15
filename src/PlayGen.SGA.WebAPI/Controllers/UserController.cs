using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller, IUserController
    {
        private UserDbController _userDbController;

        public UserController(UserDbController userDbController)
        {
            _userDbController = userDbController;
        }

        // POST api/user/username
        [HttpPost("{name}")]
        public int Create(string name)
        {
            var user = _userDbController.Create(name);
            return user.Id;
        }

        // GET api/user/username
        [HttpGet("{name}")]
        public Contracts.Actor Get(string name)
        {
            var user = _userDbController.Get(name);
            return user.ToContract();
        }

        // DELETE api/user/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userDbController.Delete(id);
        }
    }
}