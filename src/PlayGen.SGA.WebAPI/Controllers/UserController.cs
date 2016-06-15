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
    public class UserController : Controller, IUserController
    {
        private UserDbController _userDbController;

        public UserController(UserDbController userDbController)
        {
            _userDbController = userDbController;
        }

        // POST api/user/username
        [HttpPost]
        public int Create([FromBody]Actor actor)
        {
            var user = _userDbController.Create(actor.ToUserModel());
            return user.Id;
        }

        // GET api/user/username
        [HttpGet]
        public IEnumerable<Actor> Get(string[] name)
        {
            var users = _userDbController.Get(name);
            return users.ToContract();
        }

        // DELETE api/user/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userDbController.Delete(id);
        }
    }
}